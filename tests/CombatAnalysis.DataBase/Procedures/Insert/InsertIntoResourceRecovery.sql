CREATE PROCEDURE [dbo].[InsertIntoResourceRecovery]
	@Value INT,
	@Time NVARCHAR (MAX),
	@SpellOrItem NVARCHAR (MAX),
	@CombatPlayerId INT
AS
	DECLARE @OutputTbl TABLE (Id INT,Value INT,Time NVARCHAR (MAX),SpellOrItem NVARCHAR (MAX),CombatPlayerId INT)
	INSERT INTO ResourceRecovery
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Value,@Time,@SpellOrItem,@CombatPlayerId)
	SELECT * FROM @OutputTbl
RETURN 0
