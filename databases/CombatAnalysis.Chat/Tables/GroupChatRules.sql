CREATE TABLE [dbo].[GroupChatRules]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [InvitePeople] INT NOT NULL, 
    [RemovePeople] INT NOT NULL, 
    [PinMessage] INT NOT NULL, 
    [Announcements] INT NOT NULL, 
    [GroupChatId] INT NOT NULL
)
