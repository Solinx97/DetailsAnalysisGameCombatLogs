CREATE PROCEDURE UpdateResourceRecovery (@Id INT,@Value INT,@Time NVARCHAR (MAX),@SpellOrItem NVARCHAR (MAX),@CombatPlayerId INT)
	AS UPDATE ResourceRecovery
	SET Value = @Value,Time = @Time,SpellOrItem = @SpellOrItem,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id