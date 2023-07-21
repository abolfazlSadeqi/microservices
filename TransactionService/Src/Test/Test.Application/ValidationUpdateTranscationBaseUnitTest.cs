
using Application.TranscationBases.Commands.UpdateTranscationBase;

namespace Test.Application
{
    public class ValidationUpdateTranscationBaseUnitTest
    {

          public ValidationUpdateTranscationBaseUnitTest()
        {

            
        }


        [Fact]
        public void Test_ValidateRule()
        {
            //arrange

            UpdateTranscationBaseCommand request = new UpdateTranscationBaseCommand();

            request.CustomerId = 1;
            request.Amount = 200;
            request.DeviceType = "1";
            request.Date = DateTime.Now.Date;


            //act
            UpdateTranscationBaseCommandValidator validations = new UpdateTranscationBaseCommandValidator();
            var _result = validations.Validate(request);


            //assert
            Assert.NotNull(_result);
            Assert.True(_result.IsValid);

            Assert.Equal(_result.Errors.Count(), 0);

        }


      

    }

   
}