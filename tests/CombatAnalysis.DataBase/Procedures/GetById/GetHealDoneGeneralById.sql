CREATE PROCEDURE [dbo].[GetHealDoneGeneralById]
	@id int
AS
	SELECT *
	FROM HealDoneGeneral
	WHERE Id = @id
RETURN 0
