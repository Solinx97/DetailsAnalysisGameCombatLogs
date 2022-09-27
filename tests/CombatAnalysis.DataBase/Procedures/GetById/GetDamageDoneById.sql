CREATE PROCEDURE [dbo].[GetDamageDoneById]
	@id int
AS
	SELECT *
	FROM DamageDone
	WHERE Id = @id
RETURN 0
