CREATE PROCEDURE [dbo].[UpdatePersonalChat]
	@Id INT,
	@LastMessage NVARCHAR (MAX),
	@InitiatorId NVARCHAR (MAX),
	@CompanionId NVARCHAR (MAX)
AS
	UPDATE PersonalChat
	SET LastMessage = @LastMessage,InitiatorId = @InitiatorId,CompanionId = @CompanionId
	WHERE Id = @Id
RETURN 0
