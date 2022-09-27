CREATE PROCEDURE [dbo].[InsertIntoPersonalChat]
	@InitiatorUsername NVARCHAR (MAX),
	@CompanionUsername NVARCHAR (MAX),
	@LastMessage NVARCHAR (MAX),
	@InitiatorId NVARCHAR (MAX),
	@CompanionId NVARCHAR (MAX)
AS
	DECLARE @OutputTbl TABLE (Id INT,InitiatorUsername NVARCHAR (MAX),CompanionUsername NVARCHAR (MAX),LastMessage NVARCHAR (MAX),InitiatorId NVARCHAR (MAX),CompanionId NVARCHAR (MAX))
	INSERT INTO PersonalChat
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@InitiatorUsername,@CompanionUsername,@LastMessage,@InitiatorId,@CompanionId)
	SELECT * FROM @OutputTbl
RETURN 0
