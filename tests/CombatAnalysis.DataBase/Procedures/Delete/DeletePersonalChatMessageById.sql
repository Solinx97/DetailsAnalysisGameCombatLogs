CREATE PROCEDURE [dbo].[DeletePersonalChatMessageById]
	@id int
AS
	DELETE
	FROM PersonalChatMessage
	WHERE Id = @id
RETURN 0
