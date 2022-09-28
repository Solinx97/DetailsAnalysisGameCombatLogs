CREATE TABLE [dbo].[AppUser] (
    [Id]       NVARCHAR (450) NOT NULL,
    [Email]    NVARCHAR (MAX) NULL,
    [Password] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AppUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);

