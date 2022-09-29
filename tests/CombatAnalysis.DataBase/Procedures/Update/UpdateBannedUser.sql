CREATE PROCEDURE [dbo].[UpdateBannedUser]
	@Id INT,
	@WhomBannedId NVARCHAR (MAX),
	@BannedUserId NVARCHAR (MAX)
AS 
	UPDATE BannedUser
	SET WhomBannedId = @WhomBannedId,BannedUserId = @BannedUserId
	WHERE Id = @Id
RETURN 0
