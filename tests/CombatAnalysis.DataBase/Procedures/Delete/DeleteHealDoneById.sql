CREATE PROCEDURE [dbo].[DeleteHealDoneById]
	@id int
AS
	DELETE
	FROM HealDone
	WHERE Id = @id
RETURN 0
