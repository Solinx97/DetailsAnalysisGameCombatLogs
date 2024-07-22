CREATE TABLE [dbo].[GroupChatMessageCount]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Count] INT NOT NULL, 
    [GroupChatUserId] NVARCHAR(50) NOT NULL, 
    [GroupChatId] INT NOT NULL
)
