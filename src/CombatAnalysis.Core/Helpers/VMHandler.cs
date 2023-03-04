using CombatAnalysis.Core.Interfaces;

namespace CombatAnalysis.Core.Helpers;

public class VMHandler : IVMHandler
{
    public void PropertyUpdate<T1>(object context, string propertyName, object value)
        where T1 : class
    {
        var targetType = typeof(T1);
        foreach (var item in targetType.GetProperties())
        {
            if (string.Equals(item.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                item.SetValue(context, value);
                break;
            }
        }
    }
}
