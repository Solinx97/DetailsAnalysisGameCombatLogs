CREATE PROCEDURE DeleteDamageTakenGeneralById (@id INT)
	AS DELETE FROM DamageTakenGeneral
	WHERE Id = @id