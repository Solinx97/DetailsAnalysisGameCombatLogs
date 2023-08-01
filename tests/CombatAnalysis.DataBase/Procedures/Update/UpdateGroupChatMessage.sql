CREATE PROCEDURE [dbo].[UpdateGroupChatMessage]
	@Id INT,
	@Message NVARCHAR (MAX),
	@Time TIME (7),
	@GroupChatId INT,
	@OwnerId NVARCHAR (MAX)
AS
	UPDATE GroupChatMessage
	SET Message = @Message,Time = @Time,GroupChatId = @GroupChatId,OwnerId = @OwnerId
	WHERE Id = @Id
RETURN 0
