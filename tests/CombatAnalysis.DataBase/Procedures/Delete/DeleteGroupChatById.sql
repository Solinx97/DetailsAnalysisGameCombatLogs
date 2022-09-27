CREATE PROCEDURE [dbo].[DeleteGroupChatById]
	@id int
AS
	DELETE
	FROM GroupChat
	WHERE Id = @id
RETURN 0
