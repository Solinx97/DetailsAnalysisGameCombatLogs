CREATE PROCEDURE [dbo].[InsertIntoPersonalChatMessage]
	@Message NVARCHAR (MAX),
	@Time TIME (7),
	@PersonalChatId INT,
	@OwnerId NVARCHAR (MAX)
AS
	DECLARE @OutputTbl TABLE (Id INT,Message NVARCHAR (MAX),Time TIME (7),PersonalChatId INT, wnerId NVARCHAR (MAX))
	INSERT INTO PersonalChatMessage
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Message,@Time,@PersonalChatId,@OwnerId)
	SELECT * FROM @OutputTbl
RETURN 0
