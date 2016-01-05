using System.ComponentModel;
using System.Runtime.CompilerServices;
using DddInPractice.UI.Utils;

namespace DddInPractice.UI.Common
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        protected static readonly DialogService _dialogService = new DialogService();
        public event PropertyChangedEventHandler PropertyChanged;

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            protected set
            {
                _dialogResult = value;
                Notify();
            }
        }

        public virtual string Caption => string.Empty;

        protected void Notify([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
