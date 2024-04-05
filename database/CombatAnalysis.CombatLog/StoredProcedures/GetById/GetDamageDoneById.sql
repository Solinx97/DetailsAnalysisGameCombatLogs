CREATE PROCEDURE GetDamageDoneById (@id INT)
	AS SELECT * 
	FROM DamageDone
	WHERE Id = @id