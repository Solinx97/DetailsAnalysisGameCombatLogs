using CombatAnalysis.DAL.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

namespace CombatAnalysis.DAL.Helpers;

internal static class MigrationHelper
{
    private static readonly Type[] _types =
    {
            typeof(CombatLog),
            typeof(CombatPlayer),
            typeof(CombatAura),
            typeof(CombatPlayerPosition),
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
            typeof(PlayerDeath),
    };

    private static readonly Type[] _paginationTypes =
    {
            typeof(DamageDone),
            typeof(HealDone),
            typeof(DamageTaken),
            typeof(ResourceRecovery),
    };

    private static readonly Type[] _typesByCombatPlayer =
    {
            typeof(DamageDone),
            typeof(DamageDoneGeneral),
            typeof(HealDone),
            typeof(HealDoneGeneral),
            typeof(DamageTaken),
            typeof(DamageTakenGeneral),
            typeof(ResourceRecovery),
            typeof(ResourceRecoveryGeneral),
            typeof(PlayerDeath),
    };

    public static void CreateProcedures(MigrationBuilder migrationBuilder)
    {
        foreach (var item in _types)
        {
            migrationBuilder.Sql($"CREATE PROCEDURE GetAll{item.Name}\n" +
                         "AS\n" +
                         "BEGIN\n" +
                         "\tSELECT * \n" +
                        $"\tFROM {item.Name}\n" +
                        "END");

            var property = item.GetProperty("Id");
            migrationBuilder.Sql($"CREATE PROCEDURE Get{item.Name}ById (@id {Converter(property.PropertyType.Name)})\n" +
                     "AS\n" +
                     "BEGIN\n" +
                     "\tSELECT * \n" +
                    $"\tFROM {item.Name}\n" +
                     "\tWHERE Id = @id\n" +
                     "END");

            var insertIntoParams = InsertIntoParams(item);
            var insertIntoOutputParams = InsertIntoOutputParams(item);
            migrationBuilder.Sql($"CREATE PROCEDURE InsertInto{item.Name} ({insertIntoParams.Item1})\n" +
                    $"AS\n" +
                    "BEGIN\n" +
                    $"\tDECLARE @OutputTbl TABLE ({insertIntoOutputParams})\n" +
                    $"\tINSERT INTO {item.Name}\n" +
                    $"\tOUTPUT INSERTED.* INTO @OutputTbl\n" +
                    $"\tVALUES ({insertIntoParams.Item2})\n" +
                     "\tSELECT * FROM @OutputTbl\n" +
                     "END");

            insertIntoParams = UpdateParamsAndValues(item);
            migrationBuilder.Sql($"CREATE PROCEDURE Update{item.Name} ({insertIntoParams.Item1})\n" +
                    $"AS\n" +
                    "BEGIN\n" +
                    $"\tUPDATE {item.Name}\n" +
                    $"\tSET {insertIntoParams.Item2}\n" +
                     "\tWHERE Id = @Id\n" +
                     "END");

            migrationBuilder.Sql($"CREATE PROCEDURE Delete{item.Name}ById (@id {Converter(property.PropertyType.Name)})\n" +
                    $"AS\n" +
                    "BEGIN\n" +
                    $"\tDELETE FROM {item.Name}\n" +
                     "\tWHERE Id = @id\n" +
                     "END");
        }

        CreateProceduresWithPaginations(migrationBuilder);

        GetDataByCombatPlayerId(migrationBuilder);
        GetSpecializationScore(migrationBuilder);
    }

    public static void DropProcedures(MigrationBuilder migrationBuilder)
    {
        foreach (var item in _types)
        {
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS GetAll{item.Name}");
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Get{item.Name}ById");
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS InsertInto{item.Name}");
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Update{item.Name}");
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Delete{item.Name}ById");
        }

        foreach (var item in _paginationTypes)
        {
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Get{item.Name}ByCombatPlayerIdPagination");
        }

        foreach (var item in _typesByCombatPlayer)
        {
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Get{item.Name}ByCombatPlayerId");
        }

        migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS Get{nameof(SpecializationScore)}BySpecId");
    }

    private static void CreateProceduresWithPaginations(MigrationBuilder migrationBuilder)
    {
        foreach (var item in _paginationTypes)
        {
            var property = item.GetProperty("CombatPlayerId");
            migrationBuilder.Sql($"CREATE PROCEDURE Get{item.Name}ByCombatPlayerIdPagination (@combatPlayerId {Converter(property.PropertyType.Name)}, @page INT, @pageSize INT)\n" +
                          "AS\n" +
                          "BEGIN\n" +
                          "\tSELECT * \n" +
                         $"\tFROM {item.Name}\n" +
                          "\tWHERE CombatPlayerId = @combatPlayerId\n" +
                         $"\tORDER BY Id\n" +
                          "\tOFFSET (@page - 1) * @pageSize ROWS\n" +
                          "\tFETCH NEXT @pageSize ROWS ONLY\n" +
                          "END");
        }
    }

    private static void GetDataByCombatPlayerId(MigrationBuilder migrationBuilder)
    {
        foreach (var item in _typesByCombatPlayer)
        {
            var property = item.GetProperty("CombatPlayerId");
            migrationBuilder.Sql($"CREATE PROCEDURE Get{item.Name}ByCombatPlayerId (@combatPlayerId {Converter(property.PropertyType.Name)})\n" +
                          "AS\n" +
                          "BEGIN\n" +
                          "\tSELECT * \n" +
                         $"\tFROM {item.Name}\n" +
                          "\tWHERE CombatPlayerId = @combatPlayerId\n" +
                          "END");
        }
    }

    private static void GetSpecializationScore(MigrationBuilder migrationBuilder)
    {
        var classType = typeof(SpecializationScore);

        var propertySpecId = classType.GetProperty(nameof(SpecializationScore.SpecId));
        var propertyBossId = classType.GetProperty(nameof(SpecializationScore.BossId));
        var propertyDifficult = classType.GetProperty(nameof(SpecializationScore.Difficult));
        migrationBuilder.Sql($"CREATE PROCEDURE Get{classType.Name}BySpecId (@specId {Converter(propertySpecId.PropertyType.Name)}, " +
                                                                  $"@bossId {Converter(propertyBossId.PropertyType.Name)}, " +
                                                                  $"@difficult {Converter(propertyDifficult.PropertyType.Name)})\n" +
                      "AS\n" +
                      "BEGIN\n" +
                      "\tSELECT * \n" +
                     $"\tFROM {classType.Name}\n" +
                      "\tWHERE SpecId = @specId AND BossId = @bossId AND Difficult = @difficult\n" +
                      "END");
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

        for (int i = 1; i < properties.Length; i++)
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
