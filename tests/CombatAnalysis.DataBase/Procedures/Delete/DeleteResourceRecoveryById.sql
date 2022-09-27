CREATE PROCEDURE [dbo].[DeleteResourceRecoveryById]
	@id int
AS
	DELETE
	FROM ResourceRecovery
	WHERE Id = @id
RETURN 0
