CREATE PROCEDURE [dbo].[DeleteDamageTakenGeneralById]
	@id int
AS
	DELETE
	FROM DamageTakenGeneral
	WHERE Id = @id
RETURN 0
