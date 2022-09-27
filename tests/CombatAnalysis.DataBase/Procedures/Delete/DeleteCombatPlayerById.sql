CREATE PROCEDURE [dbo].[DeleteCombatPlayerById]
	@id int
AS
	DELETE
	FROM CombatPlayer
	WHERE Id = @id
RETURN 0
