CREATE PROCEDURE [dbo].[GetResourceRecoveryById]
	@id int
AS
	SELECT *
	FROM ResourceRecovery
	WHERE Id = @id
RETURN 0
