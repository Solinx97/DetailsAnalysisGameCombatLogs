CREATE PROCEDURE [dbo].[InsertIntoGroupChatMessage]
	@Username NVARCHAR (MAX),
	@Message NVARCHAR (MAX),
	@Time TIME (7),
	@GroupChatId INT
AS
	DECLARE @OutputTbl TABLE (Id INT,Username NVARCHAR (MAX),Message NVARCHAR (MAX),Time TIME (7),GroupChatId INT)
	INSERT INTO GroupChatMessage
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Username,@Message,@Time,@GroupChatId)
	SELECT * FROM @OutputTbl
RETURN 0
