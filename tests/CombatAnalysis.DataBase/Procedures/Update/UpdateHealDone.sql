CREATE PROCEDURE [dbo].[UpdateHealDone]
	@Id INT,
	@ValueWithOverheal INT,
	@Time NVARCHAR (MAX),
	@Overheal INT,
	@Value INT,
	@FromPlayer NVARCHAR (MAX),
	@ToPlayer NVARCHAR (MAX),
	@SpellOrItem NVARCHAR (MAX),
	@IsCrit BIT,
	@IsFullOverheal BIT,
	@CombatPlayerId INT
AS
	UPDATE HealDone
	SET ValueWithOverheal = @ValueWithOverheal,Time = @Time,Overheal = @Overheal,Value = @Value,FromPlayer = @FromPlayer,ToPlayer = @ToPlayer,SpellOrItem = @SpellOrItem,IsCrit = @IsCrit,IsFullOverheal = @IsFullOverheal,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
RETURN 0
