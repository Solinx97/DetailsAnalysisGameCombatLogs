CREATE PROCEDURE [dbo].[UpdateHealDoneGeneral]
	@Id INT,
	@Value INT,
	@HealPerSecond FLOAT (53),
	@SpellOrItem NVARCHAR (MAX),
	@CritNumber INT,
	@CastNumber INT,
	@MinValue INT,
	@MaxValue INT,
	@AverageValue FLOAT (53),
	@CombatPlayerId INT
AS 
	UPDATE HealDoneGeneral
	SET Value = @Value,HealPerSecond = @HealPerSecond,SpellOrItem = @SpellOrItem,CritNumber = @CritNumber,CastNumber = @CastNumber,MinValue = @MinValue,MaxValue = @MaxValue,AverageValue = @AverageValue,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
RETURN 0
