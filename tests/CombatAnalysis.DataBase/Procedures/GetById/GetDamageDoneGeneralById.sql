CREATE PROCEDURE [dbo].[GetDamageDoneGeneralById]
	@id int
AS
	SELECT *
	FROM DamageDoneGeneral
	WHERE Id = @id
RETURN 0
