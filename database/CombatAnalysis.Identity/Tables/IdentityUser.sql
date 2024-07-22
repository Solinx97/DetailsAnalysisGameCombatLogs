CREATE TABLE [dbo].[IdentityUser]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Email] NVARCHAR(50) NOT NULL, 
    [PasswordHash] NVARCHAR(50) NOT NULL, 
    [Salt] NVARCHAR(50) NOT NULL
)
