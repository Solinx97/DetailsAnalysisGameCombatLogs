CREATE PROCEDURE [dbo].[DeleteDamageTakenById]
	@id int
AS
	DELETE
	FROM DamageTaken
	WHERE Id = @id
RETURN 0
