CREATE PROCEDURE [dbo].[UpdateDamageDoneGeneral]
	@Id INT,
	@Value INT,
	@DamagePerSecond FLOAT (53),
	@SpellOrItem NVARCHAR (MAX),
	@CritNumber INT,
	@MissNumber INT,
	@CastNumber INT,
	@MinValue INT,
	@MaxValue INT,
	@AverageValue FLOAT (53),
	@CombatPlayerId INT
AS
	UPDATE DamageDoneGeneral
	SET Value = @Value,DamagePerSecond = @DamagePerSecond,SpellOrItem = @SpellOrItem,CritNumber = @CritNumber,MissNumber = @MissNumber,CastNumber = @CastNumber,MinValue = @MinValue,MaxValue = @MaxValue,AverageValue = @AverageValue,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
RETURN 0
