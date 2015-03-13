using System;

using DddInAction.Logic.SnackMachines;

using FluentAssertions;

using Xunit;


namespace DddInAction.Tests.Terminals
{
    public class SnackPileSpecs
    {
        [Fact]
        public void Cannot_create_pile_with_negative_amount()
        {
            Action action = () => new SnackPile(Snack.Chocolate, -1, 1);
            action.ShouldBreakContract();
        }


        [Fact]
        public void Cannot_create_pile_with_zero_price()
        {
            Action action = () => new SnackPile(Snack.Chocolate, 1, 0);
            action.ShouldBreakContract();
        }


        [Fact]
        public void Piles_price_should_not_be_more_precise_than_one_cent()
        {
            Action action = () => new SnackPile(Snack.Chocolate, 1, 1.001m);
            action.ShouldBreakContract();
        }


        [Fact]
        public void Two_pile_with_the_same_amount_price_and_snack_should_be_equal()
        {
            SnackPile pile1 = new SnackPile(Snack.Chocolate, 1, 2m);
            SnackPile pile2 = new SnackPile(Snack.Chocolate, 1, 2m);

            pile1.Should().Be(pile2);
            pile1.GetHashCode().Should().Be(pile2.GetHashCode());
        }


        [Fact]
        public void Two_pile_with_different_amount_or_price_or_snack_should_not_be_equal()
        {
            SnackPile pile1 = new SnackPile(Snack.Chocolate, 1, 1m);
            SnackPile pile2 = new SnackPile(Snack.Chocolate, 1, 2m);

            pile1.Should().NotBe(pile2);
            pile1.GetHashCode().Should().NotBe(pile2.GetHashCode());
        }


        [Fact]
        public void Subcract_one_should_return_new_pile_with_new_amount()
        {
            SnackPile pile = new SnackPile(Snack.Chocolate, 1, 1m);

            SnackPile newPile = pile.SubtractOne();

            newPile.Snack.Should().Be(Snack.Chocolate);
            newPile.Price.Should().Be(1m);
            newPile.Amount.Should().Be(0);
        }


        [Fact]
        public void Cannot_sum_piles_with_different_snacks()
        {
            SnackPile pile1 = new SnackPile(Snack.Chocolate, 1, 1m);
            SnackPile pile2 = new SnackPile(Snack.Soda, 1, 1m);

            Action action = () =>
            {
                SnackPile newPile = pile1 + pile2;
            };

            action.ShouldBreakContract();
        }


        [Fact]
        public void Cannot_sum_piles_with_different_prices()
        {
            SnackPile pile1 = new SnackPile(Snack.Chocolate, 1, 2m);
            SnackPile pile2 = new SnackPile(Snack.Chocolate, 1, 1m);

            Action action = () =>
            {
                SnackPile newPile = pile1 + pile2;
            };

            action.ShouldBreakContract();
        }


        [Fact]
        public void Can_sum_pile_with_zero_amount_with_any_pile()
        {
            SnackPile pile1 = new SnackPile(Snack.Chocolate, 0, 1m);
            SnackPile pile2 = new SnackPile(Snack.Soda, 2, 3m);

            SnackPile newPile = pile1 + pile2;

            newPile.Snack.Should().Be(Snack.Soda);
            newPile.Amount.Should().Be(2);
            newPile.Price.Should().Be(3m);
        }


        [Fact]
        public void Sum_should_return_correct_snack_amount()
        {
            SnackPile pile1 = new SnackPile(Snack.Soda, 2, 1m);
            SnackPile pile2 = new SnackPile(Snack.Soda, 3, 1m);

            SnackPile newPile = pile1 + pile2;

            newPile.Snack.Should().Be(Snack.Soda);
            newPile.Amount.Should().Be(5);
            newPile.Price.Should().Be(1m);
        }
    }
}
