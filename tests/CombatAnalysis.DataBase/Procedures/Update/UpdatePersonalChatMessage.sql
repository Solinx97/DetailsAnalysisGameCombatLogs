CREATE PROCEDURE [dbo].[UpdatePersonalChatMessage]
	@Id INT,
	@Message NVARCHAR (MAX),
	@Time TIME (7),
	@PersonalChatId INT,
	@OwnerId NVARCHAR (MAX)
AS 
	UPDATE PersonalChatMessage
	SET Message = @Message,Time = @Time,PersonalChatId = @PersonalChatId,OwnerId = @OwnerId
	WHERE Id = @Id
RETURN 0
