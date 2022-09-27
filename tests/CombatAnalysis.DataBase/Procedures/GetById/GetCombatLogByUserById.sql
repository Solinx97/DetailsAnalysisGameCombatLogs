CREATE PROCEDURE [dbo].[GetCombatLogByUserById]
	@id int
AS
	SELECT *
	FROM CombatLogByUser
	WHERE Id = @id
RETURN 0
