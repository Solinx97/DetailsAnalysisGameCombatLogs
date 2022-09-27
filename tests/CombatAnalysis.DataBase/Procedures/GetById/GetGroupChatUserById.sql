CREATE PROCEDURE [dbo].[GetGroupChatUserById]
	@id int
AS
	SELECT *
	FROM GroupChatUser
	WHERE Id = @id
RETURN 0
