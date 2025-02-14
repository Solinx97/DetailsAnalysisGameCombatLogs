﻿using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.Core;

internal static class Sorts<T>
    where T : class
{
    private static string _propertyName = string.Empty;

    public static void SetProperty(string propertyName)
    {
        _propertyName = propertyName;
    }

    public static ObservableCollection<T> BubbleSort(ObservableCollection<T> oldCollection)
    {
        var newCollection = new ObservableCollection<T>(oldCollection);
        for (int i = 0; i < newCollection.Count; i++)
        {
            for (int j = 0; j < newCollection.Count - 1; j++)
            {
                if (GetPropertyValue<TimeSpan>(newCollection[j]) > GetPropertyValue<TimeSpan>(newCollection[j + 1]))
                {
                    newCollection.Move(j, j + 1);
                }
            }
        }

        return newCollection;
    }

    private static TProperty GetPropertyValue<TProperty>(object item)
        where TProperty : notnull
    {
        var type = typeof(T);
        var property = type.GetProperty(_propertyName);

        if (property == null)
        {
            throw new InvalidOperationException($"Property '{_propertyName}' not found on type '{type.FullName}'.");
        }

        var value = property.GetValue(item);
        if (value == null)
        {
            throw new InvalidOperationException($"Property '{_propertyName}' on type '{type.FullName}' returned null.");
        }

        return (TProperty)value;
    }
}
