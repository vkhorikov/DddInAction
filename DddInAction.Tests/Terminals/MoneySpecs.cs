using System;

using DddInAction.Logic.SnackMachines;

using FluentAssertions;

using Xunit;
using Xunit.Extensions;


namespace DddInAction.Tests.Terminals
{
    public class MoneySpecs
    {
        [Fact]
        public void Empty_amount_should_be_zero()
        {
            Money money = Money.None;

            money.Amount.Should().Be(0m);
        }


        [Fact]
        public void One_cent_should_be_one_cent()
        {
            Money oneCent = Money.Cent;

            oneCent.Amount.Should().Be(0.01m);
            oneCent.OneCentCount.Should().Be(1);
        }


        [Fact]
        public void Ten_cent_should_be_ten_cent()
        {
            Money tenCent = Money.TenCent;

            tenCent.Amount.Should().Be(0.1m);
            tenCent.TenCentCount.Should().Be(1);
        }


        [Fact]
        public void Quarter_should_be_twenty_five_cent()
        {
            Money quarter = Money.Quarter;

            quarter.Amount.Should().Be(0.25m);
            quarter.QuarterCount.Should().Be(1);
        }


        [Fact]
        public void One_dollar_should_be_one_dollar()
        {
            Money dollar = Money.Dollar;

            dollar.Amount.Should().Be(1);
            dollar.OneDollarCount.Should().Be(1);
        }


        [Fact]
        public void Five_dollar_should_be_five_dollar()
        {
            Money fiveDollar = Money.FiveDollar;

            fiveDollar.Amount.Should().Be(5);
            fiveDollar.FiveDollarCount.Should().Be(1);
        }


        [Fact]
        public void Twenty_dollar_should_be_twenty_dollar()
        {
            Money twentyDollar = Money.TwentyDollar;

            twentyDollar.Amount.Should().Be(20);
            twentyDollar.TwentyDollarCount.Should().Be(1);
        }


