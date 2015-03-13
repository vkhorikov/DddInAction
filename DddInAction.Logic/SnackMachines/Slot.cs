using System;

using DddInAction.Logic.Common;
using DddInAction.Logic.Utils;


namespace DddInAction.Logic.SnackMachines
{
    public class Slot : Entity
    {
        public virtual SnackPile SnackPile { get; protected internal set; }

        public virtual SnackMachine SnackMachine { get; set; }
        public virtual int Position { get; protected set; }


        protected Slot()
        {
        }


        public Slot(SnackMachine snackMachine, int position)
            : this()
        {
            Contracts.Require(
                position >= 1,
                position <= snackMachine.SlotNumber);

            SnackMachine = snackMachine;
            Position = position;
            SnackPile = SnackPile.Empty;
        }
    }
}
