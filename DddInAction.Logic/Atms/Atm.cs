using System;

using DddInAction.Logic.Common;
using DddInAction.Logic.SnackMachines;
using DddInAction.Logic.Utils;


namespace DddInAction.Logic.Atms
{
    public class Atm : Entity
    {
        public virtual Money MoneyInside { get; protected set; }

        public virtual decimal CommissionRate
        {
            get { return 1; }
        }


        public Atm()
        {
            MoneyInside = Money.None;
        }


        public virtual void LoadMoney(Money money)
        {
            MoneyInside += money;
        }


        public virtual Result CanTakeMoney(decimal amount)
        {
            if (amount == 0m)
                return Result.Fail("Cannot take 0");

            if (MoneyInside.Amount < amount)
                return Result.Fail("Not enough money");

            if (amount > 5000m)
                return Result.Fail("The sum is loo large");

            if (!MoneyInside.CanDevote(amount))
                return Result.Fail("Not enough change");

            return Result.Ok();
        }


        public virtual void TakeMoney(decimal amount)
        {
            Contracts.Require(CanTakeMoney(amount).Success);

            Money output = MoneyInside.Devote(amount);
            MoneyInside -= output;

            decimal amountWithCommission = CaluculateAmountWithCommission(amount);
            AddDomainEvent(new BalanceChangedEvent(amountWithCommission));
        }


        public virtual decimal CaluculateAmountWithCommission(decimal amount)
        {
            decimal commission = amount * CommissionRate / 100;
            decimal lessThanCent = commission % 0.01m;
            if (lessThanCent > 0)
            {
                commission = commission - lessThanCent + 0.01m;
            }
            return amount + commission;
        }
    }
}
