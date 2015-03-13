using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using DddInAction.Client.Utils;
using DddInAction.Logic.Common;


namespace DddInAction.Client.Common
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        protected static readonly DialogService _dialogService = new DialogService();

        public event PropertyChangedEventHandler PropertyChanged;

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get
            {
                return _dialogResult;
            }
            protected set
            {
                _dialogResult = value;
                Notify();
            }
        }


        protected void Notify([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        protected void Notify<T>(Expression<Func<T>> propertyExpression)
        {
            MemberExpression expression = propertyExpression.Body as MemberExpression;

            if (expression == null)
                throw new ArgumentException(propertyExpression.Body.ToString());

            Notify(expression.Member.Name);
        }
    }
}
