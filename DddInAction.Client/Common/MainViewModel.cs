using System;

using DddInAction.Client.HeadOffices;


namespace DddInAction.Client.Common
{
    public class MainViewModel : ViewModel
    {
        public DashboardViewModel Dashboard { get; private set; }


        public MainViewModel()
        {
            Dashboard = new DashboardViewModel();
        }
    }
}
