﻿CREATE PROCEDURE [dbo].[UpdateDamageDone]
	@Id INT,
	@Value INT,
	@Time NVARCHAR (MAX),
	@FromPlayer NVARCHAR (MAX),
	@ToEnemy NVARCHAR (MAX),
	@SpellOrItem NVARCHAR (MAX),
	@IsPeriodicDamage BIT,
	@IsDodge BIT,
	@IsParry BIT,
	@IsMiss BIT,
	@IsResist BIT,
	@IsImmune BIT,
	@IsCrit BIT,
	@CombatPlayerId INT
AS
	UPDATE DamageDone
	SET Value = @Value,Time = @Time,FromPlayer = @FromPlayer,ToEnemy = @ToEnemy,SpellOrItem = @SpellOrItem,IsPeriodicDamage = @IsPeriodicDamage,IsDodge = @IsDodge,IsParry = @IsParry,IsMiss = @IsMiss,IsResist = @IsResist,IsImmune = @IsImmune,IsCrit = @IsCrit,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
RETURN 0
