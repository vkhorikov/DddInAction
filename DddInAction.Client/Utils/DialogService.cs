using System;

using DddInAction.Client.Common;


namespace DddInAction.Client.Utils
{
    public class DialogService
    {
        public bool? ShowDialog(ViewModel viewModel)
        {
            CustomWindow window = new CustomWindow(viewModel);
            return window.ShowDialog();
        }
    }
}
