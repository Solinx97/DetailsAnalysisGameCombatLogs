CREATE PROCEDURE [dbo].[GetCombatPlayerById]
	@id int
AS
	SELECT *
	FROM CombatPlayer
	WHERE Id = @id
RETURN 0
