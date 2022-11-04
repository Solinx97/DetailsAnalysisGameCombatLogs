CREATE PROCEDURE [dbo].[UpdatePersonalChatMessage]
	@Id INT,
	@Username NVARCHAR (MAX),
	@Message NVARCHAR (MAX),
	@Time TIME (7),
	@PersonalChatId INT
AS 
	UPDATE PersonalChatMessage
	SET Username = @Username,Message = @Message,Time = @Time,PersonalChatId = @PersonalChatId
	WHERE Id = @Id
RETURN 0
