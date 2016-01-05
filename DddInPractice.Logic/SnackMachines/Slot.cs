using DddInPractice.Logic.Common;

namespace DddInPractice.Logic.SnackMachines
{
    public class Slot : Entity
    {
        public virtual SnackPile SnackPile { get; set; }
        public virtual SnackMachine SnackMachine { get; }
        public virtual int Position { get; }

        protected Slot()
        {
        }

        public Slot(SnackMachine snackMachine, int position)
            : this()
        {
            SnackMachine = snackMachine;
            Position = position;
            SnackPile = SnackPile.Empty;
        }
    }
}
