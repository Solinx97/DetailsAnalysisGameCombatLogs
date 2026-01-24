using System.Data;

namespace CombatParser.Infrastructure.Extensions;

internal static class DataTableExtension
{
    public static void AddColumn<T>(this DataTable table, string name, bool notNull = true)
    {
        var col = new DataColumn(name, typeof(T))
        {
            AllowDBNull = !notNull
        };
        table.Columns.Add(col);
    }
}
