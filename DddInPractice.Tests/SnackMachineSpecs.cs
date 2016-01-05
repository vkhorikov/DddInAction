using System;
using System.Linq;
using DddInPractice.Logic;
using DddInPractice.Logic.SharedKernel;
using DddInPractice.Logic.SnackMachines;
using FluentAssertions;
using Xunit;
using static DddInPractice.Logic.SharedKernel.Money;
using static DddInPractice.Logic.SnackMachines.Snack;

namespace DddInPractice.Tests
{
    public class SnackMachineSpecs
    {
        [Fact]
        public void Return_money_empties_money_in_transaction()
        {
            var snackMachine = new SnackMachine();
            snackMachine.InsertMoney(Dollar);

            snackMachine.ReturnMoney();

            snackMachine.MoneyInTransaction.Should().Be(0m);
        }

        [Fact]
        public void Inserted_money_goes_to_money_in_transaction()
        {
            var snackMachine = new SnackMachine();

            snackMachine.InsertMoney(Cent);
            snackMachine.InsertMoney(Dollar);

            snackMachine.MoneyInTransaction.Should().Be(1.01m);
        }

        [Fact]
        public void Cannot_insert_more_than_one_coin_or_note_at_a_time()
        {
            var snackMachine = new SnackMachine();
            Money twoCent = Cent + Cent;

            Action action = () => snackMachine.InsertMoney(twoCent);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void BuySnack_trades_inserted_money_for_a_snack()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Chocolate, 10, 1m));
            snackMachine.InsertMoney(Dollar);

            snackMachine.BuySnack(1);

            snackMachine.MoneyInTransaction.Should().Be(0m);
            snackMachine.MoneyInside.Amount.Should().Be(1m);
            snackMachine.GetSnackPile(1).Quantity.Should().Be(9);
        }

        [Fact]
        public void Cannot_make_purchase_when_there_is_no_snacks()
        {
            var snackMachine = new SnackMachine();

            Action action = () => snackMachine.BuySnack(1);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Cannot_make_purchase_if_not_enough_money_inserted()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Chocolate, 1, 2m));
            snackMachine.InsertMoney(Dollar);

            Action action = () => snackMachine.BuySnack(1);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Snack_machine_returns_money_with_highest_denomination_first()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.LoadMoney(Dollar);

            snackMachine.InsertMoney(Quarter);
            snackMachine.InsertMoney(Quarter);
            snackMachine.InsertMoney(Quarter);
            snackMachine.InsertMoney(Quarter);
            snackMachine.ReturnMoney();

            snackMachine.MoneyInside.QuarterCount.Should().Be(4);
            snackMachine.MoneyInside.OneDollarCount.Should().Be(0);
        }

        [Fact]
        public void After_purchase_change_is_returned()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Chocolate, 1, 0.5m));
            snackMachine.LoadMoney(TenCent * 10);

            snackMachine.InsertMoney(Dollar);
            snackMachine.BuySnack(1);

            snackMachine.MoneyInside.Amount.Should().Be(1.5m);
            snackMachine.MoneyInTransaction.Should().Be(0m);
        }

        [Fact]
        public void Cannot_buy_snack_if_not_enough_change()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Chocolate, 1, 0.5m));
            snackMachine.InsertMoney(Dollar);

            Action action = () => snackMachine.BuySnack(1);

            action.ShouldThrow<InvalidOperationException>();
        }
    }
}
