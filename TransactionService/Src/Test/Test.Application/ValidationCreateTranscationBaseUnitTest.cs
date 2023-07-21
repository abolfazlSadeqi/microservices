using Application.Common.Interfaces;
using Application.Customers.Commands.CreateCustomer;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using Shouldly;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Collections;

namespace Test.Application
{
    public class ValidationCreateTranscationBaseUnitTest
    {

          public ValidationCreateTranscationBaseUnitTest()
        {

            
        }


        [Fact]
        public void Test_ValidateRule()
        {
            //arrange

            CreateTranscationBaseCommand request = new CreateTranscationBaseCommand();

            //act
            CreateTranscationBaseCommandValidator validations = new CreateTranscationBaseCommandValidator();
            var _result = validations.Validate(request);


            //assert
            Assert.NotNull(_result);
            Assert.True(_result.IsValid);

            Assert.Equal(_result.Errors.Count(), 0);

        }


      

    }

   
}