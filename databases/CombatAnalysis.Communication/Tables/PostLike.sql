CREATE TABLE [dbo].[PostLike]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [PostId] INT NOT NULL, 
    [AppUserId] NVARCHAR(50) NOT NULL
)
