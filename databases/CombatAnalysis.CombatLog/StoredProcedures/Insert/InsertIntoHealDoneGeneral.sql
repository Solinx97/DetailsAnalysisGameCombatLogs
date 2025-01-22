CREATE PROCEDURE InsertIntoHealDoneGeneral (@Value INT,@HealPerSecond FLOAT (53),@SpellOrItem NVARCHAR (MAX),@DamageAbsorbed NVARCHAR (MAX),@CritNumber INT,@CastNumber INT,@MinValue INT,@MaxValue INT,@AverageValue FLOAT (53),@CombatPlayerId INT)
	AS
	DECLARE @OutputTbl TABLE (Id INT,Value INT,HealPerSecond FLOAT (53),SpellOrItem NVARCHAR (MAX),DamageAbsorbed NVARCHAR (MAX),CritNumber INT,CastNumber INT,MinValue INT,MaxValue INT,AverageValue FLOAT (53),CombatPlayerId INT)
	INSERT INTO HealDoneGeneral
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Value,@HealPerSecond,@SpellOrItem,@DamageAbsorbed,@CritNumber,@CastNumber,@MinValue,@MaxValue,@AverageValue,@CombatPlayerId)
	SELECT * FROM @OutputTbl