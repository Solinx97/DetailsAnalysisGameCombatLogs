CREATE PROCEDURE [dbo].[InsertIntoPersonalChat]
	@LastMessage NVARCHAR (MAX),
	@InitiatorId NVARCHAR (MAX),
	@CompanionId NVARCHAR (MAX)
AS
	DECLARE @OutputTbl TABLE (Id INT,LastMessage NVARCHAR (MAX),InitiatorId NVARCHAR (MAX),CompanionId NVARCHAR (MAX))
	INSERT INTO PersonalChat
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@LastMessage,@InitiatorId,@CompanionId)
	SELECT * FROM @OutputTbl
RETURN 0
