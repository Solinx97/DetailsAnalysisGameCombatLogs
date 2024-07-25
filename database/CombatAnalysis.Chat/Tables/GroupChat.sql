CREATE TABLE [dbo].[GroupChat]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [LastMessage] NVARCHAR(MAX) NOT NULL, 
    [AppUserId] NVARCHAR(50) NOT NULL
)
