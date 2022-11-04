CREATE PROCEDURE [dbo].[UpdateCombatLog]
	@Id INT,
	@Name NVARCHAR (MAX),
	@Date DATETIMEOFFSET (7),
	@IsReady BIT
AS 
	UPDATE CombatLog
	SET Name = @Name,Date = @Date,IsReady = @IsReady
	WHERE Id = @Id
RETURN 0
