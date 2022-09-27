CREATE PROCEDURE [dbo].[DeleteHealDoneGeneralById]
	@id int
AS
	DELETE
	FROM HealDoneGeneral
	WHERE Id = @id
RETURN 0
