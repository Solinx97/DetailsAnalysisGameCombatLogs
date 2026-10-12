//using CombatAnalysis.NotificationBL.DTO;
//using CombatAnalysis.NotificationDAL.Entities;

//namespace CombatAnalysis.NotificationBL.Tests.Factory;

//internal class NotificationTestDataFactory
//{
//    public static Notification Create(int id = 1, string initiatorId = "uid-22")
//    {
//        var entity = new Notification
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

//        return entity;
//    }

//    public static NotificationDto CreateDto(int id = 1, string initiatorId = "uid-22")
//    {
//        var entityDto = new NotificationDto
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

//        return entityDto;
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

//    public static List<NotificationDto> CreateDtoCollection()
//    {
//        var collection = new List<NotificationDto>
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
