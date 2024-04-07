using CombatAnalysis.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CombatAnalysis.DAL.Helpers;

public static class DbProcedureHelper
{
    public static void CreateProcedures(DbContext dbContext)
    {
        var types = new Type[]
        {
            typeof(CombatLog),
            typeof(CombatLogByUser),
            typeof(CombatPlayer),
            typeof(PlayerParseInfo),
            typeof(SpecializationScore),
            typeof(Combat),
            typeof(DamageDone),
            typeof(DamageDoneGeneral),
            typeof(HealDone),
            typeof(HealDoneGeneral),
            typeof(DamageTaken),
            typeof(DamageTakenGeneral),
            typeof(ResourceRecovery),
            typeof(ResourceRecoveryGeneral),
        };

        foreach (var item in types)
        {
            var query = $"CREATE PROCEDURE GetAll{item.Name}\n" +
                          "\tAS SELECT * \n" +
                          $"\tFROM {item.Name}";
            dbContext.Database.ExecuteSqlRaw(query);

            var property = item.GetProperty("Id");
            query = $"CREATE PROCEDURE Get{item.Name}ById (@id {Converter(property.PropertyType.Name)})\n" +
                          "\tAS SELECT * \n" +
                          $"\tFROM {item.Name}\n" +
                          "\tWHERE Id = @id";
            dbContext.Database.ExecuteSqlRaw(query);

            var insertIntoParams = InsertIntoParams(item);
            var insertIntoOutputParams = InsertIntoOutputParams(item);
            query = $"CREATE PROCEDURE InsertInto{item.Name} ({insertIntoParams.Item1})\n" +
                          $"\tAS\n" +
                          $"\tDECLARE @OutputTbl TABLE ({insertIntoOutputParams})\n" +
                          $"\tINSERT INTO {item.Name}\n" +
                          $"\tOUTPUT INSERTED.* INTO @OutputTbl\n" +
                          $"\tVALUES ({insertIntoParams.Item2})\n" +
                          "\tSELECT * FROM @OutputTbl";
            dbContext.Database.ExecuteSqlRaw(query);

            insertIntoParams = UpdateParamsAndValues(item);
            query = $"CREATE PROCEDURE Update{item.Name} ({insertIntoParams.Item1})\n" +
                          $"\tAS UPDATE {item.Name}\n" +
                          $"\tSET {insertIntoParams.Item2}\n" +
                          "\tWHERE Id = @Id";
            dbContext.Database.ExecuteSqlRaw(query);

            query = $"CREATE PROCEDURE Delete{item.Name}ById (@id {Converter(property.PropertyType.Name)})\n" +
                          $"\tAS DELETE FROM {item.Name}\n" +
                          "\tWHERE Id = @id";
            dbContext.Database.ExecuteSqlRaw(query);
        }

        GetDataByCombatPlayerId(dbContext);
        GetSpecializationScore(dbContext);
    }

    private static void GetDataByCombatPlayerId(DbContext dbContext)
    {
        var types = new Type[]
        {
            typeof(DamageDone),
            typeof(DamageDoneGeneral),
            typeof(HealDone),
            typeof(HealDoneGeneral),
            typeof(DamageTaken),
            typeof(DamageTakenGeneral),
            typeof(ResourceRecovery),
            typeof(ResourceRecoveryGeneral),
        };

        foreach (var item in types)
        {
            var property = item.GetProperty("CombatPlayerId");
            var query = $"CREATE PROCEDURE Get{item.Name}ByCombatPlayerId (@combatPlayerId {Converter(property.PropertyType.Name)})\n" +
                          "\tAS SELECT * \n" +
                          $"\tFROM {item.Name}\n" +
                          "\tWHERE CombatPlayerId = @combatPlayerId";
            dbContext.Database.ExecuteSqlRaw(query);
        }
    }

