CREATE PROCEDURE [dbo].[DeleteRefreshTokenById]
	@id int
AS
	DELETE
	FROM RefreshToken
	WHERE Id = @id
RETURN 0
