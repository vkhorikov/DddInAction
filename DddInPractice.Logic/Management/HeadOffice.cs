using DddInPractice.Logic.Atms;
using DddInPractice.Logic.Common;
using DddInPractice.Logic.SharedKernel;
using DddInPractice.Logic.SnackMachines;

namespace DddInPractice.Logic.Management
{
    public class HeadOffice : AggregateRoot
    {
        public virtual decimal Balance { get; protected set; }
        public virtual Money Cash { get; protected set; } = Money.None;

        public virtual void ChangeBalance(decimal delta)
        {
            Balance += delta;
        }

        public virtual void UnloadCashFromSnackMachine(SnackMachine snackMachine)
        {
            Money money = snackMachine.UnloadMoney();
            Cash += money;
        }

        public virtual void LoadCashToAtm(Atm atm)
        {
            atm.LoadMoney(Cash);
            Cash = Money.None;
        }
    }
}
