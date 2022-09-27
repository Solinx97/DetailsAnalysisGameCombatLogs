CREATE PROCEDURE [dbo].[GetPersonalChatById]
	@id int
AS
	SELECT *
	FROM PersonalChat
	WHERE Id = @id
RETURN 0
