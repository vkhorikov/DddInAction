using System;
using System.Windows;
using System.Windows.Media;

using DddInAction.Logic.SnackMachines;


namespace DddInAction.Client.SnackMachines
{
    public class SnackPileViewModel
    {
        private readonly SnackPile _snackPile;

        public string Price
        {
            get { return _snackPile.Price.ToString("C2"); }
        }

        public int Amount
        {
            get { return _snackPile.Amount; }
        }

        public ImageSource Image
        {
            get { return (ImageSource)Application.Current.FindResource("img" + _snackPile.Snack.Name); }
        }

        public int ImageWidth
        {
            get { return GetImageWidth(_snackPile.Snack); }
        }


        private int GetImageWidth(Snack snack)
        {
            if (snack == Snack.Chocolate)
                return 120;

            if (snack == Snack.Soda)
                return 70;

            if (snack == Snack.Gum)
                return 70;

            throw new ArgumentException();
        }


        public SnackPileViewModel(SnackPile snackPile)
        {
            _snackPile = snackPile;
        }
    }
}
