using System;

using DddInAction.Logic.Common;
using DddInAction.Logic.Utils;


namespace DddInAction.Logic.SnackMachines
{
    public class SnackPile : ValueObject<SnackPile>
    {
        public static readonly SnackPile Empty = new SnackPile(Snack.None, 0, 1);

        public Snack Snack { get; protected set; }
        public int Amount { get; protected set; }
        public decimal Price { get; protected set; }


        protected SnackPile()
        {
        }


        public SnackPile(Snack snack, int amount, decimal price)
            : this()
        {
            Contracts.Require(amount >= 0);
            Contracts.Require(
                price > 0,
                price % 0.01m == 0);

            Snack = snack;
            Amount = amount;
            Price = price;
        }


        protected override bool EqualsCore(SnackPile other)
        {
            return Snack == other.Snack
                && Amount == other.Amount
                && Price == other.Price;
        }


        protected override int GetHashCodeCore()
        {
            unchecked
            {
                int hashCode = Snack.GetHashCode();
                hashCode = (hashCode * 397) ^ Amount;
                hashCode = (hashCode * 397) ^ Price.GetHashCode();
                return hashCode;
            }
        }


        public SnackPile SubtractOne()
        {
            return new SnackPile(Snack, Amount - 1, Price);
        }


        public static SnackPile operator +(SnackPile pile1, SnackPile pile2)
        {
            Contracts.Require((pile1.Snack == pile2.Snack && pile1.Price == pile2.Price)
                || pile1.Amount == 0 || pile2.Amount == 0);

            if (pile1.Amount == 0)
                return pile2;

            if (pile2.Amount == 0)
                return pile1;

            return new SnackPile(pile1.Snack, pile1.Amount + pile2.Amount, pile1.Price);
        }
    }
}
