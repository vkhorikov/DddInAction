using System;
using System.Collections.Generic;
using System.Windows;

using DddInAction.Client.Atms;
using DddInAction.Client.Common;
using DddInAction.Client.SnackMachines;
using DddInAction.Logic.Atms;
using DddInAction.Logic.Common;
using DddInAction.Logic.HeadOffices;
using DddInAction.Logic.SnackMachines;


namespace DddInAction.Client.HeadOffices
{
    public class DashboardViewModel : ViewModel
    {
        private readonly SnackMachineRepository _snackMachineRepository;
        private readonly AtmRepository _atmRepository;
        private readonly HeadOfficeRepository _headOfficeRepository;

        public HeadOffice HeadOffice { get; private set; }
        public IReadOnlyList<SnackMachineDto> SnackMachines { get; private set; }
        public IReadOnlyList<AtmDto> Atms { get; private set; }
        public Command<SnackMachineDto> ShowSnackMachineCommand { get; private set; }
        public Command<SnackMachineDto> UnloadCashCommand { get; private set; }
        public Command<AtmDto> ShowAtmCommand { get; private set; }
        public Command<AtmDto> LoadCashToAtmCommand { get; private set; }


        public DashboardViewModel()
        {
            HeadOffice = HeadOfficeInstance.Instance;
            _snackMachineRepository = new SnackMachineRepository();
            _atmRepository = new AtmRepository();
            _headOfficeRepository = new HeadOfficeRepository();

            RefreshAll();
            
            ShowSnackMachineCommand = new Command<SnackMachineDto>(x => x != null, ShowSnackMachine);
            UnloadCashCommand = new Command<SnackMachineDto>(CanUnloadCash, UnloadCash);
            ShowAtmCommand = new Command<AtmDto>(x => x != null, ShowAtm);
            LoadCashToAtmCommand = new Command<AtmDto>(CanLoadCashToAtm, LoadCashToAtm);
        }


        private bool CanLoadCashToAtm(AtmDto atmDto)
        {
            return atmDto != null && HeadOffice.Cash.Amount > 0;
        }


        private void LoadCashToAtm(AtmDto atmDto)
        {
            Maybe<Atm> atm = _atmRepository.GetById(atmDto.Id);

            if (atm.HasNoValue)
                return;

            HeadOffice.LoadCashToAtm(atm.Value);
            _atmRepository.Save(atm.Value);
            _headOfficeRepository.Save(HeadOffice);

            RefreshAll();
        }


        private void ShowAtm(AtmDto atmDto)
        {
            Maybe<Atm> atm = _atmRepository.GetById(atmDto.Id);

            if (atm.HasNoValue)
                return;

            _dialogService.ShowDialog(new AtmViewModel(atm.Value));
            RefreshAll();
        }


        private bool CanUnloadCash(SnackMachineDto snackMachineDto)
        {
            return snackMachineDto != null && snackMachineDto.MoneyInside > 0;
        }


        private void UnloadCash(SnackMachineDto snackMachineDto)
        {
            Maybe<SnackMachine> snackMachine = _snackMachineRepository.GetById(snackMachineDto.Id);

            if (snackMachine.HasNoValue)
                return;

            HeadOffice.UnloadCashFromSnackMachine(snackMachine.Value);
            _snackMachineRepository.Save(snackMachine.Value);
            _headOfficeRepository.Save(HeadOffice);

            RefreshAll();
        }


        private void ShowSnackMachine(SnackMachineDto snackMachineDto)
        {
            Maybe<SnackMachine> snackMachine = _snackMachineRepository.GetById(snackMachineDto.Id);

            if (snackMachine.HasNoValue)
            {
                MessageBox.Show("Snack machine was not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _dialogService.ShowDialog(new SnackMachineViewModel(snackMachine.Value));
            RefreshAll();
        }


        private void RefreshAll()
        {
            SnackMachines = _snackMachineRepository.GetSnackMachineList();
            Atms = _atmRepository.GetAtmList();

            Notify(() => Atms);
            Notify(() => SnackMachines);
            Notify(() => HeadOffice);
        }
    }
}
