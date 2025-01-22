CREATE PROCEDURE DeleteDamageDoneGeneralById (@id INT)
	AS DELETE FROM DamageDoneGeneral
	WHERE Id = @id