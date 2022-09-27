CREATE PROCEDURE [dbo].[UpdateAppUser]
	@Id NVARCHAR (MAX),
	@Email NVARCHAR (MAX),
	@Password NVARCHAR (MAX)
AS 
	UPDATE AppUser
	SET Email = @Email,Password = @Password
	WHERE Id = @Id
RETURN 0
