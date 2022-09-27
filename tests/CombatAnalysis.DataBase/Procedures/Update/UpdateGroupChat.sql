CREATE PROCEDURE [dbo].[UpdateGroupChat]
	@Id INT,
	@Name NVARCHAR (MAX),
	@ShortName NVARCHAR (MAX),
	@LastMessage NVARCHAR (MAX),
	@MemberNumber INT,
	@ChatPolicyType INT,
	@OwnerId NVARCHAR (MAX)
AS
	UPDATE GroupChat
	SET Name = @Name,ShortName = @ShortName,LastMessage = @LastMessage,MemberNumber = @MemberNumber,ChatPolicyType = @ChatPolicyType,OwnerId = @OwnerId
	WHERE Id = @Id
RETURN 0
