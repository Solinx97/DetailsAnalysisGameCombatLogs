CREATE TABLE [dbo].[Community]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NCHAR(10) NOT NULL, 
    [Description] NCHAR(10) NOT NULL, 
    [PolicyType] INT NOT NULL, 
    [AppUserId] NCHAR(10) NOT NULL
)
