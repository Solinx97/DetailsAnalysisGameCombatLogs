﻿CREATE PROCEDURE [dbo].[InsertIntoDamageTaken]
	@Value INT,
	@Time NVARCHAR (MAX),
	@FromEnemy NVARCHAR (MAX),
	@ToPlayer NVARCHAR (MAX),
	@SpellOrItem NVARCHAR (MAX),
	@Resist INT,
	@Absorb INT,
	@Blocked INT,
	@RealDamage INT,
	@Mitigated INT,
	@IsDodge BIT,
	@IsParry BIT,
	@IsMiss BIT,
	@IsResist BIT,
	@IsImmune BIT,
	@IsCrushing BIT,
	@CombatPlayerId INT
AS
	DECLARE @OutputTbl TABLE (Id INT,Value INT,Time NVARCHAR (MAX),FromEnemy NVARCHAR (MAX),ToPlayer NVARCHAR (MAX),SpellOrItem NVARCHAR (MAX),Resist INT,Absorb INT,Blocked INT,RealDamage INT,Mitigated INT, IsDodge BIT,IsParry BIT,IsMiss BIT,IsResist BIT,IsImmune BIT,IsCrushing BIT,CombatPlayerId INT)
	INSERT INTO DamageTaken
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Value,@Time,@FromEnemy,@ToPlayer,@SpellOrItem,@Resist,@Absorb,@Blocked,@RealDamage,@Mitigated,@IsDodge,@IsParry,@IsMiss,@IsResist,@IsImmune,@IsCrushing,@CombatPlayerId)
	SELECT * FROM @OutputTbl
RETURN 0