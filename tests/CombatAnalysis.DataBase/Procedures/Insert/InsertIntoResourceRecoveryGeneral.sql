CREATE PROCEDURE [dbo].[InsertIntoResourceRecoveryGeneral]
	@Value INT,
	@ResourcePerSecond FLOAT (53),
	@SpellOrItem NVARCHAR (MAX),
	@CastNumber INT,
	@MinValue INT,
	@MaxValue INT,
	@AverageValue FLOAT (53),
	@CombatPlayerId INT
AS
	DECLARE @OutputTbl TABLE (Id INT,Value INT,ResourcePerSecond FLOAT (53),SpellOrItem NVARCHAR (MAX),CastNumber INT,MinValue INT,MaxValue INT,AverageValue FLOAT (53),CombatPlayerId INT)
	INSERT INTO ResourceRecoveryGeneral
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Value,@ResourcePerSecond,@SpellOrItem,@CastNumber,@MinValue,@MaxValue,@AverageValue,@CombatPlayerId)
	SELECT * FROM @OutputTbl
RETURN 0
