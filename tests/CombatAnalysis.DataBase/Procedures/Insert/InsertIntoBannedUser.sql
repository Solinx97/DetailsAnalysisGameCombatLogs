CREATE PROCEDURE [dbo].[InsertIntoBannedUser]
	@WhomBannedId NVARCHAR (MAX),
	@BannedUserId NVARCHAR (MAX)
AS
	DECLARE @OutputTbl TABLE (Id INT,WhomBannedId NVARCHAR (MAX),BannedUserId NVARCHAR (MAX))
	INSERT INTO BannedUser
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@WhomBannedId,@BannedUserId)
	SELECT * FROM @OutputTbl
RETURN 0
