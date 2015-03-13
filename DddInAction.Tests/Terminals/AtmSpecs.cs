using System;
using System.Linq;

using DddInAction.Logic.Atms;
using DddInAction.Logic.Common;
using DddInAction.Logic.SnackMachines;

using FluentAssertions;

using Xunit;


namespace DddInAction.Tests.Terminals
{
    public class AtmSpecs
    {
        [Fact]
        public void New_atm_should_contain_nothing()
        {
            Atm atm = new Atm();
            atm.MoneyInside.Amount.Should().Be(0m);
        }


        [Fact]
        public void Loaded_money_should_go_to_money_inside()
        {
            Atm atm = new Atm();

            atm.LoadMoney(Money.Dollar);
            atm.LoadMoney(Money.Dollar);

            atm.MoneyInside.Amount.Should().Be(2m);
        }


        [Fact]
        public void Commission_should_be_one_percent()
        {
            Atm atm = new Atm();
            atm.CommissionRate.Should().Be(1);
        }


        [Fact]
        public void Take_money_should_exhange_money_with_commission()
        {
            Atm atm = new Atm();
            atm.LoadMoney(Money.Dollar);

            atm.TakeMoney(1m);

            atm.MoneyInside.Amount.Should().Be(0m);
            atm.ShouldContainBalanceChangedEvent(1.01m);
        }


        [Fact]
        public void Cannot_take_sum_more_percise_than_one_cent()
        {
            Atm atm = new Atm();
            atm.LoadMoney(Money.Dollar + Money.Cent);

            Action action = () => atm.TakeMoney(0.001m);

            action.ShouldBreakContract();
        }


        [Fact]
        public void Cannot_take_money_if_not_enough_change()
        {
            Atm atm = new Atm();
            atm.LoadMoney(Money.Dollar);

            Action action = () => atm.TakeMoney(0.01m);

            action.ShouldBreakContract();
        }


        [Fact]
        public void Cannot_take_money_if_not_enough_money()
        {
            Atm atm = new Atm();
            atm.LoadMoney(Money.Dollar);

            Action action = () => atm.TakeMoney(2m);

            action.ShouldBreakContract();
        }


        [Fact]
        public void Cannot_take_zero_amount()
        {
            Atm atm = new Atm();
            atm.LoadMoney(Money.Dollar);

            Action action = () => atm.TakeMoney(0m);

            action.ShouldBreakContract();
        }


        [Fact]
        public void Cannot_take_more_than_5000_dollars()
        {
            Atm atm = new Atm();
            atm.LoadMoney(new Money(1, 0, 0, 5000, 0, 0));

            Action action = () => atm.TakeMoney(5000.01m);

            action.ShouldBreakContract();
        }


        [Fact]
        public void Commission_shoud_be_at_least_one_cent()
        {
            Atm atm = new Atm();
            atm.LoadMoney(Money.Cent);

            atm.TakeMoney(0.01m);

            atm.ShouldContainBalanceChangedEvent(0.02m);
        }


        [Fact]
        public void Commission_shoud_be_rounded_up_to_the_next_cent()
        {
            Atm atm = new Atm();
            atm.LoadMoney(Money.Dollar + Money.TenCent);

            atm.TakeMoney(1.1m);

            atm.ShouldContainBalanceChangedEvent(1.12m);
        }
    }


    internal static class AtmExtensions
    {
        public static void ShouldContainBalanceChangedEvent(this Atm atm, decimal delta)
        {
            BalanceChangedEvent domainEvent = (BalanceChangedEvent)atm.DomainEvents
                .SingleOrDefault(x => x.GetType() == typeof(BalanceChangedEvent));

            domainEvent.Should().NotBeNull();
            domainEvent.Delta.Should().Be(delta);
        }
    }
}
