CREATE PROCEDURE UpdateCombatPlayer (@Id INT,@Username NVARCHAR (MAX),@PlayerId NVARCHAR (MAX),@AverageItemLevel FLOAT (53),@EnergyRecovery INT,@DamageDone INT,@HealDone INT,@DamageTaken INT,@UsedBuffs INT,@CombatId INT)
	AS UPDATE CombatPlayer
	SET Username = @Username,PlayerId = @PlayerId,AverageItemLevel = @AverageItemLevel,EnergyRecovery = @EnergyRecovery,DamageDone = @DamageDone,HealDone = @HealDone,DamageTaken = @DamageTaken,UsedBuffs = @UsedBuffs,CombatId = @CombatId
	WHERE Id = @Id