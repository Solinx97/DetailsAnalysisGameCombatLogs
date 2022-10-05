CREATE PROCEDURE [dbo].[DeleteResourceRecoveryGeneralById]
	@id int
AS
	DELETE
	FROM ResourceRecoveryGeneral
	WHERE Id = @id
RETURN 0
