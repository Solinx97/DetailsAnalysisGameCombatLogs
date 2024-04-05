CREATE PROCEDURE GetResourceRecoveryGeneralById (@id INT)
	AS SELECT * 
	FROM ResourceRecoveryGeneral
	WHERE Id = @id