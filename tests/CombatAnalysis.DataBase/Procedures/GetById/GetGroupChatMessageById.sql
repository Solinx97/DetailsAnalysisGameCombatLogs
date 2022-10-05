CREATE PROCEDURE [dbo].[GetGroupChatMessageById]
	@id int
AS
	SELECT *
	FROM GroupChatMessage
	WHERE Id = @id
RETURN 0
