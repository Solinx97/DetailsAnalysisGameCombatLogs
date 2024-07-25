CREATE TABLE [dbo].[CommunityDiscussion]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Title] NCHAR(10) NOT NULL, 
    [Content] NCHAR(10) NOT NULL, 
    [When] NCHAR(10) NOT NULL, 
    [AppUserId] NCHAR(10) NOT NULL, 
    [CommunityId] INT NOT NULL
)
