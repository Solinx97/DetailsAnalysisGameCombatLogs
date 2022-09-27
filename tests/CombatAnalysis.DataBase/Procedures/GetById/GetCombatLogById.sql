CREATE PROCEDURE [dbo].[GetCombatLogById]
	@id int
AS
	SELECT *
	FROM CombatLog
	WHERE Id = @id
RETURN 0
