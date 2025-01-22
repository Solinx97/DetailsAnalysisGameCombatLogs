CREATE TABLE [dbo].[PlayerDeath]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Date] DATETIMEOFFSET NOT NULL, 
    [CombatPlayerId] INT NOT NULL
)
