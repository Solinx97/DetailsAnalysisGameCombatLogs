CREATE PROCEDURE InsertIntoCombatPlayer (@Username NVARCHAR (MAX),@PlayerId NVARCHAR (MAX),@AverageItemLevel FLOAT (53),@EnergyRecovery INT,@DamageDone INT,@HealDone INT,@DamageTaken INT,@UsedBuffs INT,@CombatId INT)
	AS
	DECLARE @OutputTbl TABLE (Id INT,Username NVARCHAR (MAX),PlayerId NVARCHAR (MAX),AverageItemLevel FLOAT (53),EnergyRecovery INT,DamageDone INT,HealDone INT,DamageTaken INT,UsedBuffs INT,CombatId INT)
	INSERT INTO CombatPlayer
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Username,@PlayerId,@AverageItemLevel,@EnergyRecovery,@DamageDone,@HealDone,@DamageTaken,@UsedBuffs,@CombatId)
	SELECT * FROM @OutputTbl