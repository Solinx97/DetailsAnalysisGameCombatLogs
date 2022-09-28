CREATE TABLE [dbo].[RefreshToken] (
    [Id]      NVARCHAR (450)     NOT NULL,
    [UserId]  NVARCHAR (MAX)     NULL,
    [Token]   NVARCHAR (MAX)     NULL,
    [Expires] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED ([Id] ASC)
);
