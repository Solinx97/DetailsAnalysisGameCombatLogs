using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CombatParser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBosses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Boss",
                columns: new[] { "Id", "Difficult", "GameId", "Health", "Name", "Size" },
                values: new object[,]
                {
                    { 57, 3, 1602, 61900000L, "Глубиний", 10 },
                    { 58, 5, 1602, 91500000L, "Глубиний", 10 },
                    { 59, 3, 1598, 114000000L, "Павшие защитники", 10 },
                    { 60, 5, 1598, 250000000L, "Павшие защитники", 10 },
                    { 61, 3, 1624, 401000000L, "Норусхен", 10 },
                    { 62, 5, 1624, 702000000L, "Норусхен", 10 },
                    { 63, 3, 1604, 426000000L, "Ша Гордыни", 10 },
                    { 64, 5, 1604, 661000000L, "Ша Гордыни", 10 },
                    { 65, 3, 1622, 139000000L, "Галакрас", 10 },
                    { 66, 5, 1622, 218000000L, "Галакрас", 10 },
                    { 67, 3, 1600, 451000000L, "Железный исполин", 10 },
                    { 68, 5, 1600, 592000000L, "Железный исполин", 10 },
                    { 69, 3, 1606, 349000000L, "Кор'кронские темные шаманы", 10 },
                    { 70, 5, 1606, 654000000L, "Кор'кронские темные шаманы", 10 },
                    { 71, 3, 1603, 349000000L, "Генерал Назгрим", 10 },
                    { 72, 5, 1603, 523000000L, "Генерал Назгрим", 10 },
                    { 73, 3, 1595, 377000000L, "Малкорок", 10 },
                    { 74, 5, 1595, 630000000L, "Малкорок", 10 },
                    { 75, 3, 1594, 621000000L, "Пандарийские трофеи", 10 },
                    { 76, 5, 1594, 1190000000L, "Пандарийские трофеи", 10 },
                    { 77, 3, 1599, 445000000L, "Ток Кровожадный", 10 },
                    { 78, 5, 1599, 654000000L, "Ток Кровожадный", 10 },
                    { 79, 3, 1601, 298000000L, "Мастер осады Черноплавс", 10 },
                    { 80, 5, 1601, 500000000L, "Мастер осады Черноплавс", 10 },
                    { 81, 3, 1593, 510000000L, "Идеалы клакси", 10 },
                    { 82, 5, 1593, 1260000000L, "Идеалы клакси", 10 },
                    { 83, 3, 1623, 161000000L, "Гаррош Адский Крик", 10 },
                    { 84, 5, 1623, 228000000L, "Гаррош Адский Крик", 10 }
                });

            migrationBuilder.InsertData(
                table: "BestSpecializationScore",
                columns: new[] { "Id", "BossId", "DamageDone", "HealDone", "SpecializationId", "Updated" },
                values: new object[,]
                {
                    { 953, 57, 0, 0, 1, null },
                    { 954, 57, 0, 0, 2, null },
                    { 955, 57, 0, 0, 3, null },
                    { 956, 57, 0, 0, 4, null },
                    { 957, 57, 0, 0, 5, null },
                    { 958, 57, 0, 0, 6, null },
                    { 959, 57, 0, 0, 7, null },
                    { 960, 57, 0, 0, 8, null },
                    { 961, 57, 0, 0, 9, null },
                    { 962, 57, 0, 0, 10, null },
                    { 963, 57, 0, 0, 11, null },
                    { 964, 57, 0, 0, 12, null },
                    { 965, 57, 0, 0, 13, null },
                    { 966, 57, 0, 0, 14, null },
                    { 967, 57, 0, 0, 15, null },
                    { 968, 57, 0, 0, 16, null },
                    { 969, 57, 0, 0, 17, null },
                    { 970, 58, 0, 0, 1, null },
                    { 971, 58, 0, 0, 2, null },
                    { 972, 58, 0, 0, 3, null },
                    { 973, 58, 0, 0, 4, null },
                    { 974, 58, 0, 0, 5, null },
                    { 975, 58, 0, 0, 6, null },
                    { 976, 58, 0, 0, 7, null },
                    { 977, 58, 0, 0, 8, null },
                    { 978, 58, 0, 0, 9, null },
                    { 979, 58, 0, 0, 10, null },
                    { 980, 58, 0, 0, 11, null },
                    { 981, 58, 0, 0, 12, null },
                    { 982, 58, 0, 0, 13, null },
                    { 983, 58, 0, 0, 14, null },
                    { 984, 58, 0, 0, 15, null },
                    { 985, 58, 0, 0, 16, null },
                    { 986, 58, 0, 0, 17, null },
                    { 987, 59, 0, 0, 1, null },
                    { 988, 59, 0, 0, 2, null },
                    { 989, 59, 0, 0, 3, null },
                    { 990, 59, 0, 0, 4, null },
                    { 991, 59, 0, 0, 5, null },
                    { 992, 59, 0, 0, 6, null },
                    { 993, 59, 0, 0, 7, null },
                    { 994, 59, 0, 0, 8, null },
                    { 995, 59, 0, 0, 9, null },
                    { 996, 59, 0, 0, 10, null },
                    { 997, 59, 0, 0, 11, null },
                    { 998, 59, 0, 0, 12, null },
                    { 999, 59, 0, 0, 13, null },
                    { 1000, 59, 0, 0, 14, null },
                    { 1001, 59, 0, 0, 15, null },
                    { 1002, 59, 0, 0, 16, null },
                    { 1003, 59, 0, 0, 17, null },
                    { 1004, 60, 0, 0, 1, null },
                    { 1005, 60, 0, 0, 2, null },
                    { 1006, 60, 0, 0, 3, null },
                    { 1007, 60, 0, 0, 4, null },
                    { 1008, 60, 0, 0, 5, null },
                    { 1009, 60, 0, 0, 6, null },
                    { 1010, 60, 0, 0, 7, null },
                    { 1011, 60, 0, 0, 8, null },
                    { 1012, 60, 0, 0, 9, null },
                    { 1013, 60, 0, 0, 10, null },
                    { 1014, 60, 0, 0, 11, null },
                    { 1015, 60, 0, 0, 12, null },
                    { 1016, 60, 0, 0, 13, null },
                    { 1017, 60, 0, 0, 14, null },
                    { 1018, 60, 0, 0, 15, null },
                    { 1019, 60, 0, 0, 16, null },
                    { 1020, 60, 0, 0, 17, null },
                    { 1021, 61, 0, 0, 1, null },
                    { 1022, 61, 0, 0, 2, null },
                    { 1023, 61, 0, 0, 3, null },
                    { 1024, 61, 0, 0, 4, null },
                    { 1025, 61, 0, 0, 5, null },
                    { 1026, 61, 0, 0, 6, null },
                    { 1027, 61, 0, 0, 7, null },
                    { 1028, 61, 0, 0, 8, null },
                    { 1029, 61, 0, 0, 9, null },
                    { 1030, 61, 0, 0, 10, null },
                    { 1031, 61, 0, 0, 11, null },
                    { 1032, 61, 0, 0, 12, null },
                    { 1033, 61, 0, 0, 13, null },
                    { 1034, 61, 0, 0, 14, null },
                    { 1035, 61, 0, 0, 15, null },
                    { 1036, 61, 0, 0, 16, null },
                    { 1037, 61, 0, 0, 17, null },
                    { 1038, 62, 0, 0, 1, null },
                    { 1039, 62, 0, 0, 2, null },
                    { 1040, 62, 0, 0, 3, null },
                    { 1041, 62, 0, 0, 4, null },
                    { 1042, 62, 0, 0, 5, null },
                    { 1043, 62, 0, 0, 6, null },
                    { 1044, 62, 0, 0, 7, null },
                    { 1045, 62, 0, 0, 8, null },
                    { 1046, 62, 0, 0, 9, null },
                    { 1047, 62, 0, 0, 10, null },
                    { 1048, 62, 0, 0, 11, null },
                    { 1049, 62, 0, 0, 12, null },
                    { 1050, 62, 0, 0, 13, null },
                    { 1051, 62, 0, 0, 14, null },
                    { 1052, 62, 0, 0, 15, null },
                    { 1053, 62, 0, 0, 16, null },
                    { 1054, 62, 0, 0, 17, null },
                    { 1055, 63, 0, 0, 1, null },
                    { 1056, 63, 0, 0, 2, null },
                    { 1057, 63, 0, 0, 3, null },
                    { 1058, 63, 0, 0, 4, null },
                    { 1059, 63, 0, 0, 5, null },
                    { 1060, 63, 0, 0, 6, null },
                    { 1061, 63, 0, 0, 7, null },
                    { 1062, 63, 0, 0, 8, null },
                    { 1063, 63, 0, 0, 9, null },
                    { 1064, 63, 0, 0, 10, null },
                    { 1065, 63, 0, 0, 11, null },
                    { 1066, 63, 0, 0, 12, null },
                    { 1067, 63, 0, 0, 13, null },
                    { 1068, 63, 0, 0, 14, null },
                    { 1069, 63, 0, 0, 15, null },
                    { 1070, 63, 0, 0, 16, null },
                    { 1071, 63, 0, 0, 17, null },
                    { 1072, 64, 0, 0, 1, null },
                    { 1073, 64, 0, 0, 2, null },
                    { 1074, 64, 0, 0, 3, null },
                    { 1075, 64, 0, 0, 4, null },
                    { 1076, 64, 0, 0, 5, null },
                    { 1077, 64, 0, 0, 6, null },
                    { 1078, 64, 0, 0, 7, null },
                    { 1079, 64, 0, 0, 8, null },
                    { 1080, 64, 0, 0, 9, null },
                    { 1081, 64, 0, 0, 10, null },
                    { 1082, 64, 0, 0, 11, null },
                    { 1083, 64, 0, 0, 12, null },
                    { 1084, 64, 0, 0, 13, null },
                    { 1085, 64, 0, 0, 14, null },
                    { 1086, 64, 0, 0, 15, null },
                    { 1087, 64, 0, 0, 16, null },
                    { 1088, 64, 0, 0, 17, null },
                    { 1089, 65, 0, 0, 1, null },
                    { 1090, 65, 0, 0, 2, null },
                    { 1091, 65, 0, 0, 3, null },
                    { 1092, 65, 0, 0, 4, null },
                    { 1093, 65, 0, 0, 5, null },
                    { 1094, 65, 0, 0, 6, null },
                    { 1095, 65, 0, 0, 7, null },
                    { 1096, 65, 0, 0, 8, null },
                    { 1097, 65, 0, 0, 9, null },
                    { 1098, 65, 0, 0, 10, null },
                    { 1099, 65, 0, 0, 11, null },
                    { 1100, 65, 0, 0, 12, null },
                    { 1101, 65, 0, 0, 13, null },
                    { 1102, 65, 0, 0, 14, null },
                    { 1103, 65, 0, 0, 15, null },
                    { 1104, 65, 0, 0, 16, null },
                    { 1105, 65, 0, 0, 17, null },
                    { 1106, 66, 0, 0, 1, null },
                    { 1107, 66, 0, 0, 2, null },
                    { 1108, 66, 0, 0, 3, null },
                    { 1109, 66, 0, 0, 4, null },
                    { 1110, 66, 0, 0, 5, null },
                    { 1111, 66, 0, 0, 6, null },
                    { 1112, 66, 0, 0, 7, null },
                    { 1113, 66, 0, 0, 8, null },
                    { 1114, 66, 0, 0, 9, null },
                    { 1115, 66, 0, 0, 10, null },
                    { 1116, 66, 0, 0, 11, null },
                    { 1117, 66, 0, 0, 12, null },
                    { 1118, 66, 0, 0, 13, null },
                    { 1119, 66, 0, 0, 14, null },
                    { 1120, 66, 0, 0, 15, null },
                    { 1121, 66, 0, 0, 16, null },
                    { 1122, 66, 0, 0, 17, null },
                    { 1123, 67, 0, 0, 1, null },
                    { 1124, 67, 0, 0, 2, null },
                    { 1125, 67, 0, 0, 3, null },
                    { 1126, 67, 0, 0, 4, null },
                    { 1127, 67, 0, 0, 5, null },
                    { 1128, 67, 0, 0, 6, null },
                    { 1129, 67, 0, 0, 7, null },
                    { 1130, 67, 0, 0, 8, null },
                    { 1131, 67, 0, 0, 9, null },
                    { 1132, 67, 0, 0, 10, null },
                    { 1133, 67, 0, 0, 11, null },
                    { 1134, 67, 0, 0, 12, null },
                    { 1135, 67, 0, 0, 13, null },
                    { 1136, 67, 0, 0, 14, null },
                    { 1137, 67, 0, 0, 15, null },
                    { 1138, 67, 0, 0, 16, null },
                    { 1139, 67, 0, 0, 17, null },
                    { 1140, 68, 0, 0, 1, null },
                    { 1141, 68, 0, 0, 2, null },
                    { 1142, 68, 0, 0, 3, null },
                    { 1143, 68, 0, 0, 4, null },
                    { 1144, 68, 0, 0, 5, null },
                    { 1145, 68, 0, 0, 6, null },
                    { 1146, 68, 0, 0, 7, null },
                    { 1147, 68, 0, 0, 8, null },
                    { 1148, 68, 0, 0, 9, null },
                    { 1149, 68, 0, 0, 10, null },
                    { 1150, 68, 0, 0, 11, null },
                    { 1151, 68, 0, 0, 12, null },
                    { 1152, 68, 0, 0, 13, null },
                    { 1153, 68, 0, 0, 14, null },
                    { 1154, 68, 0, 0, 15, null },
                    { 1155, 68, 0, 0, 16, null },
                    { 1156, 68, 0, 0, 17, null },
                    { 1157, 69, 0, 0, 1, null },
                    { 1158, 69, 0, 0, 2, null },
                    { 1159, 69, 0, 0, 3, null },
                    { 1160, 69, 0, 0, 4, null },
                    { 1161, 69, 0, 0, 5, null },
                    { 1162, 69, 0, 0, 6, null },
                    { 1163, 69, 0, 0, 7, null },
                    { 1164, 69, 0, 0, 8, null },
                    { 1165, 69, 0, 0, 9, null },
                    { 1166, 69, 0, 0, 10, null },
                    { 1167, 69, 0, 0, 11, null },
                    { 1168, 69, 0, 0, 12, null },
                    { 1169, 69, 0, 0, 13, null },
                    { 1170, 69, 0, 0, 14, null },
                    { 1171, 69, 0, 0, 15, null },
                    { 1172, 69, 0, 0, 16, null },
                    { 1173, 69, 0, 0, 17, null },
                    { 1174, 70, 0, 0, 1, null },
                    { 1175, 70, 0, 0, 2, null },
                    { 1176, 70, 0, 0, 3, null },
                    { 1177, 70, 0, 0, 4, null },
                    { 1178, 70, 0, 0, 5, null },
                    { 1179, 70, 0, 0, 6, null },
                    { 1180, 70, 0, 0, 7, null },
                    { 1181, 70, 0, 0, 8, null },
                    { 1182, 70, 0, 0, 9, null },
                    { 1183, 70, 0, 0, 10, null },
                    { 1184, 70, 0, 0, 11, null },
                    { 1185, 70, 0, 0, 12, null },
                    { 1186, 70, 0, 0, 13, null },
                    { 1187, 70, 0, 0, 14, null },
                    { 1188, 70, 0, 0, 15, null },
                    { 1189, 70, 0, 0, 16, null },
                    { 1190, 70, 0, 0, 17, null },
                    { 1191, 71, 0, 0, 1, null },
                    { 1192, 71, 0, 0, 2, null },
                    { 1193, 71, 0, 0, 3, null },
                    { 1194, 71, 0, 0, 4, null },
                    { 1195, 71, 0, 0, 5, null },
                    { 1196, 71, 0, 0, 6, null },
                    { 1197, 71, 0, 0, 7, null },
                    { 1198, 71, 0, 0, 8, null },
                    { 1199, 71, 0, 0, 9, null },
                    { 1200, 71, 0, 0, 10, null },
                    { 1201, 71, 0, 0, 11, null },
                    { 1202, 71, 0, 0, 12, null },
                    { 1203, 71, 0, 0, 13, null },
                    { 1204, 71, 0, 0, 14, null },
                    { 1205, 71, 0, 0, 15, null },
                    { 1206, 71, 0, 0, 16, null },
                    { 1207, 71, 0, 0, 17, null },
                    { 1208, 72, 0, 0, 1, null },
                    { 1209, 72, 0, 0, 2, null },
                    { 1210, 72, 0, 0, 3, null },
                    { 1211, 72, 0, 0, 4, null },
                    { 1212, 72, 0, 0, 5, null },
                    { 1213, 72, 0, 0, 6, null },
                    { 1214, 72, 0, 0, 7, null },
                    { 1215, 72, 0, 0, 8, null },
                    { 1216, 72, 0, 0, 9, null },
                    { 1217, 72, 0, 0, 10, null },
                    { 1218, 72, 0, 0, 11, null },
                    { 1219, 72, 0, 0, 12, null },
                    { 1220, 72, 0, 0, 13, null },
                    { 1221, 72, 0, 0, 14, null },
                    { 1222, 72, 0, 0, 15, null },
                    { 1223, 72, 0, 0, 16, null },
                    { 1224, 72, 0, 0, 17, null },
                    { 1225, 73, 0, 0, 1, null },
                    { 1226, 73, 0, 0, 2, null },
                    { 1227, 73, 0, 0, 3, null },
                    { 1228, 73, 0, 0, 4, null },
                    { 1229, 73, 0, 0, 5, null },
                    { 1230, 73, 0, 0, 6, null },
                    { 1231, 73, 0, 0, 7, null },
                    { 1232, 73, 0, 0, 8, null },
                    { 1233, 73, 0, 0, 9, null },
                    { 1234, 73, 0, 0, 10, null },
                    { 1235, 73, 0, 0, 11, null },
                    { 1236, 73, 0, 0, 12, null },
                    { 1237, 73, 0, 0, 13, null },
                    { 1238, 73, 0, 0, 14, null },
                    { 1239, 73, 0, 0, 15, null },
                    { 1240, 73, 0, 0, 16, null },
                    { 1241, 73, 0, 0, 17, null },
                    { 1242, 74, 0, 0, 1, null },
                    { 1243, 74, 0, 0, 2, null },
                    { 1244, 74, 0, 0, 3, null },
                    { 1245, 74, 0, 0, 4, null },
                    { 1246, 74, 0, 0, 5, null },
                    { 1247, 74, 0, 0, 6, null },
                    { 1248, 74, 0, 0, 7, null },
                    { 1249, 74, 0, 0, 8, null },
                    { 1250, 74, 0, 0, 9, null },
                    { 1251, 74, 0, 0, 10, null },
                    { 1252, 74, 0, 0, 11, null },
                    { 1253, 74, 0, 0, 12, null },
                    { 1254, 74, 0, 0, 13, null },
                    { 1255, 74, 0, 0, 14, null },
                    { 1256, 74, 0, 0, 15, null },
                    { 1257, 74, 0, 0, 16, null },
                    { 1258, 74, 0, 0, 17, null },
                    { 1259, 75, 0, 0, 1, null },
                    { 1260, 75, 0, 0, 2, null },
                    { 1261, 75, 0, 0, 3, null },
                    { 1262, 75, 0, 0, 4, null },
                    { 1263, 75, 0, 0, 5, null },
                    { 1264, 75, 0, 0, 6, null },
                    { 1265, 75, 0, 0, 7, null },
                    { 1266, 75, 0, 0, 8, null },
                    { 1267, 75, 0, 0, 9, null },
                    { 1268, 75, 0, 0, 10, null },
                    { 1269, 75, 0, 0, 11, null },
                    { 1270, 75, 0, 0, 12, null },
                    { 1271, 75, 0, 0, 13, null },
                    { 1272, 75, 0, 0, 14, null },
                    { 1273, 75, 0, 0, 15, null },
                    { 1274, 75, 0, 0, 16, null },
                    { 1275, 75, 0, 0, 17, null },
                    { 1276, 76, 0, 0, 1, null },
                    { 1277, 76, 0, 0, 2, null },
                    { 1278, 76, 0, 0, 3, null },
                    { 1279, 76, 0, 0, 4, null },
                    { 1280, 76, 0, 0, 5, null },
                    { 1281, 76, 0, 0, 6, null },
                    { 1282, 76, 0, 0, 7, null },
                    { 1283, 76, 0, 0, 8, null },
                    { 1284, 76, 0, 0, 9, null },
                    { 1285, 76, 0, 0, 10, null },
                    { 1286, 76, 0, 0, 11, null },
                    { 1287, 76, 0, 0, 12, null },
                    { 1288, 76, 0, 0, 13, null },
                    { 1289, 76, 0, 0, 14, null },
                    { 1290, 76, 0, 0, 15, null },
                    { 1291, 76, 0, 0, 16, null },
                    { 1292, 76, 0, 0, 17, null },
                    { 1293, 77, 0, 0, 1, null },
                    { 1294, 77, 0, 0, 2, null },
                    { 1295, 77, 0, 0, 3, null },
                    { 1296, 77, 0, 0, 4, null },
                    { 1297, 77, 0, 0, 5, null },
                    { 1298, 77, 0, 0, 6, null },
                    { 1299, 77, 0, 0, 7, null },
                    { 1300, 77, 0, 0, 8, null },
                    { 1301, 77, 0, 0, 9, null },
                    { 1302, 77, 0, 0, 10, null },
                    { 1303, 77, 0, 0, 11, null },
                    { 1304, 77, 0, 0, 12, null },
                    { 1305, 77, 0, 0, 13, null },
                    { 1306, 77, 0, 0, 14, null },
                    { 1307, 77, 0, 0, 15, null },
                    { 1308, 77, 0, 0, 16, null },
                    { 1309, 77, 0, 0, 17, null },
                    { 1310, 78, 0, 0, 1, null },
                    { 1311, 78, 0, 0, 2, null },
                    { 1312, 78, 0, 0, 3, null },
                    { 1313, 78, 0, 0, 4, null },
                    { 1314, 78, 0, 0, 5, null },
                    { 1315, 78, 0, 0, 6, null },
                    { 1316, 78, 0, 0, 7, null },
                    { 1317, 78, 0, 0, 8, null },
                    { 1318, 78, 0, 0, 9, null },
                    { 1319, 78, 0, 0, 10, null },
                    { 1320, 78, 0, 0, 11, null },
                    { 1321, 78, 0, 0, 12, null },
                    { 1322, 78, 0, 0, 13, null },
                    { 1323, 78, 0, 0, 14, null },
                    { 1324, 78, 0, 0, 15, null },
                    { 1325, 78, 0, 0, 16, null },
                    { 1326, 78, 0, 0, 17, null },
                    { 1327, 79, 0, 0, 1, null },
                    { 1328, 79, 0, 0, 2, null },
                    { 1329, 79, 0, 0, 3, null },
                    { 1330, 79, 0, 0, 4, null },
                    { 1331, 79, 0, 0, 5, null },
                    { 1332, 79, 0, 0, 6, null },
                    { 1333, 79, 0, 0, 7, null },
                    { 1334, 79, 0, 0, 8, null },
                    { 1335, 79, 0, 0, 9, null },
                    { 1336, 79, 0, 0, 10, null },
                    { 1337, 79, 0, 0, 11, null },
                    { 1338, 79, 0, 0, 12, null },
                    { 1339, 79, 0, 0, 13, null },
                    { 1340, 79, 0, 0, 14, null },
                    { 1341, 79, 0, 0, 15, null },
                    { 1342, 79, 0, 0, 16, null },
                    { 1343, 79, 0, 0, 17, null },
                    { 1344, 80, 0, 0, 1, null },
                    { 1345, 80, 0, 0, 2, null },
                    { 1346, 80, 0, 0, 3, null },
                    { 1347, 80, 0, 0, 4, null },
                    { 1348, 80, 0, 0, 5, null },
                    { 1349, 80, 0, 0, 6, null },
                    { 1350, 80, 0, 0, 7, null },
                    { 1351, 80, 0, 0, 8, null },
                    { 1352, 80, 0, 0, 9, null },
                    { 1353, 80, 0, 0, 10, null },
                    { 1354, 80, 0, 0, 11, null },
                    { 1355, 80, 0, 0, 12, null },
                    { 1356, 80, 0, 0, 13, null },
                    { 1357, 80, 0, 0, 14, null },
                    { 1358, 80, 0, 0, 15, null },
                    { 1359, 80, 0, 0, 16, null },
                    { 1360, 80, 0, 0, 17, null },
                    { 1361, 81, 0, 0, 1, null },
                    { 1362, 81, 0, 0, 2, null },
                    { 1363, 81, 0, 0, 3, null },
                    { 1364, 81, 0, 0, 4, null },
                    { 1365, 81, 0, 0, 5, null },
                    { 1366, 81, 0, 0, 6, null },
                    { 1367, 81, 0, 0, 7, null },
                    { 1368, 81, 0, 0, 8, null },
                    { 1369, 81, 0, 0, 9, null },
                    { 1370, 81, 0, 0, 10, null },
                    { 1371, 81, 0, 0, 11, null },
                    { 1372, 81, 0, 0, 12, null },
                    { 1373, 81, 0, 0, 13, null },
                    { 1374, 81, 0, 0, 14, null },
                    { 1375, 81, 0, 0, 15, null },
                    { 1376, 81, 0, 0, 16, null },
                    { 1377, 81, 0, 0, 17, null },
                    { 1378, 82, 0, 0, 1, null },
                    { 1379, 82, 0, 0, 2, null },
                    { 1380, 82, 0, 0, 3, null },
                    { 1381, 82, 0, 0, 4, null },
                    { 1382, 82, 0, 0, 5, null },
                    { 1383, 82, 0, 0, 6, null },
                    { 1384, 82, 0, 0, 7, null },
                    { 1385, 82, 0, 0, 8, null },
                    { 1386, 82, 0, 0, 9, null },
                    { 1387, 82, 0, 0, 10, null },
                    { 1388, 82, 0, 0, 11, null },
                    { 1389, 82, 0, 0, 12, null },
                    { 1390, 82, 0, 0, 13, null },
                    { 1391, 82, 0, 0, 14, null },
                    { 1392, 82, 0, 0, 15, null },
                    { 1393, 82, 0, 0, 16, null },
                    { 1394, 82, 0, 0, 17, null },
                    { 1395, 83, 0, 0, 1, null },
                    { 1396, 83, 0, 0, 2, null },
                    { 1397, 83, 0, 0, 3, null },
                    { 1398, 83, 0, 0, 4, null },
                    { 1399, 83, 0, 0, 5, null },
                    { 1400, 83, 0, 0, 6, null },
                    { 1401, 83, 0, 0, 7, null },
                    { 1402, 83, 0, 0, 8, null },
                    { 1403, 83, 0, 0, 9, null },
                    { 1404, 83, 0, 0, 10, null },
                    { 1405, 83, 0, 0, 11, null },
                    { 1406, 83, 0, 0, 12, null },
                    { 1407, 83, 0, 0, 13, null },
                    { 1408, 83, 0, 0, 14, null },
                    { 1409, 83, 0, 0, 15, null },
                    { 1410, 83, 0, 0, 16, null },
                    { 1411, 83, 0, 0, 17, null },
                    { 1412, 84, 0, 0, 1, null },
                    { 1413, 84, 0, 0, 2, null },
                    { 1414, 84, 0, 0, 3, null },
                    { 1415, 84, 0, 0, 4, null },
                    { 1416, 84, 0, 0, 5, null },
                    { 1417, 84, 0, 0, 6, null },
                    { 1418, 84, 0, 0, 7, null },
                    { 1419, 84, 0, 0, 8, null },
                    { 1420, 84, 0, 0, 9, null },
                    { 1421, 84, 0, 0, 10, null },
                    { 1422, 84, 0, 0, 11, null },
                    { 1423, 84, 0, 0, 12, null },
                    { 1424, 84, 0, 0, 13, null },
                    { 1425, 84, 0, 0, 14, null },
                    { 1426, 84, 0, 0, 15, null },
                    { 1427, 84, 0, 0, 16, null },
                    { 1428, 84, 0, 0, 17, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 953);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 954);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 955);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 956);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 957);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 958);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 959);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 960);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 961);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 962);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 963);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 964);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 965);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 966);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 967);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 968);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 969);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 970);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 971);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 972);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 973);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 974);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 975);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 976);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 977);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 978);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 979);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 980);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 981);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 982);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 983);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 984);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 985);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 986);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 987);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 988);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 989);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 990);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 991);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 992);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 993);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 994);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 995);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 996);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 997);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 998);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 999);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1000);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1009);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1010);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1011);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1012);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1013);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1014);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1015);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1016);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1017);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1018);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1019);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1020);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1021);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1022);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1023);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1024);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1025);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1026);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1027);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1028);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1029);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1030);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1031);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1032);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1033);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1034);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1035);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1036);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1037);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1038);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1039);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1040);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1041);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1042);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1043);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1044);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1045);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1046);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1047);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1048);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1049);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1050);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1051);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1052);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1053);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1054);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1055);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1056);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1057);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1058);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1059);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1060);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1061);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1062);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1063);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1064);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1065);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1066);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1067);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1068);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1069);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1070);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1071);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1072);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1073);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1074);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1075);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1076);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1077);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1078);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1079);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1080);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1081);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1082);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1083);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1084);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1085);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1086);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1087);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1088);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1089);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1090);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1091);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1092);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1093);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1094);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1095);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1096);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1097);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1098);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1099);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1100);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1101);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1102);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1103);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1104);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1105);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1106);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1107);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1108);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1109);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1110);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1111);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1112);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1113);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1114);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1115);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1116);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1117);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1118);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1119);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1120);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1121);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1122);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1123);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1124);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1125);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1126);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1127);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1128);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1129);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1130);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1131);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1132);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1133);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1134);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1135);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1136);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1137);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1138);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1139);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1140);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1141);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1142);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1143);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1144);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1145);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1146);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1147);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1148);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1149);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1150);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1151);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1152);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1153);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1154);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1155);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1156);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1157);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1158);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1159);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1160);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1161);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1162);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1163);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1164);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1165);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1166);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1167);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1168);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1169);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1170);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1171);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1172);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1173);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1174);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1175);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1176);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1177);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1178);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1179);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1180);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1181);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1182);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1183);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1184);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1185);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1186);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1187);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1188);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1189);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1190);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1191);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1192);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1193);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1194);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1195);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1196);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1197);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1198);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1199);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1200);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1201);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1202);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1203);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1204);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1205);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1206);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1207);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1208);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1209);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1210);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1211);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1212);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1213);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1214);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1215);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1216);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1217);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1218);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1219);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1220);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1221);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1222);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1223);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1224);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1225);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1226);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1227);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1228);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1229);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1230);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1231);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1232);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1233);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1234);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1235);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1236);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1237);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1238);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1239);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1240);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1241);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1242);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1243);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1244);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1245);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1246);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1247);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1248);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1249);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1250);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1251);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1252);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1253);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1254);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1255);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1256);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1257);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1258);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1259);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1260);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1261);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1262);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1263);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1264);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1265);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1266);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1267);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1268);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1269);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1270);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1271);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1272);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1273);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1274);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1275);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1276);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1277);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1278);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1279);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1280);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1281);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1282);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1283);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1284);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1285);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1286);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1287);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1288);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1289);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1290);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1291);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1292);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1293);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1294);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1295);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1296);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1297);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1298);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1299);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1300);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1301);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1302);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1303);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1304);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1305);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1306);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1307);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1308);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1309);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1310);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1311);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1312);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1313);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1314);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1315);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1316);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1317);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1318);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1319);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1320);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1321);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1322);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1323);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1324);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1325);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1326);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1327);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1328);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1329);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1330);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1331);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1332);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1333);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1334);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1335);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1336);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1337);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1338);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1339);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1340);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1341);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1342);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1343);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1344);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1345);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1346);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1347);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1348);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1349);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1350);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1351);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1352);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1353);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1354);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1355);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1356);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1357);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1358);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1359);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1360);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1361);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1362);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1363);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1364);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1365);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1366);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1367);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1368);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1369);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1370);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1371);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1372);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1373);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1374);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1375);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1376);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1377);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1378);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1379);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1380);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1381);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1382);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1383);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1384);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1385);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1386);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1387);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1388);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1389);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1390);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1391);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1392);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1393);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1394);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1395);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1396);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1397);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1398);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1399);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1400);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1401);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1402);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1403);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1404);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1405);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1406);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1407);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1408);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1409);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1410);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1411);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1412);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1413);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1414);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1415);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1416);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1417);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1418);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1419);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1420);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1421);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1422);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1423);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1424);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1425);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1426);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1427);

            migrationBuilder.DeleteData(
                table: "BestSpecializationScore",
                keyColumn: "Id",
                keyValue: 1428);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Boss",
                keyColumn: "Id",
                keyValue: 84);
        }
    }
}
