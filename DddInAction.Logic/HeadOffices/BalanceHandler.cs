using System;

using DddInAction.Logic.Atms;
using DddInAction.Logic.Common;


namespace DddInAction.Logic.HeadOffices
{
    internal class BalanceHandler : IHandler<BalanceChangedEvent>
    {
        public void Handle(BalanceChangedEvent domainEvent)
        {
            var repository = new HeadOfficeRepository();
            HeadOffice headOffice = HeadOfficeInstance.Instance;
            headOffice.ChangeBalance(domainEvent.Delta);
            repository.Save(headOffice);
        }
    }
}
