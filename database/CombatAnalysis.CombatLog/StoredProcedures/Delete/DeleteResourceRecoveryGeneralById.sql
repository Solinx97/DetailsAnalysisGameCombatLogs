CREATE PROCEDURE DeleteResourceRecoveryGeneralById (@id INT)
	AS DELETE FROM ResourceRecoveryGeneral
	WHERE Id = @id