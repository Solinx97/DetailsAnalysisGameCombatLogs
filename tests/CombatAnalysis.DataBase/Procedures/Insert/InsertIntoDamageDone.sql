CREATE PROCEDURE [dbo].[InsertIntoDamageDone]
	@Value INT,
	@Time NVARCHAR (MAX),
	@FromPlayer NVARCHAR (MAX),
	@ToEnemy NVARCHAR (MAX),
	@SpellOrItem NVARCHAR (MAX),
	@IsPeriodicDamage BIT,
	@IsDodge BIT,
	@IsParry BIT,
	@IsMiss BIT,
	@IsResist BIT,
	@IsImmune BIT,
	@IsCrit BIT,
	@CombatPlayerId INT
AS
	DECLARE @OutputTbl TABLE (Id INT,Value INT,Time NVARCHAR (MAX),FromPlayer NVARCHAR (MAX),ToEnemy NVARCHAR (MAX),SpellOrItem NVARCHAR (MAX),IsPeriodicDamage BIT,IsDodge BIT,IsParry BIT,IsMiss BIT,IsResist BIT,IsImmune BIT,IsCrit BIT,CombatPlayerId INT)
	INSERT INTO DamageDone
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Value,@Time,@FromPlayer,@ToEnemy,@SpellOrItem,@IsPeriodicDamage,@IsDodge,@IsParry,@IsMiss,@IsResist,@IsImmune,@IsCrit,@CombatPlayerId)
	SELECT * FROM @OutputTbl
RETURN 0
