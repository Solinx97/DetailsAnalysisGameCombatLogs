CREATE PROCEDURE [dbo].[InsertIntoGroupChat]
	@Name NVARCHAR (MAX),
	@ShortName NVARCHAR (MAX),
	@LastMessage NVARCHAR (MAX),
	@MemberNumber INT,
	@ChatPolicyType INT,
	@OwnerId NVARCHAR (MAX)
AS
	DECLARE @OutputTbl TABLE (Id INT,Name NVARCHAR (MAX),ShortName NVARCHAR (MAX),LastMessage NVARCHAR (MAX),MemberNumber INT,ChatPolicyType INT,OwnerId NVARCHAR (MAX))
	INSERT INTO GroupChat
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Name,@ShortName,@LastMessage,@MemberNumber,@ChatPolicyType,@OwnerId)
	SELECT * FROM @OutputTbl
RETURN 0
