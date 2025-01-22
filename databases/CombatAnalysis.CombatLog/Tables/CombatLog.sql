CREATE TABLE [dbo].[CombatLog] (
    [Id]      INT                IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (MAX)     NOT NULL,
    [Date]    DATETIMEOFFSET (7) NOT NULL,
    [IsReady] BIT                NOT NULL,
    [NumberReadyCombats] INT NOT NULL, 
    [CombatsInQueue] INT NOT NULL, 
    CONSTRAINT [PK_CombatLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

