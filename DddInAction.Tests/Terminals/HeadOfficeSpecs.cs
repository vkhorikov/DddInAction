using System;

using DddInAction.Logic.Atms;
using DddInAction.Logic.Common;
using DddInAction.Logic.HeadOffices;
using DddInAction.Logic.SnackMachines;

using FluentAssertions;

using Xunit;


namespace DddInAction.Tests.Terminals
{
    public class HeadOfficeSpecs
    {
        [Fact]
        public void Change_balance_should_change_current_balance()
        {
            HeadOffice headOffice = new HeadOffice();

            headOffice.ChangeBalance(5m);

            headOffice.Balance.Should().Be(5m);
        }


        [Fact]
        public void After_unloading_cash_all_money_go_from_snack_machine_to_head_office()
        {
            HeadOffice headOffice = new HeadOffice();
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.LoadMoney(Money.Dollar);

            headOffice.UnloadCashFromSnackMachine(snackMachine);

            headOffice.Cash.Amount.Should().Be(1m);
            snackMachine.MoneyInside.Amount.Should().Be(0m);
        }


        [Fact]
        public void Cannot_unload_cash_if_snack_machine_has_a_transaction()
        {
            HeadOffice headOffice = new HeadOffice();
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.InsertMoney(Money.Dollar);

            Result result = headOffice.UnloadCashFromSnackMachine(snackMachine);

            result.Failure.Should().Be(true);
        }


        [Fact]
        public void Added_cash_should_go_to_offices_cash()
        {
            HeadOffice headOffice = new HeadOffice();

            headOffice.AddCache(Money.Dollar);

            headOffice.Cash.Amount.Should().Be(1m);
        }


        [Fact]
        public void After_loading_cash_all_money_go_from_head_office_to_ATM()
        {
            HeadOffice headOffice = new HeadOffice();
            headOffice.AddCache(Money.TwentyDollar);
            Atm atm = new Atm();

            headOffice.LoadCashToAtm(atm);

            headOffice.Cash.Amount.Should().Be(0m);
            atm.MoneyInside.Amount.Should().Be(20m);
        }
    }
}
