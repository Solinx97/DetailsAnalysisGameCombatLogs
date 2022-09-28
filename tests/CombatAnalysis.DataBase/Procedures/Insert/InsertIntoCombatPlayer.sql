CREATE PROCEDURE [dbo].[InsertIntoCombatPlayer]
	@UserName NVARCHAR (MAX),
	@EnergyRecovery INT,
	@DamageDone INT,
	@HealDone INT,
	@DamageTaken INT,
	@UsedBuffs INT,
	@CombatId INT
AS
	DECLARE @OutputTbl TABLE (Id INT,UserName NVARCHAR (MAX),EnergyRecovery INT,DamageDone INT,HealDone INT,DamageTaken INT,UsedBuffs INT,CombatId INT)
	INSERT INTO CombatPlayer
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@UserName,@EnergyRecovery,@DamageDone,@HealDone,@DamageTaken,@UsedBuffs,@CombatId)
	SELECT * FROM @OutputTbl
RETURN 0
