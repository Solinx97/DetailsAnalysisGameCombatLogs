CREATE PROCEDURE [dbo].[UpdateGroupChatMessage]
	@Id INT,
	@Username NVARCHAR (MAX),
	@Message NVARCHAR (MAX),
	@Time TIME (7),
	@GroupChatId INT
AS
	UPDATE GroupChatMessage
	SET Username = @Username,Message = @Message,Time = @Time,GroupChatId = @GroupChatId
	WHERE Id = @Id
RETURN 0
