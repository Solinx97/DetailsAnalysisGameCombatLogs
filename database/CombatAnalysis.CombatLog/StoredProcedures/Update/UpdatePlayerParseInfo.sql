CREATE PROCEDURE UpdatePlayerParseInfo (@Id INT,@SpecId INT,@ClassId INT,@BossId INT,@Difficult INT,@DamageEfficiency INT,@HealEfficiency INT,@CombatPlayerId INT)
	AS UPDATE PlayerParseInfo
	SET SpecId = @SpecId,ClassId = @ClassId,BossId = @BossId,Difficult = @Difficult,DamageEfficiency = @DamageEfficiency,HealEfficiency = @HealEfficiency,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id