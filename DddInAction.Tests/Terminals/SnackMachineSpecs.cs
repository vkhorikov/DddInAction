using System;
using System.Collections.Generic;

using DddInAction.Logic.SnackMachines;

using FluentAssertions;

using Xunit;
using Xunit.Extensions;


namespace DddInAction.Tests.Terminals
{
    public class SnackMachineSpecs
    {
        [Fact]
        public void Snack_machine_should_have_three_slots()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.SlotNumber.Should().Be(3);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void Cannot_access_non_existant_slot(int position)
        {
            SnackMachine snackMachine = new SnackMachine();

            Action action = () => snackMachine.GetSnackPile(position);
            action.ShouldBreakContract();
        }


        [Fact]
        public void New_snack_machine_should_contain_nothing()
        {
            SnackMachine snackMachine = new SnackMachine();

            snackMachine.MoneyInTransaction.Should().Be(0m);
            snackMachine.MoneyInside.Amount.Should().Be(0m);
            for (int i = 0; i < snackMachine.SlotNumber; i++)
            {
                snackMachine.GetSnackPile(i + 1).Amount.Should().Be(0);
            }
        }


        [Fact]
        public void Cannot_insert_more_than_one_coin_or_note_at_a_time()
        {
            Money money = new Money(2, 0, 0, 0, 0, 0);
            SnackMachine snackMachine = new SnackMachine();

            Action action = () => snackMachine.InsertMoney(money);

            action.ShouldBreakContract();
        }


        [Fact]
        public void Inserted_money_should_go_to_money_in_transaction()
        {
            SnackMachine snackMachine = new SnackMachine();

            snackMachine.InsertMoney(Money.Cent);
            snackMachine.InsertMoney(Money.Dollar);

            snackMachine.MoneyInTransaction.Should().Be(1.01m);
        }


        [Fact]
        public void Loaded_money_should_go_to_money_inside()
        {
            SnackMachine snackMachine = new SnackMachine();

            snackMachine.LoadMoney(Money.TwentyDollar + Money.Quarter);
            snackMachine.LoadMoney(new Money(1, 1, 3, 0, 0, 0));

            snackMachine.MoneyInside.Amount.Should().Be(21.11m);
        }


        [Fact]
        public void Return_money_should_empty_money_in_transaction()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.InsertMoney(Money.Dollar);
            snackMachine.InsertMoney(Money.Cent);

            snackMachine.ReturnMoney();

            snackMachine.MoneyInside.Amount.Should().Be(0m);
        }


        [Fact]
        public void Return_money_should_not_affect_money_inside()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.InsertMoney(Money.Dollar);
            snackMachine.LoadMoney(Money.TwentyDollar);

            snackMachine.ReturnMoney();

            snackMachine.MoneyInside.Amount.Should().Be(20m);
        }


        [Fact]
        public void Snack_machine_should_return_notes_with_the_highest_denomination_first()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.LoadMoney(Money.Dollar);

            snackMachine.InsertMoney(Money.Quarter);
            snackMachine.InsertMoney(Money.Quarter);
            snackMachine.InsertMoney(Money.Quarter);
            snackMachine.InsertMoney(Money.Quarter);
            snackMachine.ReturnMoney();

            snackMachine.MoneyInside.OneDollarCount.Should().Be(0);
            snackMachine.MoneyInside.QuarterCount.Should().Be(4);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void Cannot_load_snack_into_non_existant_slot(int position)
        {
            SnackMachine snackMachine = new SnackMachine();

            Action action = () => snackMachine.LoadSnacks(position, SnackPile.Empty);
            action.ShouldBreakContract();
        }


        [Fact]
        public void Loading_snacks_loads_them_into_correct_slot()
        {
            SnackMachine snackMachine = new SnackMachine();

            snackMachine.LoadSnacks(2, new SnackPile(Snack.Chocolate, 1, 2m));

            snackMachine.GetSnackPile(2).Snack.Should().Be(Snack.Chocolate);
            snackMachine.GetSnackPile(2).Amount.Should().Be(1);
            snackMachine.GetSnackPile(2).Price.Should().Be(2m);
        }


        [Fact]
        public void Loading_snacks_should_sum_its_amount_with_existing_pile()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 5, 2m));

            snackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 1, 2m));

            snackMachine.GetSnackPile(1).Snack.Should().Be(Snack.Chocolate);
            snackMachine.GetSnackPile(1).Amount.Should().Be(6);
            snackMachine.GetSnackPile(1).Price.Should().Be(2m);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void Cannot_buy_snack_from_non_existant_slot(int position)
        {
            SnackMachine snackMachine = new SnackMachine();

            Action action = () => snackMachine.LoadSnacks(position, SnackPile.Empty);
            action.ShouldBreakContract();
        }


        [Fact]
        public void Cannot_buy_snack_if_there_is_no_snacks()
        {
            SnackMachine snackMachine = new SnackMachine();

            Action action = () => snackMachine.BuySnack(1);

            action.ShouldBreakContract();
        }


        [Fact]
        public void Cannot_buy_snack_if_not_enough_money_inserted()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 1, 2m));
            snackMachine.InsertMoney(Money.Dollar);

            Action action = () => snackMachine.BuySnack(1);

            action.ShouldBreakContract();
        }


        [Fact]
        public void Buying_snack_should_exhange_money_for_a_snack()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 2, 1m));
            snackMachine.InsertMoney(Money.Dollar);

            snackMachine.BuySnack(1);

            snackMachine.MoneyInTransaction.Should().Be(0);
            snackMachine.MoneyInside.Amount.Should().Be(1m);
            snackMachine.GetSnackPile(1).Amount.Should().Be(1);
        }


        [Fact]
        public void After_buying_snack_change_should_be_returned()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 1, 0.5m));
            snackMachine.LoadMoney(new Money(0, 10, 0, 0, 0, 0));
            snackMachine.InsertMoney(Money.Dollar);

            snackMachine.BuySnack(1);

            snackMachine.MoneyInside.Amount.Should().Be(1.5m);
        }


        [Fact]
        public void Cannot_buy_snack_if_not_enough_change()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 1, 0.5m));
            snackMachine.InsertMoney(Money.Dollar);

            Action action = () => snackMachine.BuySnack(1);

            action.ShouldBreakContract();
        }


        [Fact]
        public void Get_all_shack_piles_should_return_all_piles_ordered_by_position()
        {
            SnackMachine snackMachine = new SnackMachine();
            SnackPile pile1 = new SnackPile(Snack.Chocolate, 1, 1m);
            SnackPile pile2 = new SnackPile(Snack.Soda, 1, 1m);
            snackMachine.LoadSnacks(1, pile1);
            snackMachine.LoadSnacks(2, pile2);
            snackMachine.LoadSnacks(3, SnackPile.Empty);

            IReadOnlyList<SnackPile> snackPiles = snackMachine.GetAllSnackPiles();

            snackPiles.Count.Should().Be(3);
            snackPiles[0].Should().Be(pile1);
            snackPiles[1].Should().Be(pile2);
            snackPiles[2].Should().Be(SnackPile.Empty);
        }


        [Fact]
        public void Unload_money_unloads_all_money()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.LoadMoney(Money.FiveDollar);

            Money money = snackMachine.UnloadMoney();

            snackMachine.MoneyInside.Amount.Should().Be(0m);
            money.Amount.Should().Be(5m);
        }


        [Fact]
        public void Cannot_unload_money_if_transaction_is_in_progress()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.InsertMoney(Money.Dollar);

            Action action = () => snackMachine.UnloadMoney();

            action.ShouldBreakContract();
        }
    }
}
