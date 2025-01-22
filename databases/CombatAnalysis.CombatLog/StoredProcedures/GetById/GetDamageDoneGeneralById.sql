CREATE PROCEDURE GetDamageDoneGeneralById (@id INT)
	AS SELECT * 
	FROM DamageDoneGeneral
	WHERE Id = @id