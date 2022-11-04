CREATE PROCEDURE [dbo].[UpdateDamageTakenGeneral]
	@Id INT,
	@Value INT,
	@DamageTakenPerSecond FLOAT (53),
	@SpellOrItem NVARCHAR (MAX),
	@CritNumber INT,
	@MissNumber INT,
	@CastNumber INT,
	@MinValue INT,
	@MaxValue INT,
	@AverageValue FLOAT (53),
	@CombatPlayerId INT
AS
	UPDATE DamageTakenGeneral
	SET Value = @Value,DamageTakenPerSecond = @DamageTakenPerSecond,SpellOrItem = @SpellOrItem,CritNumber = @CritNumber,MissNumber = @MissNumber,CastNumber = @CastNumber,MinValue = @MinValue,MaxValue = @MaxValue,AverageValue = @AverageValue,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
RETURN 0
