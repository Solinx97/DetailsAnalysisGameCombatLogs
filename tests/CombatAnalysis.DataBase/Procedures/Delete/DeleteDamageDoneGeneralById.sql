CREATE PROCEDURE [dbo].[DeleteDamageDoneGeneralById]
	@id int
AS
	DELETE
	FROM DamageDoneGeneral
	WHERE Id = @id
RETURN 0
