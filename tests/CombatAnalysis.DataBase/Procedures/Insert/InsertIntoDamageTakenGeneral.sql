CREATE PROCEDURE [dbo].[InsertIntoDamageTakenGeneral]
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
	DECLARE @OutputTbl TABLE (Id INT,Value INT,DamageTakenPerSecond FLOAT (53),SpellOrItem NVARCHAR (MAX),CritNumber INT,MissNumber INT,CastNumber INT,MinValue INT,MaxValue INT,AverageValue FLOAT (53),CombatPlayerId INT)
	INSERT INTO DamageTakenGeneral
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Value,@DamageTakenPerSecond,@SpellOrItem,@CritNumber,@MissNumber,@CastNumber,@MinValue,@MaxValue,@AverageValue,@CombatPlayerId)
	SELECT * FROM @OutputTbl
RETURN 0
