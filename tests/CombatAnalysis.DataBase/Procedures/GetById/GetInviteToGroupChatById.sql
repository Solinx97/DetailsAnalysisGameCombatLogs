CREATE PROCEDURE [dbo].[GetInviteToGroupChatById]
	@id int
AS
	SELECT *
	FROM InviteToGroupChat
	WHERE Id = @id
RETURN 0
