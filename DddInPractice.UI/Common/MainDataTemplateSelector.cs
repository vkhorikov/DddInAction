using System;
using System.Windows;
using System.Windows.Controls;

namespace DddInPractice.UI.Common
{
    public class MainDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null || Application.Current == null)
            {
                return null;
            }

            string name = item.GetType().Name;
            DataTemplate template = (DataTemplate)Application.Current.TryFindResource(name);

            if (template == null)
            {
                throw new ArgumentException("Template for ViewModel " + name + " was not found");
            }

            return template;
        }
    }
}
