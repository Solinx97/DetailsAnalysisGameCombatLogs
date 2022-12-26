CREATE TABLE [dbo].[InviteToGroupChat] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      NVARCHAR (MAX) NULL,
    [Response]    INT            NOT NULL,
    [GroupChatId] INT            NOT NULL,
    CONSTRAINT [PK_InviteToGroupChat] PRIMARY KEY CLUSTERED ([Id] ASC)
);

