using CombatAnalysis.Core.Localizations;
using System;
using System.Resources;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace CombatAnalysis.App.Localizations;

public class LocExtension : MarkupExtension
{
    public LocExtension(string stringName)
    {
        ResourceName = stringName;
    }

    public string ResourceName { get; }

    private static ResourceManager? GetResourceManager(object control)
    {
        if (control is not DependencyObject dependencyObject)
        {
            return null;
        }

        var localValue = dependencyObject.ReadLocalValue(LocalizationObject.ResourceManagerProperty);

        if (localValue != DependencyProperty.UnsetValue && localValue is ResourceManager resourceManager)
        {
            TranslationSource.Instance.AddResourceManager(resourceManager);

            return resourceManager;
        }
        else
        {
            return null;
        }
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var targetObject = (serviceProvider as IProvideValueTarget)?.TargetObject;
        if (targetObject?.GetType().Name == "SharedDp")
        {
            return targetObject;
        }

        var baseName = GetBaseName(targetObject, serviceProvider);

        var binding = new Binding
        {
            Mode = BindingMode.OneWay,
            Path = new PropertyPath($"[{baseName}.{ResourceName}]"),
            Source = TranslationSource.Instance,
            FallbackValue = ResourceName,
        };

        var expression = binding.ProvideValue(serviceProvider);

        return expression;
    }

    private static string GetBaseName(object targetObject, IServiceProvider serviceProvider)
    {
        var baseName = GetResourceManager(targetObject)?.BaseName ?? string.Empty;
        if (string.IsNullOrEmpty(baseName))
        {
            var rootObjectProvider = (IRootObjectProvider)serviceProvider;
            var rootObject = rootObjectProvider?.RootObject;
            baseName = GetResourceManager(rootObject)?.BaseName ?? string.Empty;
        }

        return baseName;
    }
}
