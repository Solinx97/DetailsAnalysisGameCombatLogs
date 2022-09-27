CREATE PROCEDURE [dbo].[DeleteGroupChatMessageById]
	@id int
AS
	DELETE
	FROM GroupChatMessage
	WHERE Id = @id
RETURN 0
