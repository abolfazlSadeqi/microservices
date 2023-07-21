using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Customers.Commands.CreateCustomer;
using Application.TranscationBases.Commands.UpdateTranscationBase;
using Application.TranscationBases.Queries.GetTranscationBasesWithPagination;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<PublicApi.Startup>>
{
    private readonly HttpClient httpClient;

    public IntegrationTests(WebApplicationFactory<PublicApi.Startup> factory)
    {
        httpClient = factory.CreateClient();
    }


    [Fact]
    public async Task DeleteCustomer()
    {
        // arrange


        int id = 17;

        //act
        var result = await httpClient.DeleteAsync("/api/Customers/Delete/" + id);
        string resultContent = await result.Content.ReadAsStringAsync();


        //Assert
        Assert.NotNull(resultContent);


    }



    [Fact]
    public async Task CreateCustomer()
    {
        // arrange



        int CustomerId = 1;
        long Amount = 10;
        DateTime Date = DateTime.Now;
        string DeviceType = "";

        CreateTranscationBaseCommand command = new CreateTranscationBaseCommand();
        command.CustomerId = CustomerId;
        command.Amount = Amount;
        command.Date = Date;
        command.DeviceType = DeviceType;


        // act
        var jsonString = JsonConvert.SerializeObject(command);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var result = await httpClient.PostAsync("/api/Customers/Create", content);
        string resultContent = await result.Content.ReadAsStringAsync();


        //Assert
        Assert.NotNull(resultContent);
        Assert.True(int.Parse(resultContent) > 1);

    }


    [Fact]
    public async Task UpdateCustomer()
    {
        // arrange


        int Id = 17;

        int CustomerId = 1;
        long Amount = 10;
        DateTime Date = DateTime.Now;
        string DeviceType = "";

        UpdateTranscationBaseCommand command = new UpdateTranscationBaseCommand();
        command.CustomerId = CustomerId;
        command.Amount = Amount;
        command.Date = Date;
        command.DeviceType = DeviceType;
        command.Id = Id;


        // act
        var jsonString = JsonConvert.SerializeObject(command);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var result = await httpClient.PutAsync("/api/Customers/Update", content);
        string resultContent = await result.Content.ReadAsStringAsync();


        //Assert
        Assert.Equal(resultContent, Id.ToString());

    }




    [Fact]
    public async Task GetById()
    {
        //arrange 
        int Id = 11;
        var response = await httpClient.GetAsync("api/Customers/Get/" + Id);

        // Act
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var terms = System.Text.Json.JsonSerializer.Deserialize<TranscationBaseDto>(stringResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        //Assert
        Assert.Equal(Id, terms.Id);

    }




}