        [Theory]
        [InlineData(-1, 0, 0, 0, 0, 0)]
        [InlineData(0, -2, 0, 0, 0, 0)]
        [InlineData(0, 0, -3, 0, 0, 0)]
        [InlineData(0, 0, 0, -4, 0, 0)]
        [InlineData(0, 0, 0, 0, -5, 0)]
        [InlineData(0, 0, 0, 0, 0, -6)]
        public void Cannot_create_money_with_negative_count(
            int oneCentCount,
            int tenCentCount,
            int quarterCount,
            int oneDollarCount,
            int fiveDollarCount,
            int twentyDollarCount)
        {
            Action action = () => new Money(
                oneCentCount,
                tenCentCount,
                quarterCount,
                oneDollarCount,
                fiveDollarCount,
                twentyDollarCount);

            action.ShouldBreakContract();
        }


        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0, 0)]
        [InlineData(1, 0, 0, 0, 0, 0, 0.01)]
        [InlineData(1, 2, 0, 0, 0, 0, 0.21)]
        [InlineData(1, 2, 3, 0, 0, 0, 0.96)]
        [InlineData(1, 2, 3, 4, 0, 0, 4.96)]
        [InlineData(1, 2, 3, 4, 5, 0, 29.96)]
        [InlineData(1, 2, 3, 4, 5, 6, 149.96)]
        [InlineData(11, 0, 0, 0, 0, 0, 0.11)]
        [InlineData(110, 0, 0, 0, 100, 0, 501.1)]
        public void Amount_should_be_calculated_correctly(
            int oneCentCount,
            int tenCentCount,
            int quarterCount,
            int oneDollarCount,
            int fiveDollarCount,
            int twentyDollarCount,
            double expectedAmount)
        {
            Money money = new Money(
                oneCentCount, 
                tenCentCount, 
                quarterCount, 
                oneDollarCount, 
                fiveDollarCount, 
                twentyDollarCount);

            money.Amount.ShouldBeEquivalentTo(expectedAmount);
        }


        [Fact]
        public void Two_cents_should_be_equal()
        {
            Money cent1 = new Money(1, 0, 0, 0, 0, 0);
            Money cent2 = new Money(1, 0, 0, 0, 0, 0);

            cent1.Should().Be(cent2);
            cent1.GetHashCode().Should().Be(cent2.GetHashCode());
        }


        [Fact]
        public void One_dollar_should_not_equal_100_cent()
        {
            Money dollar = Money.Dollar;
            Money hundredCents = new Money(100, 0, 0, 0, 0, 0);

            dollar.Should().NotBe(hundredCents);
            dollar.GetHashCode().Should().NotBe(hundredCents.GetHashCode());
        }


        [Fact]
        public void Sum_of_two_money_produces_correct_amount_of_money()
        {
            Money money1 = new Money(1, 2, 3, 4, 5, 6);
            Money money2 = new Money(1, 2, 3, 4, 5, 6);

            Money sum = money1 + money2;

            sum.OneCentCount.Should().Be(2);
            sum.TenCentCount.Should().Be(4);
            sum.QuarterCount.Should().Be(6);
            sum.OneDollarCount.Should().Be(8);
            sum.FiveDollarCount.Should().Be(10);
            sum.TwentyDollarCount.Should().Be(12);
        }


        [Fact]
        public void Substruction_of_two_sums_produces_correct_amount_of_money()
        {
            Money money1 = new Money(10, 10, 10, 10, 10, 10);
            Money money2 = new Money(1, 2, 3, 4, 5, 6);

            Money sum = money1 - money2;

            sum.OneCentCount.Should().Be(9);
            sum.TenCentCount.Should().Be(8);
            sum.QuarterCount.Should().Be(7);
            sum.OneDollarCount.Should().Be(6);
            sum.FiveDollarCount.Should().Be(5);
            sum.TwentyDollarCount.Should().Be(4);
        }


        [Fact]
        public void Cannot_subtract_more_than_exists()
        {
            Money money1 = new Money(0, 1, 0, 0, 0, 0);
            Money money2 = new Money(1, 0, 0, 0, 0, 0);

            Action action = () =>
            {
                Money money = money1 - money2;
            };

            action.ShouldBreakContract();
        }


        [Fact]
        public void Devote_should_return_sum_with_the_fewest_number_of_notes()
        {
            Money money = new Money(100, 10, 4, 1, 1, 1);

            Money calculated = money.Devote(6m);

            calculated.Amount.Should().Be(6m);
            calculated.FiveDollarCount.Should().Be(1);
            calculated.OneDollarCount.Should().Be(1);
        }


        [Fact]
        public void Devote_should_return_correct_sum_if_some_notes_dont_present()
        {
            Money money = new Money(100, 10, 4, 1, 0, 0);

            Money calculated = money.Devote(3.5m);

            calculated.Amount.Should().Be(3.5m);
            calculated.OneDollarCount.Should().Be(1);
            calculated.QuarterCount.Should().Be(4);
            calculated.TenCentCount.Should().Be(10);
            calculated.OneCentCount.Should().Be(50);
        }


        [Fact]
        public void Cannot_devote_more_than_have()
        {
            Money money = Money.Dollar;

            Action action = () => money.Devote(2m);

            action.ShouldBreakContract();
        }


        [Fact]
        public void Cannot_devote_if_not_enough_small_coins()
        {
            Money money = Money.Dollar;

            Action action = () => money.Devote(0.5m);

            action.ShouldBreakContract();
        }


        [Theory]
        [InlineData(1, 0, 0, 0, 0, 0, "¢1")]
        [InlineData(0, 0, 0, 1, 0, 0, "$1.00")]
        [InlineData(1, 0, 0, 1, 0, 0, "$1.01")]
        [InlineData(0, 0, 2, 1, 0, 0, "$1.50")]
        public void To_string_should_return_amount_of_money(
            int oneCentCount,
            int tenCentCount,
            int quarterCount,
            int oneDollarCount,
            int fiveDollarCount,
            int twentyDollarCount,
            string expectedString)
        {
            Money money = new Money(
                oneCentCount,
                tenCentCount,
                quarterCount,
                oneDollarCount,
                fiveDollarCount,
                twentyDollarCount);

            money.ToString().Should().Be(expectedString);
        }
    }
}
