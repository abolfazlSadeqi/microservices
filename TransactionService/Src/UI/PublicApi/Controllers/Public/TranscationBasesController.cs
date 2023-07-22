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
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using PublicApi.Controllers.Base;
using System;
using System.Net.Http;

namespace PublicAPI.Controllers.Public;
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AdminLevel2")]
public class TranscationBasesController : ApiControllerBase
{
    private readonly ILogger<TranscationBasesController> _logger;
    private readonly IDistributedCache _DistributedCache;
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
   

    private readonly AsyncRetryPolicy _retryPolicy;

    public TranscationBasesController(ILogger<TranscationBasesController> logger, IDistributedCache DistributedCache
      )
    {
        _logger = logger;
        _DistributedCache = DistributedCache;
    

        _retryPolicy = Policy.Handle<Exception>().RetryAsync(2);
        if (_circuitBreakerPolicy == null)
        {
            _circuitBreakerPolicy = Policy.Handle<Exception>()
                                            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
        }

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

            var result = _circuitBreakerPolicy.ExecuteAsync(() => Mediator.Send(query));
            return await result;

           // return await Mediator.Send(query);
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
            var result = _retryPolicy.ExecuteAsync(() => Mediator.Send(new GetTranscationBaseQuery { Id = id }));
            return await result;

            //return await _fallbackPolicy.ExecuteAsync(async () =>
            //{
            //    Mediator.Send(new GetTranscationBaseQuery { Id = id });

            //});

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

