using System;
using DddInPractice.Logic.Common;
using DddInPractice.Logic.Management;
using DddInPractice.Logic.SharedKernel;
using static DddInPractice.Logic.SharedKernel.Money;

namespace DddInPractice.Logic.Atms
{
    public class Atm : AggregateRoot
    {
        private const decimal CommissionRate = 0.01m;

        public virtual Money MoneyInside { get; protected set; } = None;
        public virtual decimal MoneyCharged { get; protected set; }

        public virtual string CanTakeMoney(decimal amount)
        {
            if (amount <= 0m)
                return "Invalid amount";

            if (MoneyInside.Amount < amount)
                return "Not enough money";

            if (!MoneyInside.CanAllocate(amount))
                return "Not enough change";

            return string.Empty;
        }

        public virtual void TakeMoney(decimal amount)
        {
            if (CanTakeMoney(amount) != string.Empty)
                throw new InvalidOperationException();

            Money output = MoneyInside.Allocate(amount);
            MoneyInside -= output;

            decimal amountWithCommission = CaluculateAmountWithCommission(amount);
            MoneyCharged += amountWithCommission;

            AddDomainEvent(new BalanceChangedEvent(amountWithCommission));
        }

        public virtual decimal CaluculateAmountWithCommission(decimal amount)
        {
            decimal commission = amount * CommissionRate;
            decimal lessThanCent = commission % 0.01m;
            if (lessThanCent > 0)
            {
                commission = commission - lessThanCent + 0.01m;
            }
            return amount + commission;
        }

        public virtual void LoadMoney(Money money)
        {
            MoneyInside += money;
        }
    }
}
