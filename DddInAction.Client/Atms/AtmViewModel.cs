using System;

using DddInAction.Client.Common;
using DddInAction.Logic.Atms;
using DddInAction.Logic.Common;
using DddInAction.Logic.SnackMachines;


namespace DddInAction.Client.Atms
{
    public class AtmViewModel : ViewModel
    {
        private readonly AtmRepository _repository;
        private readonly PaymentGateway _paymentGateway;
        private readonly Atm _atm;

        private string _message;
        public string Message
        {
            get { return _message; }
            private set
            {
                _message = value;
                Notify();
            }
        }

        public Money MoneyInside
        {
            get { return _atm.MoneyInside; }
        }

        public Command<decimal> TakeMoneyCommand { get; private set; }


        public AtmViewModel(Atm atm)
        {
            _repository = new AtmRepository();
            _atm = atm;
            _paymentGateway = new PaymentGateway();
            TakeMoneyCommand = new Command<decimal>(x => x > 0, TakeMoney);
        }


        private void TakeMoney(decimal amount)
        {
            Result.Ok()
                .OnSuccess(() => _atm.CanTakeMoney(amount))
                .OnSuccess(() => _atm.CaluculateAmountWithCommission(amount))
                .OnSuccess(toCharge => _paymentGateway.ChargePayment(toCharge))
                .OnSuccess(() => _atm.TakeMoney(amount))
                .OnSuccess(
                    () => _repository.Save(_atm)
                        .OnFailure(() => _paymentGateway.CancelLastPayment()))
                .OnBoth(result => NotifyMoneyTaken(result.Success, result.Error, amount));
        }


        private void NotifyMoneyTaken(bool success, string error, decimal amount)
        {
            if (success)
            {
                Message = "You have taken " + amount.ToString("C2");
                Notify(() => MoneyInside);
            }
            else
            {
                Message = error;
            }
        }
    }
}
