CREATE TABLE [dbo].[GroupChatUser]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Username] NVARCHAR(50) NOT NULL, 
    [AppUserId] NVARCHAR(50) NOT NULL, 
    [GroupChatId] INT NOT NULL
)
