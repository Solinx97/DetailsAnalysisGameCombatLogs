CREATE PROCEDURE [dbo].[GetDamageTakenGeneralById]
	@id int
AS
	SELECT *
	FROM DamageTakenGeneral
	WHERE Id = @id
RETURN 0
