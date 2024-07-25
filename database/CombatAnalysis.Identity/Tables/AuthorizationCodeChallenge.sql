CREATE TABLE [dbo].[AuthorizationCodeChallenge]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [CodeChallenge] NVARCHAR(50) NOT NULL, 
    [CodeChallengeMethod] NVARCHAR(50) NOT NULL, 
    [RedirectUrl] NVARCHAR(50) NOT NULL, 
    [CreatedAt] DATETIMEOFFSET NOT NULL, 
    [ExpiryTime] DATETIMEOFFSET NOT NULL, 
    [ClientId] NVARCHAR(50) NOT NULL, 
    [IsUsed] BIT NOT NULL
)
