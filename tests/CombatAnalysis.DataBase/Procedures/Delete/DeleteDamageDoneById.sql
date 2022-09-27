CREATE PROCEDURE [dbo].[DeleteDamageDoneById]
	@id int
AS
	DELETE
	FROM DamageDone
	WHERE Id = @id
RETURN 0
