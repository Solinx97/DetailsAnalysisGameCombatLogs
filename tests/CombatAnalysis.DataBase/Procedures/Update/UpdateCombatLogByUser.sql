CREATE PROCEDURE [dbo].[UpdateCombatLogByUser]
	@Id INT,
	@CombatLogId INT,
	@UserId NVARCHAR (MAX),
	@PersonalLogType INT
AS
	UPDATE CombatLogByUser
	SET CombatLogId = @CombatLogId,UserId = @UserId,PersonalLogType = @PersonalLogType
	WHERE Id = @Id
RETURN 0
