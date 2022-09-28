CREATE PROCEDURE [dbo].[GetGroupChatById]
	@id int
AS
	SELECT *
	FROM GroupChat
	WHERE Id = @id
RETURN 0
