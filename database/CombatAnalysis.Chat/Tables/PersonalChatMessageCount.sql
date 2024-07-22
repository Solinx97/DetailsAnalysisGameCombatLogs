CREATE TABLE [dbo].[PersonalChatMessageCount]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Count] INT NOT NULL, 
    [AppUserId] NVARCHAR(50) NOT NULL, 
    [PersonalChatId] INT NOT NULL
)
