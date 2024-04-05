CREATE PROCEDURE DeleteDamageDoneById (@id INT)
	AS DELETE FROM DamageDone
	WHERE Id = @id