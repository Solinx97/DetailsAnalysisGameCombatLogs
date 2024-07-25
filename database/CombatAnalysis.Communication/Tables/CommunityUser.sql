CREATE TABLE [dbo].[CommunityUser]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Username] NVARCHAR(50) NOT NULL, 
    [AppUserId] NVARCHAR(50) NOT NULL, 
    [CommunityId] INT NOT NULL
)
