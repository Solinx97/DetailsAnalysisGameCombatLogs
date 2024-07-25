CREATE PROCEDURE [dbo].[InsertIntoGroupChatMessage]
	@Message NVARCHAR (MAX),
	@Time TIME (7),
	@GroupChatId INT,
	@OwnerId NVARCHAR (MAX)
AS
	DECLARE @OutputTbl TABLE (Id INT,Message NVARCHAR (MAX),Time TIME (7),GroupChatId INT, OwnerId NVARCHAR (MAX))
	INSERT INTO GroupChatMessage
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Message,@Time,@GroupChatId,@OwnerId)
	SELECT * FROM @OutputTbl
RETURN 0
