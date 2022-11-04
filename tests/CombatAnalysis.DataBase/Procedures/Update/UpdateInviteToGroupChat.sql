CREATE PROCEDURE [dbo].[UpdateInviteToGroupChat]
	@Id INT,
	@UserId NVARCHAR (MAX),
	@Response INT,
	@GroupChatId INT
AS
	UPDATE InviteToGroupChat
	SET UserId = @UserId,Response = @Response,GroupChatId = @GroupChatId
	WHERE Id = @Id
RETURN 0
