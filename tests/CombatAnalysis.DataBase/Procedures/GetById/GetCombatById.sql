CREATE PROCEDURE [dbo].[GetCombatById]
	@id int
AS
	SELECT *
	FROM Combat
	WHERE Id = @id
RETURN 0
