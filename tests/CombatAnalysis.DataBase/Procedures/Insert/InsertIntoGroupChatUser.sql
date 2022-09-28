CREATE PROCEDURE [dbo].[InsertIntoGroupChatUser]
	@UserId NVARCHAR (MAX),
	@GroupChatId INT
AS
	DECLARE @OutputTbl TABLE (Id INT,UserId NVARCHAR (MAX),GroupChatId INT)
	INSERT INTO GroupChatUser
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@UserId,@GroupChatId)
	SELECT * FROM @OutputTbl
RETURN 0
