CREATE PROCEDURE [dbo].[GetPersonalChatMessageById]
	@id int
AS
	SELECT *
	FROM PersonalChatMessage
	WHERE Id = @id
RETURN 0
