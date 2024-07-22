CREATE PROCEDURE UpdatePlayerDeath (@Id INT,@Date DATETIMEOFFSET (7),@CombatPlayerId INT)
	AS UPDATE PlayerDeath
	SET Date = @Date,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id