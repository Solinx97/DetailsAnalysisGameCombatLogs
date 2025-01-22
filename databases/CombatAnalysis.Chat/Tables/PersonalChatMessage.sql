CREATE TABLE [dbo].[PersonalChatMessage]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Message] NVARCHAR(MAX) NOT NULL, 
    [Time] NCHAR(10) NOT NULL, 
    [Status] INT NOT NULL, 
    [Type] INT NOT NULL, 
    [PersonalChatId] INT NOT NULL, 
    [AppUserId] NVARCHAR(50) NOT NULL
)
