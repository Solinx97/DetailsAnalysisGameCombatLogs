CREATE PROCEDURE [dbo].[DeleteBannedUserById]
	@id int
AS
	DELETE
	FROM BannedUser
	WHERE Id = @id
RETURN 0
