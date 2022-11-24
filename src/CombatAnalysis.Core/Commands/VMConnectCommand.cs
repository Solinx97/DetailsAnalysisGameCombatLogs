using CombatAnalysis.Core.Interfaces;

namespace CombatAnalysis.Core.Commands;

public class ViewModelMConnect : IViewModelConnect
{
    public object Data { get; set; }

    public void PropertyUpdate<T>(object context, string propertyName, object value)
        where T : class
    {
        var targetType = typeof(T);
        foreach (var item in targetType.GetProperties())
        {
            if (item.Name == propertyName)
            {
                item.SetValue(context, value);
                break;
            }
        }
    }
}

public class ViewModelMConnect<T> : IViewModelConnect<T>
    where T : notnull
{
    public T Data { get; set; }

    public void PropertyUpdate<T>(object context, string propertyName, object value)
        where T : class
    {
        var targetType = typeof(T);
        foreach (var item in targetType.GetProperties())
        {
            if (item.Name == propertyName)
            {
                item.SetValue(context, value);
                break;
            }
        }
    }
}