    private static void GetSpecializationScore(DbContext dbContext)
    {
        var classType = typeof(SpecializationScore);

        var propertySpecId = classType.GetProperty(nameof(SpecializationScore.SpecId));
        var propertyBossId = classType.GetProperty(nameof(SpecializationScore.BossId));
        var propertyDifficult= classType.GetProperty(nameof(SpecializationScore.Difficult));
        var query = $"CREATE PROCEDURE Get{classType.Name}BySpecId (@specId {Converter(propertySpecId.PropertyType.Name)}, " +
                                                                  $"@bossId {Converter(propertyBossId.PropertyType.Name)}, " +
                                                                  $"@difficult {Converter(propertyDifficult.PropertyType.Name)})\n" +
                      "\tAS SELECT * \n" +
                      $"\tFROM {classType.Name}\n" +
                      "\tWHERE SpecId = @specId AND BossId = @bossId AND Difficult = @difficult";
        dbContext.Database.ExecuteSqlRaw(query);
    }

    private static Tuple<string, string> InsertIntoParams(Type type)
    {
        var properties = type.GetProperties();
        var procedureParamNames = new StringBuilder();
        var procedureParamNamesWithPropertyTypes = new StringBuilder();
        if (type.GetProperty("Id")?.PropertyType != typeof(int))
        {
            var propertyTypeName = properties[0].PropertyType.Name;
            procedureParamNamesWithPropertyTypes.Append($"@{properties[0].Name} {Converter(propertyTypeName)},");
            procedureParamNames.Append($"@{properties[0].Name},");
        }

        for (var i = 1; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                var propertTypeName = properties[i].PropertyType.Name;
                procedureParamNamesWithPropertyTypes.Append($"@{properties[i].Name} {Converter(propertTypeName)},");
                procedureParamNames.Append($"@{properties[i].Name},");
            }
        }

        procedureParamNamesWithPropertyTypes.Remove(procedureParamNamesWithPropertyTypes.Length - 1, 1);
        procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

        return new Tuple<string, string>(procedureParamNamesWithPropertyTypes.ToString(), procedureParamNames.ToString());
    }

    private static string InsertIntoOutputParams(Type type)
    {
        var properties = type.GetProperties();
        var procedureParamNamesWithPropertyTypes = new StringBuilder();
        for (int i = 0; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                var propertTypeName = properties[i].PropertyType.Name;
                procedureParamNamesWithPropertyTypes.Append($"{properties[i].Name} {Converter(propertTypeName)},");
            }
        }

        procedureParamNamesWithPropertyTypes.Remove(procedureParamNamesWithPropertyTypes.Length - 1, 1);

        return procedureParamNamesWithPropertyTypes.ToString();
    }

    private static Tuple<string, string> UpdateParamsAndValues(Type type)
    {
        var properties = type.GetProperties();
        var procedureParamNames = new StringBuilder();
        var procedureParamNamesWithPropertyTypes = new StringBuilder();
        for (int i = 0; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                var propertTypeName = properties[i].PropertyType.Name;
                procedureParamNamesWithPropertyTypes.Append($"@{properties[i].Name} {Converter(propertTypeName)},");
            }
        }

        for (int i = 1; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                procedureParamNames.Append($"{properties[i].Name} = @{properties[i].Name},");
            }
        }

        procedureParamNamesWithPropertyTypes.Remove(procedureParamNamesWithPropertyTypes.Length - 1, 1);
        procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

        return new Tuple<string, string>(procedureParamNamesWithPropertyTypes.ToString(), procedureParamNames.ToString());
    }

    private static string Converter(string type)
    {
        return type switch
        {
            "String" => "NVARCHAR (MAX)",
            "Int32" => "INT",
            "Int16" => "INT",
            "Boolean" => "BIT",
            "DateTimeOffset" => "DATETIMEOFFSET (7)",
            "Double" => "FLOAT (53)",
            "TimeSpan" => "TIME (7)",
            _ => "NVARCHAR (MAX)",
        };
    }
}
