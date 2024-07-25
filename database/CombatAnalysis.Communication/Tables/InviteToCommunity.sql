CREATE TABLE [dbo].[InviteToCommunity]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [CommunityId] INT NOT NULL, 
    [ToAppUserId] NVARCHAR(50) NOT NULL, 
    [When] DATETIMEOFFSET NOT NULL, 
    [AppUserId] NVARCHAR(50) NOT NULL
)
