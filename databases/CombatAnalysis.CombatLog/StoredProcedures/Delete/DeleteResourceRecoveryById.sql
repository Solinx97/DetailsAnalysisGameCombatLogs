CREATE PROCEDURE DeleteResourceRecoveryById (@id INT)
	AS DELETE FROM ResourceRecovery
	WHERE Id = @id