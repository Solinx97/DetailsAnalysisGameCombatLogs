CREATE PROCEDURE [dbo].[UpdatePersonalChat]
	@Id INT,
	@InitiatorUsername NVARCHAR (MAX),
	@CompanionUsername NVARCHAR (MAX),
	@LastMessage NVARCHAR (MAX),
	@InitiatorId NVARCHAR (MAX),
	@CompanionId NVARCHAR (MAX)
AS
	UPDATE PersonalChat
	SET InitiatorUsername = @InitiatorUsername,CompanionUsername = @CompanionUsername,LastMessage = @LastMessage,InitiatorId = @InitiatorId,CompanionId = @CompanionId
	WHERE Id = @Id
RETURN 0
