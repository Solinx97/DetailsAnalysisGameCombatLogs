CREATE PROCEDURE [dbo].[InsertIntoHealDone]
	@ValueWithOverheal INT,
	@Time NVARCHAR (MAX),
	@Overheal INT,
	@Value INT,
	@FromPlayer NVARCHAR (MAX),
	@ToPlayer NVARCHAR (MAX),
	@SpellOrItem NVARCHAR (MAX),
	@IsCrit BIT,
	@IsFullOverheal BIT,
	@CombatPlayerId INT
AS
	DECLARE @OutputTbl TABLE (Id INT,ValueWithOverheal INT,Time NVARCHAR (MAX),Overheal INT,Value INT,FromPlayer NVARCHAR (MAX),ToPlayer NVARCHAR (MAX),SpellOrItem NVARCHAR (MAX),IsCrit BIT,IsFullOverheal BIT,CombatPlayerId INT)
	INSERT INTO HealDone
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@ValueWithOverheal,@Time,@Overheal,@Value,@FromPlayer,@ToPlayer,@SpellOrItem,@IsCrit,@IsFullOverheal,@CombatPlayerId)
	SELECT * FROM @OutputTbl
RETURN 0
