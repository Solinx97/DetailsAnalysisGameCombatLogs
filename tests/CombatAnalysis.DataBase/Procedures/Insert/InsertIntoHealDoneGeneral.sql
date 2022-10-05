CREATE PROCEDURE [dbo].[InsertIntoHealDoneGeneral]
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
	DECLARE @OutputTbl TABLE (Id INT,Value INT,HealPerSecond FLOAT (53),SpellOrItem NVARCHAR (MAX),CritNumber INT,CastNumber INT,MinValue INT,MaxValue INT,AverageValue FLOAT (53),CombatPlayerId INT)
	INSERT INTO HealDoneGeneral
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Value,@HealPerSecond,@SpellOrItem,@CritNumber,@CastNumber,@MinValue,@MaxValue,@AverageValue,@CombatPlayerId)
	SELECT * FROM @OutputTbl
RETURN 0
