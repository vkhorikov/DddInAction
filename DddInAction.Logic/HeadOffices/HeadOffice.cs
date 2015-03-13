using System;

using DddInAction.Logic.Atms;
using DddInAction.Logic.Common;
using DddInAction.Logic.SnackMachines;


namespace DddInAction.Logic.HeadOffices
{
    public class HeadOffice : Entity
    {
        public virtual decimal Balance { get; protected set; }
        public virtual Money Cash { get; protected set; }


        public HeadOffice()
        {
            Cash = Money.None;
        }


        public virtual void ChangeBalance(decimal delta)
        {
            Balance += delta;
        }


        public virtual void AddCache(Money money)
        {
            Cash += money;
        }


        public virtual Result UnloadCashFromSnackMachine(SnackMachine snackMachine)
        {
            return Result.Ok()
                .OnSuccess(() => snackMachine.CanUnloadMoney())
                .OnSuccess(() => snackMachine.UnloadMoney())
                .OnSuccess(money => Cash += money);
        }


        public virtual void LoadCashToAtm(Atm atm)
        {
            atm.LoadMoney(Cash);
            Cash = Money.None;
        }
    }
}
