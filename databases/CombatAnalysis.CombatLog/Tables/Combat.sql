CREATE TABLE [dbo].[Combat] (
    [Id]             INT                IDENTITY (1, 1) NOT NULL,
    [LocallyNumber]  INT                NOT NULL,
    [DungeonName]    NVARCHAR (MAX)     NOT NULL,
    [Name]           NVARCHAR (MAX)     NOT NULL,
    [Difficulty]     INT                NOT NULL,
    [DamageDone]     INT                NOT NULL,
    [HealDone]       INT                NOT NULL,
    [DamageTaken]    INT                NOT NULL,
    [EnergyRecovery] INT                NOT NULL,
    [DeathNumber]    INT                NOT NULL,
    [IsWin]          BIT                NOT NULL,
    [StartDate]      DATETIMEOFFSET (7) NOT NULL,
    [FinishDate]     DATETIMEOFFSET (7) NOT NULL,
    [IsReady]        BIT                NOT NULL,
    [CombatLogId]    INT                NOT NULL,
    CONSTRAINT [PK_Combat] PRIMARY KEY CLUSTERED ([Id] ASC)
);

