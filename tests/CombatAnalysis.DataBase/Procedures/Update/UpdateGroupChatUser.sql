CREATE PROCEDURE [dbo].[UpdateGroupChatUser]
	@Id INT,
	@UserId NVARCHAR (MAX),
	@GroupChatId INT
AS
	UPDATE GroupChatUser
	SET UserId = @UserId,GroupChatId = @GroupChatId
	WHERE Id = @Id
RETURN 0
