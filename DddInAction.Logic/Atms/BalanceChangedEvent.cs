using System;

using DddInAction.Logic.Common;


namespace DddInAction.Logic.Atms
{
    public class BalanceChangedEvent : IDomainEvent
    {
        public decimal Delta { get; private set; }


        public BalanceChangedEvent(decimal delta)
        {
            Delta = delta;
        }
    }
}
