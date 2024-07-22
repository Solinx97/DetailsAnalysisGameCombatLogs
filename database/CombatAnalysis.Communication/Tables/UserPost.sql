CREATE TABLE [dbo].[UserPost]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [AppUserId] NVARCHAR(50) NOT NULL, 
    [PostId] INT NOT NULL
)
