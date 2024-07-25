CREATE TABLE [dbo].[PersonalChat]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [LastMessage] NVARCHAR(MAX) NOT NULL, 
    [InitiatorId] NVARCHAR(50) NOT NULL, 
    [CompanionId] NVARCHAR(50) NOT NULL
)
