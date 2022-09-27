CREATE PROCEDURE [dbo].[UpdateRefreshToken]
	@Id NVARCHAR (MAX),
	@UserId NVARCHAR (MAX),
	@Token NVARCHAR (MAX),
	@Expires DATETIMEOFFSET (7)
AS
	UPDATE RefreshToken
	SET UserId = @UserId,Token = @Token,Expires = @Expires
	WHERE Id = @Id
RETURN 0
