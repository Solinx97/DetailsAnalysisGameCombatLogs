CREATE TABLE [dbo].[CombatLog] (
    [Id]      INT                IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (MAX)     NULL,
    [Date]    DATETIMEOFFSET (7) NOT NULL,
    [IsReady] BIT                NOT NULL,
    CONSTRAINT [PK_CombatLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

