CREATE PROCEDURE [dbo].[GetBannedUserById]
	@id int
AS
	SELECT *
	FROM BannedUser
	WHERE Id = @id
RETURN 0
