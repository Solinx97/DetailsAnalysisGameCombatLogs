CREATE PROCEDURE GetDamageTakenGeneralById (@id INT)
	AS SELECT * 
	FROM DamageTakenGeneral
	WHERE Id = @id