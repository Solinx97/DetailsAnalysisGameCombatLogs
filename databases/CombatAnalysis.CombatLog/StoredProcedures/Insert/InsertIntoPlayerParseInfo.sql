CREATE PROCEDURE InsertIntoPlayerParseInfo (@SpecId INT,@ClassId INT,@BossId INT,@Difficult INT,@DamageEfficiency INT,@HealEfficiency INT,@CombatPlayerId INT)
	AS
	DECLARE @OutputTbl TABLE (Id INT,SpecId INT,ClassId INT,BossId INT,Difficult INT,DamageEfficiency INT,HealEfficiency INT,CombatPlayerId INT)
	INSERT INTO PlayerParseInfo
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@SpecId,@ClassId,@BossId,@Difficult,@DamageEfficiency,@HealEfficiency,@CombatPlayerId)
	SELECT * FROM @OutputTbl