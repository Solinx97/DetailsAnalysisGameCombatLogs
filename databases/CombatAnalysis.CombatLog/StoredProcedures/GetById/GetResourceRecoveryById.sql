CREATE PROCEDURE GetResourceRecoveryById (@id INT)
	AS SELECT * 
	FROM ResourceRecovery
	WHERE Id = @id