CREATE PROCEDURE [dbo].[GetResourceRecoveryGeneralById]
	@id int
AS
	SELECT *
	FROM ResourceRecoveryGeneral
	WHERE Id = @id
RETURN 0
