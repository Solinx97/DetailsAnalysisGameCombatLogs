CREATE PROCEDURE [dbo].[UpdateCombatPlayer]
	@Id INT,
	@UserName NVARCHAR (MAX),
	@EnergyRecovery INT,
	@DamageDone INT,
	@HealDone INT,
	@DamageTaken INT,
	@UsedBuffs INT,
	@CombatId INT
AS
	UPDATE CombatPlayer
	SET UserName = @UserName,EnergyRecovery = @EnergyRecovery,DamageDone = @DamageDone,HealDone = @HealDone,DamageTaken = @DamageTaken,UsedBuffs = @UsedBuffs,CombatId = @CombatId
	WHERE Id = @Id
RETURN 0
