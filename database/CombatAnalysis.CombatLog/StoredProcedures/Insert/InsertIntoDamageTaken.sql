CREATE PROCEDURE InsertIntoDamageTaken (@Value INT,@ActualValue INT,@Time TIME (7),@FromEnemy NVARCHAR (MAX),@ToPlayer NVARCHAR (MAX),@SpellOrItem NVARCHAR (MAX),@IsPeriodicDamage BIT,@Resisted INT,@Absorbed INT,@Blocked INT,@RealDamage INT,@Mitigated INT,@IsDodge BIT,@IsParry BIT,@IsMiss BIT,@IsResist BIT,@IsImmune BIT,@IsAbsorb BIT,@IsCrushing BIT,@CombatPlayerId INT)
	AS
	DECLARE @OutputTbl TABLE (Id INT,Value INT,ActualValue INT,Time TIME (7),FromEnemy NVARCHAR (MAX),ToPlayer NVARCHAR (MAX),SpellOrItem NVARCHAR (MAX),IsPeriodicDamage BIT,Resisted INT,Absorbed INT,Blocked INT,RealDamage INT,Mitigated INT,IsDodge BIT,IsParry BIT,IsMiss BIT,IsResist BIT,IsImmune BIT,IsAbsorb BIT,IsCrushing BIT,CombatPlayerId INT)
	INSERT INTO DamageTaken
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Value,@ActualValue,@Time,@FromEnemy,@ToPlayer,@SpellOrItem,@IsPeriodicDamage,@Resisted,@Absorbed,@Blocked,@RealDamage,@Mitigated,@IsDodge,@IsParry,@IsMiss,@IsResist,@IsImmune,@IsAbsorb,@IsCrushing,@CombatPlayerId)
	SELECT * FROM @OutputTbl