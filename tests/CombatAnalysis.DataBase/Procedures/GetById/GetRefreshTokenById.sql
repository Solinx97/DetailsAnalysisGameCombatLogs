CREATE PROCEDURE [dbo].[GetRefreshTokenById]
	@id int
AS
	SELECT *
	FROM RefreshToken
	WHERE Id = @id
RETURN 0
