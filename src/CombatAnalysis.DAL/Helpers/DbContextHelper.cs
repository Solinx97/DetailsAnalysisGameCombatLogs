using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Helpers
{
    public static class DbProcedureHelper
    {
        public static async Task CreateProceduresAsync(DbContext dbContext)
        {
            var types = new Type[]
            {
                typeof(AppUser),
                typeof(PersonalChat),
                typeof(PersonalChatMessage),
                typeof(InviteToGroupChat),
                typeof(GroupChat),
                typeof(GroupChatMessage),
                typeof(GroupChatUser),
                typeof(BannedUser),
                typeof(CombatLog),
                typeof(CombatLogByUser),
                typeof(CombatPlayer),
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

            try
            {
                foreach (var item in types)
                {
                    var query = $"CREATE PROCEDURE GetAll{item.Name}\n" +
                                  "\tAS SELECT * \n" +
                                  $"\tFROM {item.Name}";
                    await dbContext.Database.ExecuteSqlRawAsync(query);

                    var property = item.GetProperty("Id");
                    query = $"CREATE PROCEDURE Get{item.Name}ById (@id {Converter(property.PropertyType.Name)})\n" +
                                  "\tAS SELECT * \n" +
                                  $"\tFROM {item.Name}\n" +
                                  "\tWHERE Id = @id";
                    await dbContext.Database.ExecuteSqlRawAsync(query);

                    var data = InsertIntoParamsAndValues(item);
                    var data1 = InsertIntoParamsAndValues1(item);
                    query = $"CREATE PROCEDURE InsertInto{item.Name} ({data.Item1})\n" +
                                  $"\tAS\n" +
                                  $"\tDECLARE @OutputTbl TABLE ({data1})\n" +
                                  $"\tINSERT INTO {item.Name}\n" +
                                  $"\tOUTPUT INSERTED.* INTO @OutputTbl\n" +
                                  $"\tVALUES ({data.Item2})\n" +
                                  "\tSELECT * FROM @OutputTbl";
                    await dbContext.Database.ExecuteSqlRawAsync(query);

                    data = UpdateParamsAndValues(item);
                    query = $"CREATE PROCEDURE Update{item.Name} ({data.Item1})\n" +
                                  $"\tAS UPDATE {item.Name}\n" +
                                  $"\tSET {data.Item2}\n" +
                                  "\tWHERE Id = @Id";
                    await dbContext.Database.ExecuteSqlRawAsync(query);

                    query = $"CREATE PROCEDURE Delete{item.Name}ById (@id {Converter(property.PropertyType.Name)})\n" +
                                  $"\tAS DELETE FROM {item.Name}\n" +
                                  "\tWHERE Id = @id";
                    await dbContext.Database.ExecuteSqlRawAsync(query);
                }
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message;
            }
        }

        private static Tuple<string, string> InsertIntoParamsAndValues(Type type)
        {
            var properties = type.GetProperties();
            var procedureParamNames = new StringBuilder();
            var procedureParamNamesWithPropertyTypes = new StringBuilder();
            if (type.GetProperty("Id").PropertyType != typeof(int))
            {
                var propertTypeName = properties[0].PropertyType.Name;
                procedureParamNamesWithPropertyTypes.Append($"@{properties[0].Name} {Converter(propertTypeName)},");
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

        private static string InsertIntoParamsAndValues1(Type type)
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
            switch (type)
            {
                case "String":
                    return "NVARCHAR (MAX)";
                case "Int32":
                    return "INT";
                case "Int16":
                    return "INT";
                case "Boolean":
                    return "BIT";
                case "DateTimeOffset":
                    return "DATETIMEOFFSET (7)";
                case "Double":
                    return "FLOAT (53)";
                case "TimeSpan":
                    return "TIME (7)";
                default:
                    return "NVARCHAR (MAX)";
            }
        }
    }
}
