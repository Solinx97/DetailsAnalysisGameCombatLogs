CREATE PROCEDURE [dbo].[GetDamageTakenById]
	@id int
AS
	SELECT *
	FROM DamageTaken
	WHERE Id = @id
RETURN 0
