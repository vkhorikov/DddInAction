using System;
using System.Collections.Generic;
using System.Linq;

using DddInAction.Logic.Common;
using DddInAction.Logic.Utils;


namespace DddInAction.Logic.SnackMachines
{
    public class SnackMachine : Entity
    {
        public virtual Money MoneyInside { get; protected set; }
        public virtual decimal MoneyInTransaction { get; protected set; }
        public virtual IList<Slot> Slots { get; set; }

        public virtual int SlotNumber
        {
            get { return 3; }
        }


        public SnackMachine()
        {
            MoneyInside = Money.None;
            MoneyInTransaction = 0m;

            Slots = new List<Slot>();
            for (int i = 0; i < SlotNumber; i++)
            {
                Slots.Add(new Slot(this, i + 1));
            }
        }


        public virtual SnackPile GetSnackPile(int position)
        {
            return GetSlot(position).SnackPile;
        }


        public virtual IReadOnlyList<SnackPile> GetAllSnackPiles()
        {
            return Slots
                .OrderBy(x => x.Position)
                .Select(x => x.SnackPile)
                .ToList()
                .AsReadOnly();
        }


        private Slot GetSlot(int position)
        {
            Contracts.Require(
                position >= 1,
                position <= SlotNumber);

            return Slots.Single(x => x.Position == position);
        }


        public virtual void LoadSnacks(int position, SnackPile snackPile)
        {
            Slot slot = GetSlot(position);
            slot.SnackPile += snackPile;
        }


        public virtual void InsertMoney(Money money)
        {
            Money[] coinsAndNotes =
            {
                Money.Cent,
                Money.TenCent,
                Money.Quarter,
                Money.Dollar,
                Money.FiveDollar,
                Money.TwentyDollar
            };
            Contracts.Require(coinsAndNotes.Contains(money));

            MoneyInTransaction += money.Amount;
            MoneyInside += money;
        }


        public virtual Result CanBuySnack(int position)
        {
            SnackPile pile = GetSnackPile(position);

            if (pile.Amount == 0)
                return Result.Fail("The snack pile is empty");

            if (MoneyInTransaction < pile.Price)
                return Result.Fail("Not enough money");

            if (!MoneyInside.CanDevote(MoneyInTransaction - pile.Price))
                return Result.Fail("Not enough change");

            return Result.Ok();
        }


        public virtual void BuySnack(int position)
        {
            Contracts.Require(CanBuySnack(position).Success);

            Slot slot = GetSlot(position);
            slot.SnackPile = slot.SnackPile.SubtractOne();

            Money change = MoneyInside.Devote(MoneyInTransaction - slot.SnackPile.Price);
            MoneyInside -= change;
            MoneyInTransaction = 0;
        }


        public virtual void ReturnMoney()
        {
            Money moneyToReturn = MoneyInside.Devote(MoneyInTransaction);
            MoneyInside -= moneyToReturn;
            MoneyInTransaction = 0;
        }


        public virtual void LoadMoney(Money money)
        {
            MoneyInside += money;
        }


        public virtual Result CanUnloadMoney()
        {
            if (MoneyInTransaction > 0m)
                return Result.Fail("Cannot unload money: transaction in progress");

            return Result.Ok();
        }


        public virtual Money UnloadMoney()
        {
            Contracts.Require(CanUnloadMoney().Success);

            Money money = MoneyInside;
            MoneyInside = Money.None;
            return money;
        }
    }
}
