CREATE TABLE [dbo].[UnreadGroupChatMessage]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [GroupChatUserId] NVARCHAR(50) NOT NULL, 
    [GroupChatMessageId] INT NOT NULL
)
