CREATE TABLE [dbo].[CombatPlayer] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [UserName]       NVARCHAR (MAX) NULL,
    [EnergyRecovery] INT            NOT NULL,
    [DamageDone]     INT            NOT NULL,
    [HealDone]       INT            NOT NULL,
    [DamageTaken]    INT            NOT NULL,
    [UsedBuffs]      INT            NOT NULL,
    [CombatId]       INT            NOT NULL,
    CONSTRAINT [PK_CombatPlayer] PRIMARY KEY CLUSTERED ([Id] ASC)
);

