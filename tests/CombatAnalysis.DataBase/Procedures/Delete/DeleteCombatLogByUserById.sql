CREATE PROCEDURE [dbo].[DeleteCombatLogByUserById]
	@id int
AS
	DELETE
	FROM CombatLogByUser
	WHERE Id = @id
RETURN 0
