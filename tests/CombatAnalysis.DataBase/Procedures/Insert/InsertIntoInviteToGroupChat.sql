CREATE PROCEDURE [dbo].[InsertIntoInviteToGroupChat]
	@UserId NVARCHAR (MAX),
	@Response INT,
	@GroupChatId INT
AS
	DECLARE @OutputTbl TABLE (Id INT,UserId NVARCHAR (MAX),Response INT,GroupChatId INT)
	INSERT INTO InviteToGroupChat
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@UserId,@Response,@GroupChatId)
	SELECT * FROM @OutputTbl
RETURN 0
