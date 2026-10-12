//using CombatAnalysis.NotificationDAL.Entities;

//namespace CombatAnalysis.NotificationDAL.IntegrationTests.Factory;

//internal static class NotificationTestDataFactory
//{
//    public static Notification Create(int id = 1, string initiatorId = "uid-22")
//    {
//        var collection = new Notification
//        {
//            Id = id,
//            Type = 0,
//            Status = 0,
//            InitiatorId = initiatorId,
//            InitiatorName = "Solinx",
//            RecipientId = "uid-23",
//            CreatedAt = DateTime.UtcNow.AddHours(1),
//            ReadAt = null
//        };

//        return collection;
//    }

//    public static List<Notification> CreateCollection()
//    {
//        var collection = new List<Notification>
//        {
//            new () {
//                Id = 1,
//                Type = 0,
//                Status = 0,
//                InitiatorId = "uid-22",
//                InitiatorName = "Solinx",
//                RecipientId = "uid-23",
//                CreatedAt = DateTime.UtcNow.AddHours(1),
//                ReadAt = null
//            },
//            new () {
//                Id = 2,
//                Type = 0,
//                Status = 0,
//                InitiatorId = "uid-22",
//                InitiatorName = "Solinx",
//                RecipientId = "uid-24",
//                CreatedAt = DateTime.UtcNow.AddHours(1),
//                ReadAt = null
//            },
//            new () {
//                Id = 31,
//                Type = 0,
//                Status = 0,
//                InitiatorId = "uid-22",
//                InitiatorName = "Solinx",
//                RecipientId = "uid-25",
//                CreatedAt = DateTime.UtcNow.AddHours(1),
//                ReadAt = null
//            }
//        };

//        return collection;
//    }
//}
