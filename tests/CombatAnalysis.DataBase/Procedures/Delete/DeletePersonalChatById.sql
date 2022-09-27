CREATE PROCEDURE [dbo].[DeletePersonalChatById]
	@id int
AS
	DELETE
	FROM PersonalChat
	WHERE Id = @id
RETURN 0
