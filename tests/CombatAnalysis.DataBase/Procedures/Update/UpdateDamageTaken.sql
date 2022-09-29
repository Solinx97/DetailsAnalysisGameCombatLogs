CREATE PROCEDURE [dbo].[UpdateDamageTaken]
	@Id INT,
	@Value INT,
	@Time NVARCHAR (MAX),
	@FromEnemy NVARCHAR (MAX),
	@ToPlayer NVARCHAR (MAX),
	@SpellOrItem NVARCHAR (MAX),
	@Resist INT,
	@Absorb INT,
	@Blocked INT,
	@RealDamage INT,
	@Mitigated INT,
	@IsDodge BIT,
	@IsParry BIT,
	@IsMiss BIT,
	@IsResist BIT,
	@IsImmune BIT,
	@IsCrushing BIT,
	@CombatPlayerId INT
AS
	UPDATE DamageTaken
	SET Value = @Value,Time = @Time,FromEnemy = @FromEnemy,ToPlayer = @ToPlayer,SpellOrItem = @SpellOrItem,Resist = @Resist, Absorb = @Absorb, Blocked = @Blocked, RealDamage = @RealDamage, Mitigated = @Mitigated, IsDodge = @IsDodge,IsParry = @IsParry,IsMiss = @IsMiss,IsResist = @IsResist,IsImmune = @IsImmune,IsCrushing = @IsCrushing,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
RETURN 0
