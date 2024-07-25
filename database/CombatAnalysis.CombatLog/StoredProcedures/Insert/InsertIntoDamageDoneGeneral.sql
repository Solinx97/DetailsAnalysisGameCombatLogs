CREATE PROCEDURE InsertIntoDamageDoneGeneral (@Value INT,@DamagePerSecond FLOAT (53),@SpellOrItem NVARCHAR (MAX),@CritNumber INT,@MissNumber INT,@CastNumber INT,@MinValue INT,@MaxValue INT,@AverageValue FLOAT (53),@IsPet BIT,@CombatPlayerId INT)
	AS
	DECLARE @OutputTbl TABLE (Id INT,Value INT,DamagePerSecond FLOAT (53),SpellOrItem NVARCHAR (MAX),CritNumber INT,MissNumber INT,CastNumber INT,MinValue INT,MaxValue INT,AverageValue FLOAT (53),IsPet BIT,CombatPlayerId INT)
	INSERT INTO DamageDoneGeneral
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Value,@DamagePerSecond,@SpellOrItem,@CritNumber,@MissNumber,@CastNumber,@MinValue,@MaxValue,@AverageValue,@IsPet,@CombatPlayerId)
	SELECT * FROM @OutputTbl