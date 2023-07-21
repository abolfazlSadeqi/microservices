using Application.Common.Models;
using Application.Customers.Commands.CreateCustomer;
using Application.Customers.Commands.DeleteCustomer;
using Application.TranscationBases.Commands.UpdateTranscationBase;
using Application.TranscationBases.Queries.GetTranscationBasesWithPagination;
using Common.HelperDistributedCache;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Models;
using PublicApi.Controllers.Base;
using System;

namespace PublicAPI.Controllers.Public;
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AdminLevel2")]
public class TranscationBasesController : ApiControllerBase
{
    private readonly ILogger<TranscationBasesController> _logger;
    private readonly IDistributedCache _DistributedCache;

    public TranscationBasesController(ILogger<TranscationBasesController> logger, IDistributedCache DistributedCache)
    {
        _logger = logger;
        _DistributedCache = DistributedCache;
    }




    [HttpGet]
    public async Task<ActionResult<PaginatedList<TranscationBaseDto>>> GetTranscationBasesWithPagination(
        [FromQuery] GetTranscationBasesWithPaginationQuery query) => await _GetTranscationBasesWithPagination(query);


    #region
    private async Task<ActionResult<PaginatedList<TranscationBaseDto>>> _GetTranscationBasesWithPagination([FromQuery] 
    GetTranscationBasesWithPaginationQuery query)
    {
        _logger.LogInformation("start_TranscationBase");
        

        if (!_DistributedCache.TryGetValue(ListCache.TranscationBaseCacheKey, out IEnumerable<TranscationBaseDto>? TranscationBaseDtos))
        {

            GetTranscationBasesWithPaginationQuery querynew = new GetTranscationBasesWithPaginationQuery();
            querynew.PageNumber = 1;

            querynew.PageSize = int.MaxValue;
            var _listTranscationBaseDtoalls = Mediator.Send(querynew);

            await _DistributedCache.SetAsync(ListCache.TranscationBaseCacheKey, _listTranscationBaseDtoalls.Result.Items);

            _logger.LogInformation("Count_TranscationBase =" + _listTranscationBaseDtoalls.Result.Items.Count);

            return await Mediator.Send(query);
        }

        var item = TranscationBaseDtos.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToList();
        PaginatedList<TranscationBaseDto> paginatedList = new PaginatedList<TranscationBaseDto>(
           item, TranscationBaseDtos.Count(), query.PageNumber, query.PageSize);

        //_logger.LogInformation("Count_TranscationBase=" + item.Count);


        return new ActionResult<PaginatedList<TranscationBaseDto>>(paginatedList);
    }

    #endregion

    [HttpGet("Get/{id:int}")]
    public async Task<ActionResult<TranscationBaseDto>> GetTranscationBase(int id)
    {
        if (!_DistributedCache.TryGetValue(ListCache.TranscationBaseCacheKey, out IEnumerable<TranscationBaseDto>? PersonDtos))
        {
            var _result= await Mediator.Send(new GetTranscationBaseQuery { Id = id });
            return _result;
        }

        return PersonDtos.FirstOrDefault(d => d.Id == id);


        ;
    }


    [HttpPost("Create")]
    public async Task<ActionResult<int>> Create(CreateTranscationBaseCommand command)
    {
        _DistributedCache.Remove(ListCache.TranscationBaseCacheKey);
        return await Mediator.Send(command);
    }



    [HttpPut("Update")]
    public async Task<ActionResult<int>> Update(UpdateTranscationBaseCommand command)
    {
        _DistributedCache.Remove(ListCache.TranscationBaseCacheKey);
        return await Mediator.Send(command);


    }

    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTranscationBaseCommand { Id = id });
        _DistributedCache.Remove(ListCache.TranscationBaseCacheKey);
        return Ok();
    }




}

