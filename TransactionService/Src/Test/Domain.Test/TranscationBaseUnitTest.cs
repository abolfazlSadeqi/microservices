using Domain.Entities;

namespace Domain.Test
{
    public class TranscationBaseUnitTest
    {
        [Fact]
        public void Test_Create()
        {
            //arrange
            TranscationBase TranscationBase = new();

            int CustomerId = 1;
            long Amount = 10;
            DateTime Date = DateTime.Now;
            string DeviceType = "";

            //act
            var _TranscationBase = TranscationBase.CreateCustomer(CustomerId, Amount, Date, DeviceType);


            //assert
            Assert.NotNull(_TranscationBase);
            Assert.True(

              _TranscationBase.CustomerId.Equals(CustomerId) &&

                  _TranscationBase.Amount.Equals(Amount)
              && _TranscationBase.Date.Equals(Date)
                   && _TranscationBase.DeviceType.Equals(DeviceType)
                );
        }


        [Fact]
        public void Test_Update()
        {
            //arrange
            TranscationBase transcationBase = new();

            int CustomerId = 1;
            long Amount = 10;
            DateTime Date = DateTime.Now;
            string DeviceType = "";

            //act
            var _UpdatetranscationBase = transcationBase.UpdateCustomer(
                transcationBase, CustomerId, Amount, Date, DeviceType);


            //assert
            Assert.NotNull(_UpdatetranscationBase);
            Assert.True(_UpdatetranscationBase.CustomerId.Equals(CustomerId) &&

                  _UpdatetranscationBase.Amount.Equals(Amount)
              && _UpdatetranscationBase.Date.Equals(Date) 
                   && _UpdatetranscationBase.DeviceType.Equals(DeviceType)
                );
        }


    }
}