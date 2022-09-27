CREATE PROCEDURE [dbo].[DeleteCombatLogById]
	@id int
AS
	DELETE
	FROM CombatLog
	WHERE Id = @id
RETURN 0
