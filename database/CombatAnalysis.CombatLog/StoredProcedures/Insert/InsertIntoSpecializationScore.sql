CREATE PROCEDURE InsertIntoSpecializationScore (@SpecId INT,@BossId INT,@Difficult INT,@Damage INT,@Heal INT,@Updated DATETIMEOFFSET (7))
	AS
	DECLARE @OutputTbl TABLE (Id INT,SpecId INT,BossId INT,Difficult INT,Damage INT,Heal INT,Updated DATETIMEOFFSET (7))
	INSERT INTO SpecializationScore
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@SpecId,@BossId,@Difficult,@Damage,@Heal,@Updated)
	SELECT * FROM @OutputTbl