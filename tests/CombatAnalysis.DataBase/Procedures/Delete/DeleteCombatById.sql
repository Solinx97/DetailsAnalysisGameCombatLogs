CREATE PROCEDURE [dbo].[DeleteCombatById]
	@id int
AS
	DELETE
	FROM Combat
	WHERE Id = @id
RETURN 0
