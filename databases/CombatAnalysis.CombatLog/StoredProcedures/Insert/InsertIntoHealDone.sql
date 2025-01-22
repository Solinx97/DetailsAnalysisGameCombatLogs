CREATE PROCEDURE InsertIntoHealDone (@ValueWithOverheal INT,@Time NVARCHAR (MAX),@Overheal INT,@Value INT,@FromPlayer NVARCHAR (MAX),@ToPlayer NVARCHAR (MAX),@SpellOrItem NVARCHAR (MAX),@DamageAbsorbed NVARCHAR (MAX),@IsCrit BIT,@IsFullOverheal BIT,@IsAbsorbed BIT,@CombatPlayerId INT)
	AS
	DECLARE @OutputTbl TABLE (Id INT,ValueWithOverheal INT,Time NVARCHAR (MAX),Overheal INT,Value INT,FromPlayer NVARCHAR (MAX),ToPlayer NVARCHAR (MAX),SpellOrItem NVARCHAR (MAX),DamageAbsorbed NVARCHAR (MAX),IsCrit BIT,IsFullOverheal BIT,IsAbsorbed BIT,CombatPlayerId INT)
	INSERT INTO HealDone
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@ValueWithOverheal,@Time,@Overheal,@Value,@FromPlayer,@ToPlayer,@SpellOrItem,@DamageAbsorbed,@IsCrit,@IsFullOverheal,@IsAbsorbed,@CombatPlayerId)
	SELECT * FROM @OutputTbl