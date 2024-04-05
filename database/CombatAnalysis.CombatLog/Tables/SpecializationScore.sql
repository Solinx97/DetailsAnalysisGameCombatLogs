CREATE TABLE [dbo].[SpecializationScore] (
    [Id]        INT                IDENTITY (1, 1) NOT NULL,
    [SpecId]    INT                NOT NULL,
    [BossId]    INT                NOT NULL,
    [Difficult] INT                NOT NULL,
    [Damage]    INT                NOT NULL,
    [Heal]      INT                NOT NULL,
    [Updated]   DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_SpecializationScore] PRIMARY KEY CLUSTERED ([Id] ASC)
);

