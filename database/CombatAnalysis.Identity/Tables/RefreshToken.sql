﻿CREATE TABLE [dbo].[RefreshToken]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Token] NVARCHAR(50) NOT NULL, 
    [ExpiryTime] DATETIMEOFFSET NOT NULL, 
    [ClientId] NVARCHAR(50) NOT NULL, 
    [UserId] NVARCHAR(50) NOT NULL
)
