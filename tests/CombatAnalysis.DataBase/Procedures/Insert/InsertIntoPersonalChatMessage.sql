CREATE PROCEDURE [dbo].[InsertIntoPersonalChatMessage]
	@Username NVARCHAR (MAX),
	@Message NVARCHAR (MAX),
	@Time TIME (7),
	@PersonalChatId INT
AS
	DECLARE @OutputTbl TABLE (Id INT,Username NVARCHAR (MAX),Message NVARCHAR (MAX),Time TIME (7),PersonalChatId INT)
	INSERT INTO PersonalChatMessage
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Username,@Message,@Time,@PersonalChatId)
	SELECT * FROM @OutputTbl
RETURN 0
