CREATE PROCEDURE [dbo].[DeleteInviteToGroupChatById]
	@id int
AS
	DELETE
	FROM InviteToGroupChat
	WHERE Id = @id
RETURN 0
