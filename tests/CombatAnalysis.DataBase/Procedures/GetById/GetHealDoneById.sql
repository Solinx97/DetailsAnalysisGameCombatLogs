CREATE PROCEDURE [dbo].[GetHealDoneById]
	@id int
AS
	SELECT *
	FROM HealDone
	WHERE Id = @id
RETURN 0
