CREATE PROCEDURE [dbo].[UpdateResourceRecoveryGeneral]
	@Id INT,
	@Value INT,
	@ResourcePerSecond FLOAT (53),
	@SpellOrItem NVARCHAR (MAX),
	@CastNumber INT,
	@MinValue INT,
	@MaxValue INT,
	@AverageValue FLOAT (53),
	@CombatPlayerId INT
AS 
	UPDATE ResourceRecoveryGeneral
	SET Value = @Value,ResourcePerSecond = @ResourcePerSecond,SpellOrItem = @SpellOrItem,CastNumber = @CastNumber,MinValue = @MinValue,MaxValue = @MaxValue,AverageValue = @AverageValue,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
RETURN 0
