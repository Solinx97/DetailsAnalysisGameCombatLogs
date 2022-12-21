using System;
using System.Resources;
using System.Windows;

namespace CombatAnalysis.App.Localizations;

public class LocalizationObject : DependencyObject
{
    public static readonly DependencyProperty ResourceManagerProperty =
         DependencyProperty.RegisterAttached("ResourceManager", typeof(ResourceManager), typeof(LocalizationObject));

    public static ResourceManager GetResourceManager(DependencyObject dependencyObject)
    {
        if (dependencyObject == null)
        {
            throw new ArgumentNullException(nameof(dependencyObject));
        }

        var value = (ResourceManager)dependencyObject.GetValue(ResourceManagerProperty);
        return value;
    }

    public static void SetResourceManager(DependencyObject dependencyObject, ResourceManager value)
    {
        dependencyObject?.SetValue(ResourceManagerProperty, value);
    }
}
