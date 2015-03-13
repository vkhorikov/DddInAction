using System;

using DddInAction.Logic.Common;


namespace DddInAction.Logic.Atms
{
    public class PaymentGateway
    {
        public Result ChargePayment(decimal amount)
        {
            try
            {
                CallToExternalService(amount);
                return Result.Ok();
            }
            catch (TimeoutException)
            {
                return Result.Fail("Could not charge the card");
            }
        }


        private void CallToExternalService(decimal amount)
        {
            if (new Random().Next() % 2 == 0)
                throw new TimeoutException();
        }


        public void CancelLastPayment()
        {
            CallToExternalService_2();
        }


        private void CallToExternalService_2()
        {
        }
    }
}
