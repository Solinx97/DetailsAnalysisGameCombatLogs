CREATE PROCEDURE [dbo].[DeleteGroupChatUserById]
	@id int
AS
	DELETE
	FROM GroupChatUser
	WHERE Id = @id
RETURN 0
