CREATE TABLE [dbo].[GroupChat] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (MAX) NULL,
    [ShortName]      NVARCHAR (MAX) NULL,
    [LastMessage]    NVARCHAR (MAX) NULL,
    [MemberNumber]   INT            NOT NULL,
    [ChatPolicyType] INT            NOT NULL,
    [OwnerId]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_GroupChat] PRIMARY KEY CLUSTERED ([Id] ASC)
);

