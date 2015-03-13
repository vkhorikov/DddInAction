using System;
using System.Collections.Generic;
using System.Linq;

using DddInAction.Client.Common;
using DddInAction.Logic.Common;
using DddInAction.Logic.SnackMachines;


namespace DddInAction.Client.SnackMachines
{
    public class SnackMachineViewModel : ViewModel
    {
        private readonly SnackMachineRepository _repository;
        private readonly SnackMachine _machine;

        private string _message = " ";
        public string Message
        {
            get { return _message; }
            private set
            {
                _message = value;
                Notify();
            }
        }

        public string MoneyInTransaction
        {
            get { return _machine.MoneyInTransaction.ToString("C2"); }
        }

        public IReadOnlyList<SnackPileViewModel> Piles
        {
            get
            {
                return _machine.GetAllSnackPiles()
                    .Select(x => new SnackPileViewModel(x))
                    .ToList()
                    .AsReadOnly();
            }
        }

        public Money MoneyInside
        {
            get { return _machine.MoneyInside; }
        }

        public Command InsertCentCommand { get; private set; }
        public Command InsertTenCentCommand { get; private set; }
        public Command InsertQuarterCommand { get; private set; }
        public Command InsertDollarCommand { get; private set; }
        public Command InsertFiveDollarCommand { get; private set; }
        public Command InsertTwentyDollarCommand { get; private set; }
        public Command ReturnMoneyCommand { get; private set; }
        public Command<string> BuySnackCommand { get; private set; }


        public SnackMachineViewModel(SnackMachine machine)
        {
            _repository = new SnackMachineRepository();
            _machine = machine;

            InsertCentCommand = new Command(() => InsertMoney(Money.Cent));
            InsertTenCentCommand = new Command(() => InsertMoney(Money.TenCent));
            InsertQuarterCommand = new Command(() => InsertMoney(Money.Quarter));
            InsertDollarCommand = new Command(() => InsertMoney(Money.Dollar));
            InsertFiveDollarCommand = new Command(() => InsertMoney(Money.FiveDollar));
            InsertTwentyDollarCommand = new Command(() => InsertMoney(Money.TwentyDollar));
            ReturnMoneyCommand = new Command(() => ReturnMoney());
            BuySnackCommand = new Command<string>(BuySnack);
        }


        private void BuySnack(string positionString)
        {
            int position = int.Parse(positionString);

            Result.Ok()
                .OnSuccess(() => _machine.CanBuySnack(position))
                .OnSuccess(() => _machine.BuySnack(position))
                .OnSuccess(() => _repository.Save(_machine))
                .OnBoth(result => NotifyPurchase(result.Success, result.Error, position));
        }


        private void NotifyPurchase(bool success, string error, int position)
        {
            if (success)
            {
                Message = "You have bought snack #" + position;
                Notify(() => Piles);
                Notify(() => MoneyInTransaction);
                Notify(() => MoneyInside);
            }
            else
            {
                Message = error;
            }
        }


        private void ReturnMoney()
        {
            _machine.ReturnMoney();

            Message = "Money was returned";
            Notify(() => MoneyInTransaction);
            Notify(() => MoneyInside);
        }


        private void InsertMoney(Money coinOrNote)
        {
            _machine.InsertMoney(coinOrNote);

            Message = "You have inserted " + coinOrNote;
            Notify(() => MoneyInTransaction);
            Notify(() => MoneyInside);
        }
    }
}
