CREATE TABLE [dbo].[Post]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Owner] NVARCHAR(50) NOT NULL, 
    [Content] NVARCHAR(MAX) NOT NULL, 
    [PostType] INT NOT NULL, 
    [Tags] NVARCHAR(MAX) NOT NULL, 
    [When] DATETIMEOFFSET NOT NULL, 
    [LikeCount] INT NOT NULL, 
    [DislikeCount] INT NOT NULL, 
    [CommentCount] INT NOT NULL, 
    [AppUserId] NVARCHAR(50) NOT NULL
)
