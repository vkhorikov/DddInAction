using System.Windows;

namespace DddInPractice.UI.Common
{
    public static class DialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached(
            "DialogResult",
            typeof(bool?),
            typeof(DialogCloser),
            new PropertyMetadata(DialogResultChanged));


        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }


        private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
            {
                window.DialogResult = e.NewValue as bool?;
            }
        }
    }
}
