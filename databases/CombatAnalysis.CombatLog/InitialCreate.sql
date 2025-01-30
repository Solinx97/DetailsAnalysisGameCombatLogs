IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Combat] (
    [Id] int NOT NULL IDENTITY,
    [LocallyNumber] int NOT NULL,
    [DungeonName] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Difficulty] int NOT NULL,
    [DamageDone] int NOT NULL,
    [HealDone] int NOT NULL,
    [DamageTaken] int NOT NULL,
    [EnergyRecovery] int NOT NULL,
    [IsWin] bit NOT NULL,
    [StartDate] datetimeoffset NOT NULL,
    [FinishDate] datetimeoffset NOT NULL,
    [IsReady] bit NOT NULL,
    [CombatLogId] int NOT NULL,
    CONSTRAINT [PK_Combat] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CombatAura] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Creator] nvarchar(max) NOT NULL,
    [Target] nvarchar(max) NOT NULL,
    [AuraCreatorType] int NOT NULL,
    [AuraType] int NOT NULL,
    [StartTime] time NOT NULL,
    [FinishTime] time NOT NULL,
    [Stacks] int NOT NULL,
    [CombatId] int NOT NULL,
    CONSTRAINT [PK_CombatAura] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CombatLog] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Date] datetimeoffset NOT NULL,
    [LogType] int NOT NULL,
    [NumberReadyCombats] int NOT NULL,
    [CombatsInQueue] int NOT NULL,
    [IsReady] bit NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_CombatLog] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CombatPlayer] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [PlayerId] nvarchar(max) NOT NULL,
    [AverageItemLevel] float NOT NULL,
    [ResourcesRecovery] int NOT NULL,
    [DamageDone] int NOT NULL,
    [HealDone] int NOT NULL,
    [DamageTaken] int NOT NULL,
    [CombatId] int NOT NULL,
    CONSTRAINT [PK_CombatPlayer] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [CombatPlayerPosition] (
    [Id] int NOT NULL IDENTITY,
    [PositionX] float NOT NULL,
    [PositionY] float NOT NULL,
    [Time] time NOT NULL,
    [CombatPlayerId] int NOT NULL,
    [CombatId] int NOT NULL,
    CONSTRAINT [PK_CombatPlayerPosition] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [DamageDone] (
    [Id] int NOT NULL IDENTITY,
    [Spell] nvarchar(max) NOT NULL,
    [Value] int NOT NULL,
    [Time] time NOT NULL,
    [Creator] nvarchar(max) NOT NULL,
    [Target] nvarchar(max) NOT NULL,
    [DamageType] int NOT NULL,
    [IsPeriodicDamage] bit NOT NULL,
    [IsPet] bit NOT NULL,
    [CombatPlayerId] int NOT NULL,
    CONSTRAINT [PK_DamageDone] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [DamageDoneGeneral] (
    [Id] int NOT NULL IDENTITY,
    [Value] int NOT NULL,
    [DamagePerSecond] float NOT NULL,
    [Spell] nvarchar(max) NOT NULL,
    [CritNumber] int NOT NULL,
    [MissNumber] int NOT NULL,
    [CastNumber] int NOT NULL,
    [MinValue] int NOT NULL,
    [MaxValue] int NOT NULL,
    [AverageValue] float NOT NULL,
    [IsPet] bit NOT NULL,
    [CombatPlayerId] int NOT NULL,
    CONSTRAINT [PK_DamageDoneGeneral] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [DamageTaken] (
    [Id] int NOT NULL IDENTITY,
    [Spell] nvarchar(max) NOT NULL,
    [Value] int NOT NULL,
    [Time] time NOT NULL,
    [Creator] nvarchar(max) NOT NULL,
    [Target] nvarchar(max) NOT NULL,
    [DamageTakenType] int NOT NULL,
    [ActualValue] int NOT NULL,
    [IsPeriodicDamage] bit NOT NULL,
    [Resisted] int NOT NULL,
    [Absorbed] int NOT NULL,
    [Blocked] int NOT NULL,
    [RealDamage] int NOT NULL,
    [Mitigated] int NOT NULL,
    [CombatPlayerId] int NOT NULL,
    CONSTRAINT [PK_DamageTaken] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [DamageTakenGeneral] (
    [Id] int NOT NULL IDENTITY,
    [Spell] nvarchar(max) NOT NULL,
    [Value] int NOT NULL,
    [ActualValue] int NOT NULL,
    [DamageTakenPerSecond] float NOT NULL,
    [CritNumber] int NOT NULL,
    [MissNumber] int NOT NULL,
    [CastNumber] int NOT NULL,
    [MinValue] int NOT NULL,
    [MaxValue] int NOT NULL,
    [AverageValue] float NOT NULL,
    [CombatPlayerId] int NOT NULL,
    CONSTRAINT [PK_DamageTakenGeneral] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [HealDone] (
    [Id] int NOT NULL IDENTITY,
    [Spell] nvarchar(max) NOT NULL,
    [Value] int NOT NULL,
    [Time] time NOT NULL,
    [Creator] nvarchar(max) NOT NULL,
    [Target] nvarchar(max) NOT NULL,
    [Overheal] int NOT NULL,
    [IsCrit] bit NOT NULL,
    [IsAbsorbed] bit NOT NULL,
    [CombatPlayerId] int NOT NULL,
    CONSTRAINT [PK_HealDone] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [HealDoneGeneral] (
    [Id] int NOT NULL IDENTITY,
    [Spell] nvarchar(max) NOT NULL,
    [Value] int NOT NULL,
    [HealPerSecond] float NOT NULL,
    [CritNumber] int NOT NULL,
    [CastNumber] int NOT NULL,
    [MinValue] int NOT NULL,
    [MaxValue] int NOT NULL,
    [AverageValue] float NOT NULL,
    [CombatPlayerId] int NOT NULL,
    CONSTRAINT [PK_HealDoneGeneral] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PlayerDeath] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [LastHitSpellOrItem] nvarchar(max) NOT NULL,
    [LastHitValue] int NOT NULL,
    [Time] time NOT NULL,
    [CombatPlayerId] int NOT NULL,
    CONSTRAINT [PK_PlayerDeath] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PlayerParseInfo] (
    [Id] int NOT NULL IDENTITY,
    [SpecId] int NOT NULL,
    [ClassId] int NOT NULL,
    [BossId] int NOT NULL,
    [Difficult] int NOT NULL,
    [DamageEfficiency] int NOT NULL,
    [HealEfficiency] int NOT NULL,
    [CombatPlayerId] int NOT NULL,
    CONSTRAINT [PK_PlayerParseInfo] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ResourceRecovery] (
    [Id] int NOT NULL IDENTITY,
    [Spell] nvarchar(max) NOT NULL,
    [Value] int NOT NULL,
    [Time] time NOT NULL,
    [Creator] nvarchar(max) NOT NULL,
    [Target] nvarchar(max) NOT NULL,
    [CombatPlayerId] int NOT NULL,
    CONSTRAINT [PK_ResourceRecovery] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ResourceRecoveryGeneral] (
    [Id] int NOT NULL IDENTITY,
    [Spell] nvarchar(max) NOT NULL,
    [Value] int NOT NULL,
    [ResourcePerSecond] float NOT NULL,
    [CastNumber] int NOT NULL,
    [MinValue] int NOT NULL,
    [MaxValue] int NOT NULL,
    [AverageValue] float NOT NULL,
    [CombatPlayerId] int NOT NULL,
    CONSTRAINT [PK_ResourceRecoveryGeneral] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [SpecializationScore] (
    [Id] int NOT NULL IDENTITY,
    [SpecId] int NOT NULL,
    [BossId] int NOT NULL,
    [Difficult] int NOT NULL,
    [Damage] int NOT NULL,
    [Heal] int NOT NULL,
    [Updated] datetimeoffset NOT NULL,
    CONSTRAINT [PK_SpecializationScore] PRIMARY KEY ([Id])
);
GO

CREATE PROCEDURE GetAllCombatLog
AS
BEGIN
	SELECT * 
	FROM CombatLog
END
GO

CREATE PROCEDURE GetCombatLogById (@id INT)
AS
BEGIN
	SELECT * 
	FROM CombatLog
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoCombatLog (@Name NVARCHAR (MAX),@Date DATETIMEOFFSET (7),@LogType INT,@NumberReadyCombats INT,@CombatsInQueue INT,@IsReady BIT,@AppUserId NVARCHAR (MAX))
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Name NVARCHAR (MAX),Date DATETIMEOFFSET (7),LogType INT,NumberReadyCombats INT,CombatsInQueue INT,IsReady BIT,AppUserId NVARCHAR (MAX))
	INSERT INTO CombatLog
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Name,@Date,@LogType,@NumberReadyCombats,@CombatsInQueue,@IsReady,@AppUserId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateCombatLog (@Id INT,@Name NVARCHAR (MAX),@Date DATETIMEOFFSET (7),@LogType INT,@NumberReadyCombats INT,@CombatsInQueue INT,@IsReady BIT,@AppUserId NVARCHAR (MAX))
AS
BEGIN
	UPDATE CombatLog
	SET Name = @Name,Date = @Date,LogType = @LogType,NumberReadyCombats = @NumberReadyCombats,CombatsInQueue = @CombatsInQueue,IsReady = @IsReady,AppUserId = @AppUserId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteCombatLogById (@id INT)
AS
BEGIN
	DELETE FROM CombatLog
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllCombatPlayer
AS
BEGIN
	SELECT * 
	FROM CombatPlayer
END
GO

CREATE PROCEDURE GetCombatPlayerById (@id INT)
AS
BEGIN
	SELECT * 
	FROM CombatPlayer
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoCombatPlayer (@Username NVARCHAR (MAX),@PlayerId NVARCHAR (MAX),@AverageItemLevel FLOAT (53),@ResourcesRecovery INT,@DamageDone INT,@HealDone INT,@DamageTaken INT,@CombatId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Username NVARCHAR (MAX),PlayerId NVARCHAR (MAX),AverageItemLevel FLOAT (53),ResourcesRecovery INT,DamageDone INT,HealDone INT,DamageTaken INT,CombatId INT)
	INSERT INTO CombatPlayer
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Username,@PlayerId,@AverageItemLevel,@ResourcesRecovery,@DamageDone,@HealDone,@DamageTaken,@CombatId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateCombatPlayer (@Id INT,@Username NVARCHAR (MAX),@PlayerId NVARCHAR (MAX),@AverageItemLevel FLOAT (53),@ResourcesRecovery INT,@DamageDone INT,@HealDone INT,@DamageTaken INT,@CombatId INT)
AS
BEGIN
	UPDATE CombatPlayer
	SET Username = @Username,PlayerId = @PlayerId,AverageItemLevel = @AverageItemLevel,ResourcesRecovery = @ResourcesRecovery,DamageDone = @DamageDone,HealDone = @HealDone,DamageTaken = @DamageTaken,CombatId = @CombatId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteCombatPlayerById (@id INT)
AS
BEGIN
	DELETE FROM CombatPlayer
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllCombatAura
AS
BEGIN
	SELECT * 
	FROM CombatAura
END
GO

CREATE PROCEDURE GetCombatAuraById (@id INT)
AS
BEGIN
	SELECT * 
	FROM CombatAura
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoCombatAura (@Name NVARCHAR (MAX),@Creator NVARCHAR (MAX),@Target NVARCHAR (MAX),@AuraCreatorType INT,@AuraType INT,@StartTime TIME (7),@FinishTime TIME (7),@Stacks INT,@CombatId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Name NVARCHAR (MAX),Creator NVARCHAR (MAX),Target NVARCHAR (MAX),AuraCreatorType INT,AuraType INT,StartTime TIME (7),FinishTime TIME (7),Stacks INT,CombatId INT)
	INSERT INTO CombatAura
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Name,@Creator,@Target,@AuraCreatorType,@AuraType,@StartTime,@FinishTime,@Stacks,@CombatId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateCombatAura (@Id INT,@Name NVARCHAR (MAX),@Creator NVARCHAR (MAX),@Target NVARCHAR (MAX),@AuraCreatorType INT,@AuraType INT,@StartTime TIME (7),@FinishTime TIME (7),@Stacks INT,@CombatId INT)
AS
BEGIN
	UPDATE CombatAura
	SET Name = @Name,Creator = @Creator,Target = @Target,AuraCreatorType = @AuraCreatorType,AuraType = @AuraType,StartTime = @StartTime,FinishTime = @FinishTime,Stacks = @Stacks,CombatId = @CombatId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteCombatAuraById (@id INT)
AS
BEGIN
	DELETE FROM CombatAura
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllCombatPlayerPosition
AS
BEGIN
	SELECT * 
	FROM CombatPlayerPosition
END
GO

CREATE PROCEDURE GetCombatPlayerPositionById (@id INT)
AS
BEGIN
	SELECT * 
	FROM CombatPlayerPosition
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoCombatPlayerPosition (@PositionX FLOAT (53),@PositionY FLOAT (53),@Time TIME (7),@CombatPlayerId INT,@CombatId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,PositionX FLOAT (53),PositionY FLOAT (53),Time TIME (7),CombatPlayerId INT,CombatId INT)
	INSERT INTO CombatPlayerPosition
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@PositionX,@PositionY,@Time,@CombatPlayerId,@CombatId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateCombatPlayerPosition (@Id INT,@PositionX FLOAT (53),@PositionY FLOAT (53),@Time TIME (7),@CombatPlayerId INT,@CombatId INT)
AS
BEGIN
	UPDATE CombatPlayerPosition
	SET PositionX = @PositionX,PositionY = @PositionY,Time = @Time,CombatPlayerId = @CombatPlayerId,CombatId = @CombatId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteCombatPlayerPositionById (@id INT)
AS
BEGIN
	DELETE FROM CombatPlayerPosition
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllPlayerParseInfo
AS
BEGIN
	SELECT * 
	FROM PlayerParseInfo
END
GO

CREATE PROCEDURE GetPlayerParseInfoById (@id INT)
AS
BEGIN
	SELECT * 
	FROM PlayerParseInfo
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoPlayerParseInfo (@SpecId INT,@ClassId INT,@BossId INT,@Difficult INT,@DamageEfficiency INT,@HealEfficiency INT,@CombatPlayerId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,SpecId INT,ClassId INT,BossId INT,Difficult INT,DamageEfficiency INT,HealEfficiency INT,CombatPlayerId INT)
	INSERT INTO PlayerParseInfo
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@SpecId,@ClassId,@BossId,@Difficult,@DamageEfficiency,@HealEfficiency,@CombatPlayerId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdatePlayerParseInfo (@Id INT,@SpecId INT,@ClassId INT,@BossId INT,@Difficult INT,@DamageEfficiency INT,@HealEfficiency INT,@CombatPlayerId INT)
AS
BEGIN
	UPDATE PlayerParseInfo
	SET SpecId = @SpecId,ClassId = @ClassId,BossId = @BossId,Difficult = @Difficult,DamageEfficiency = @DamageEfficiency,HealEfficiency = @HealEfficiency,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeletePlayerParseInfoById (@id INT)
AS
BEGIN
	DELETE FROM PlayerParseInfo
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllSpecializationScore
AS
BEGIN
	SELECT * 
	FROM SpecializationScore
END
GO

CREATE PROCEDURE GetSpecializationScoreById (@id INT)
AS
BEGIN
	SELECT * 
	FROM SpecializationScore
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoSpecializationScore (@SpecId INT,@BossId INT,@Difficult INT,@Damage INT,@Heal INT,@Updated DATETIMEOFFSET (7))
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,SpecId INT,BossId INT,Difficult INT,Damage INT,Heal INT,Updated DATETIMEOFFSET (7))
	INSERT INTO SpecializationScore
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@SpecId,@BossId,@Difficult,@Damage,@Heal,@Updated)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateSpecializationScore (@Id INT,@SpecId INT,@BossId INT,@Difficult INT,@Damage INT,@Heal INT,@Updated DATETIMEOFFSET (7))
AS
BEGIN
	UPDATE SpecializationScore
	SET SpecId = @SpecId,BossId = @BossId,Difficult = @Difficult,Damage = @Damage,Heal = @Heal,Updated = @Updated
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteSpecializationScoreById (@id INT)
AS
BEGIN
	DELETE FROM SpecializationScore
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllCombat
AS
BEGIN
	SELECT * 
	FROM Combat
END
GO

CREATE PROCEDURE GetCombatById (@id INT)
AS
BEGIN
	SELECT * 
	FROM Combat
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoCombat (@LocallyNumber INT,@DungeonName NVARCHAR (MAX),@Name NVARCHAR (MAX),@Difficulty INT,@DamageDone INT,@HealDone INT,@DamageTaken INT,@EnergyRecovery INT,@IsWin BIT,@StartDate DATETIMEOFFSET (7),@FinishDate DATETIMEOFFSET (7),@IsReady BIT,@CombatLogId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,LocallyNumber INT,DungeonName NVARCHAR (MAX),Name NVARCHAR (MAX),Difficulty INT,DamageDone INT,HealDone INT,DamageTaken INT,EnergyRecovery INT,IsWin BIT,StartDate DATETIMEOFFSET (7),FinishDate DATETIMEOFFSET (7),IsReady BIT,CombatLogId INT)
	INSERT INTO Combat
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@LocallyNumber,@DungeonName,@Name,@Difficulty,@DamageDone,@HealDone,@DamageTaken,@EnergyRecovery,@IsWin,@StartDate,@FinishDate,@IsReady,@CombatLogId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateCombat (@Id INT,@LocallyNumber INT,@DungeonName NVARCHAR (MAX),@Name NVARCHAR (MAX),@Difficulty INT,@DamageDone INT,@HealDone INT,@DamageTaken INT,@EnergyRecovery INT,@IsWin BIT,@StartDate DATETIMEOFFSET (7),@FinishDate DATETIMEOFFSET (7),@IsReady BIT,@CombatLogId INT)
AS
BEGIN
	UPDATE Combat
	SET LocallyNumber = @LocallyNumber,DungeonName = @DungeonName,Name = @Name,Difficulty = @Difficulty,DamageDone = @DamageDone,HealDone = @HealDone,DamageTaken = @DamageTaken,EnergyRecovery = @EnergyRecovery,IsWin = @IsWin,StartDate = @StartDate,FinishDate = @FinishDate,IsReady = @IsReady,CombatLogId = @CombatLogId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteCombatById (@id INT)
AS
BEGIN
	DELETE FROM Combat
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllDamageDone
AS
BEGIN
	SELECT * 
	FROM DamageDone
END
GO

CREATE PROCEDURE GetDamageDoneById (@id INT)
AS
BEGIN
	SELECT * 
	FROM DamageDone
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoDamageDone (@Spell NVARCHAR (MAX),@Value INT,@Time TIME (7),@Creator NVARCHAR (MAX),@Target NVARCHAR (MAX),@DamageType INT,@IsPeriodicDamage BIT,@IsPet BIT,@CombatPlayerId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Spell NVARCHAR (MAX),Value INT,Time TIME (7),Creator NVARCHAR (MAX),Target NVARCHAR (MAX),DamageType INT,IsPeriodicDamage BIT,IsPet BIT,CombatPlayerId INT)
	INSERT INTO DamageDone
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Spell,@Value,@Time,@Creator,@Target,@DamageType,@IsPeriodicDamage,@IsPet,@CombatPlayerId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateDamageDone (@Id INT,@Spell NVARCHAR (MAX),@Value INT,@Time TIME (7),@Creator NVARCHAR (MAX),@Target NVARCHAR (MAX),@DamageType INT,@IsPeriodicDamage BIT,@IsPet BIT,@CombatPlayerId INT)
AS
BEGIN
	UPDATE DamageDone
	SET Spell = @Spell,Value = @Value,Time = @Time,Creator = @Creator,Target = @Target,DamageType = @DamageType,IsPeriodicDamage = @IsPeriodicDamage,IsPet = @IsPet,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteDamageDoneById (@id INT)
AS
BEGIN
	DELETE FROM DamageDone
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllDamageDoneGeneral
AS
BEGIN
	SELECT * 
	FROM DamageDoneGeneral
END
GO

CREATE PROCEDURE GetDamageDoneGeneralById (@id INT)
AS
BEGIN
	SELECT * 
	FROM DamageDoneGeneral
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoDamageDoneGeneral (@Value INT,@DamagePerSecond FLOAT (53),@Spell NVARCHAR (MAX),@CritNumber INT,@MissNumber INT,@CastNumber INT,@MinValue INT,@MaxValue INT,@AverageValue FLOAT (53),@IsPet BIT,@CombatPlayerId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Value INT,DamagePerSecond FLOAT (53),Spell NVARCHAR (MAX),CritNumber INT,MissNumber INT,CastNumber INT,MinValue INT,MaxValue INT,AverageValue FLOAT (53),IsPet BIT,CombatPlayerId INT)
	INSERT INTO DamageDoneGeneral
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Value,@DamagePerSecond,@Spell,@CritNumber,@MissNumber,@CastNumber,@MinValue,@MaxValue,@AverageValue,@IsPet,@CombatPlayerId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateDamageDoneGeneral (@Id INT,@Value INT,@DamagePerSecond FLOAT (53),@Spell NVARCHAR (MAX),@CritNumber INT,@MissNumber INT,@CastNumber INT,@MinValue INT,@MaxValue INT,@AverageValue FLOAT (53),@IsPet BIT,@CombatPlayerId INT)
AS
BEGIN
	UPDATE DamageDoneGeneral
	SET Value = @Value,DamagePerSecond = @DamagePerSecond,Spell = @Spell,CritNumber = @CritNumber,MissNumber = @MissNumber,CastNumber = @CastNumber,MinValue = @MinValue,MaxValue = @MaxValue,AverageValue = @AverageValue,IsPet = @IsPet,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteDamageDoneGeneralById (@id INT)
AS
BEGIN
	DELETE FROM DamageDoneGeneral
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllHealDone
AS
BEGIN
	SELECT * 
	FROM HealDone
END
GO

CREATE PROCEDURE GetHealDoneById (@id INT)
AS
BEGIN
	SELECT * 
	FROM HealDone
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoHealDone (@Spell NVARCHAR (MAX),@Value INT,@Time TIME (7),@Creator NVARCHAR (MAX),@Target NVARCHAR (MAX),@Overheal INT,@IsCrit BIT,@IsAbsorbed BIT,@CombatPlayerId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Spell NVARCHAR (MAX),Value INT,Time TIME (7),Creator NVARCHAR (MAX),Target NVARCHAR (MAX),Overheal INT,IsCrit BIT,IsAbsorbed BIT,CombatPlayerId INT)
	INSERT INTO HealDone
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Spell,@Value,@Time,@Creator,@Target,@Overheal,@IsCrit,@IsAbsorbed,@CombatPlayerId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateHealDone (@Id INT,@Spell NVARCHAR (MAX),@Value INT,@Time TIME (7),@Creator NVARCHAR (MAX),@Target NVARCHAR (MAX),@Overheal INT,@IsCrit BIT,@IsAbsorbed BIT,@CombatPlayerId INT)
AS
BEGIN
	UPDATE HealDone
	SET Spell = @Spell,Value = @Value,Time = @Time,Creator = @Creator,Target = @Target,Overheal = @Overheal,IsCrit = @IsCrit,IsAbsorbed = @IsAbsorbed,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteHealDoneById (@id INT)
AS
BEGIN
	DELETE FROM HealDone
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllHealDoneGeneral
AS
BEGIN
	SELECT * 
	FROM HealDoneGeneral
END
GO

CREATE PROCEDURE GetHealDoneGeneralById (@id INT)
AS
BEGIN
	SELECT * 
	FROM HealDoneGeneral
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoHealDoneGeneral (@Spell NVARCHAR (MAX),@Value INT,@HealPerSecond FLOAT (53),@CritNumber INT,@CastNumber INT,@MinValue INT,@MaxValue INT,@AverageValue FLOAT (53),@CombatPlayerId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Spell NVARCHAR (MAX),Value INT,HealPerSecond FLOAT (53),CritNumber INT,CastNumber INT,MinValue INT,MaxValue INT,AverageValue FLOAT (53),CombatPlayerId INT)
	INSERT INTO HealDoneGeneral
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Spell,@Value,@HealPerSecond,@CritNumber,@CastNumber,@MinValue,@MaxValue,@AverageValue,@CombatPlayerId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateHealDoneGeneral (@Id INT,@Spell NVARCHAR (MAX),@Value INT,@HealPerSecond FLOAT (53),@CritNumber INT,@CastNumber INT,@MinValue INT,@MaxValue INT,@AverageValue FLOAT (53),@CombatPlayerId INT)
AS
BEGIN
	UPDATE HealDoneGeneral
	SET Spell = @Spell,Value = @Value,HealPerSecond = @HealPerSecond,CritNumber = @CritNumber,CastNumber = @CastNumber,MinValue = @MinValue,MaxValue = @MaxValue,AverageValue = @AverageValue,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteHealDoneGeneralById (@id INT)
AS
BEGIN
	DELETE FROM HealDoneGeneral
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllDamageTaken
AS
BEGIN
	SELECT * 
	FROM DamageTaken
END
GO

CREATE PROCEDURE GetDamageTakenById (@id INT)
AS
BEGIN
	SELECT * 
	FROM DamageTaken
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoDamageTaken (@Spell NVARCHAR (MAX),@Value INT,@Time TIME (7),@Creator NVARCHAR (MAX),@Target NVARCHAR (MAX),@DamageTakenType INT,@ActualValue INT,@IsPeriodicDamage BIT,@Resisted INT,@Absorbed INT,@Blocked INT,@RealDamage INT,@Mitigated INT,@CombatPlayerId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Spell NVARCHAR (MAX),Value INT,Time TIME (7),Creator NVARCHAR (MAX),Target NVARCHAR (MAX),DamageTakenType INT,ActualValue INT,IsPeriodicDamage BIT,Resisted INT,Absorbed INT,Blocked INT,RealDamage INT,Mitigated INT,CombatPlayerId INT)
	INSERT INTO DamageTaken
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Spell,@Value,@Time,@Creator,@Target,@DamageTakenType,@ActualValue,@IsPeriodicDamage,@Resisted,@Absorbed,@Blocked,@RealDamage,@Mitigated,@CombatPlayerId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateDamageTaken (@Id INT,@Spell NVARCHAR (MAX),@Value INT,@Time TIME (7),@Creator NVARCHAR (MAX),@Target NVARCHAR (MAX),@DamageTakenType INT,@ActualValue INT,@IsPeriodicDamage BIT,@Resisted INT,@Absorbed INT,@Blocked INT,@RealDamage INT,@Mitigated INT,@CombatPlayerId INT)
AS
BEGIN
	UPDATE DamageTaken
	SET Spell = @Spell,Value = @Value,Time = @Time,Creator = @Creator,Target = @Target,DamageTakenType = @DamageTakenType,ActualValue = @ActualValue,IsPeriodicDamage = @IsPeriodicDamage,Resisted = @Resisted,Absorbed = @Absorbed,Blocked = @Blocked,RealDamage = @RealDamage,Mitigated = @Mitigated,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteDamageTakenById (@id INT)
AS
BEGIN
	DELETE FROM DamageTaken
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllDamageTakenGeneral
AS
BEGIN
	SELECT * 
	FROM DamageTakenGeneral
END
GO

CREATE PROCEDURE GetDamageTakenGeneralById (@id INT)
AS
BEGIN
	SELECT * 
	FROM DamageTakenGeneral
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoDamageTakenGeneral (@Spell NVARCHAR (MAX),@Value INT,@ActualValue INT,@DamageTakenPerSecond FLOAT (53),@CritNumber INT,@MissNumber INT,@CastNumber INT,@MinValue INT,@MaxValue INT,@AverageValue FLOAT (53),@CombatPlayerId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Spell NVARCHAR (MAX),Value INT,ActualValue INT,DamageTakenPerSecond FLOAT (53),CritNumber INT,MissNumber INT,CastNumber INT,MinValue INT,MaxValue INT,AverageValue FLOAT (53),CombatPlayerId INT)
	INSERT INTO DamageTakenGeneral
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Spell,@Value,@ActualValue,@DamageTakenPerSecond,@CritNumber,@MissNumber,@CastNumber,@MinValue,@MaxValue,@AverageValue,@CombatPlayerId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateDamageTakenGeneral (@Id INT,@Spell NVARCHAR (MAX),@Value INT,@ActualValue INT,@DamageTakenPerSecond FLOAT (53),@CritNumber INT,@MissNumber INT,@CastNumber INT,@MinValue INT,@MaxValue INT,@AverageValue FLOAT (53),@CombatPlayerId INT)
AS
BEGIN
	UPDATE DamageTakenGeneral
	SET Spell = @Spell,Value = @Value,ActualValue = @ActualValue,DamageTakenPerSecond = @DamageTakenPerSecond,CritNumber = @CritNumber,MissNumber = @MissNumber,CastNumber = @CastNumber,MinValue = @MinValue,MaxValue = @MaxValue,AverageValue = @AverageValue,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteDamageTakenGeneralById (@id INT)
AS
BEGIN
	DELETE FROM DamageTakenGeneral
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllResourceRecovery
AS
BEGIN
	SELECT * 
	FROM ResourceRecovery
END
GO

CREATE PROCEDURE GetResourceRecoveryById (@id INT)
AS
BEGIN
	SELECT * 
	FROM ResourceRecovery
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoResourceRecovery (@Spell NVARCHAR (MAX),@Value INT,@Time TIME (7),@Creator NVARCHAR (MAX),@Target NVARCHAR (MAX),@CombatPlayerId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Spell NVARCHAR (MAX),Value INT,Time TIME (7),Creator NVARCHAR (MAX),Target NVARCHAR (MAX),CombatPlayerId INT)
	INSERT INTO ResourceRecovery
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Spell,@Value,@Time,@Creator,@Target,@CombatPlayerId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateResourceRecovery (@Id INT,@Spell NVARCHAR (MAX),@Value INT,@Time TIME (7),@Creator NVARCHAR (MAX),@Target NVARCHAR (MAX),@CombatPlayerId INT)
AS
BEGIN
	UPDATE ResourceRecovery
	SET Spell = @Spell,Value = @Value,Time = @Time,Creator = @Creator,Target = @Target,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteResourceRecoveryById (@id INT)
AS
BEGIN
	DELETE FROM ResourceRecovery
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllResourceRecoveryGeneral
AS
BEGIN
	SELECT * 
	FROM ResourceRecoveryGeneral
END
GO

CREATE PROCEDURE GetResourceRecoveryGeneralById (@id INT)
AS
BEGIN
	SELECT * 
	FROM ResourceRecoveryGeneral
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoResourceRecoveryGeneral (@Spell NVARCHAR (MAX),@Value INT,@ResourcePerSecond FLOAT (53),@CastNumber INT,@MinValue INT,@MaxValue INT,@AverageValue FLOAT (53),@CombatPlayerId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Spell NVARCHAR (MAX),Value INT,ResourcePerSecond FLOAT (53),CastNumber INT,MinValue INT,MaxValue INT,AverageValue FLOAT (53),CombatPlayerId INT)
	INSERT INTO ResourceRecoveryGeneral
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Spell,@Value,@ResourcePerSecond,@CastNumber,@MinValue,@MaxValue,@AverageValue,@CombatPlayerId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdateResourceRecoveryGeneral (@Id INT,@Spell NVARCHAR (MAX),@Value INT,@ResourcePerSecond FLOAT (53),@CastNumber INT,@MinValue INT,@MaxValue INT,@AverageValue FLOAT (53),@CombatPlayerId INT)
AS
BEGIN
	UPDATE ResourceRecoveryGeneral
	SET Spell = @Spell,Value = @Value,ResourcePerSecond = @ResourcePerSecond,CastNumber = @CastNumber,MinValue = @MinValue,MaxValue = @MaxValue,AverageValue = @AverageValue,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeleteResourceRecoveryGeneralById (@id INT)
AS
BEGIN
	DELETE FROM ResourceRecoveryGeneral
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetAllPlayerDeath
AS
BEGIN
	SELECT * 
	FROM PlayerDeath
END
GO

CREATE PROCEDURE GetPlayerDeathById (@id INT)
AS
BEGIN
	SELECT * 
	FROM PlayerDeath
	WHERE Id = @id
END
GO

CREATE PROCEDURE InsertIntoPlayerDeath (@Username NVARCHAR (MAX),@LastHitSpellOrItem NVARCHAR (MAX),@LastHitValue INT,@Time TIME (7),@CombatPlayerId INT)
AS
BEGIN
	DECLARE @OutputTbl TABLE (Id INT,Username NVARCHAR (MAX),LastHitSpellOrItem NVARCHAR (MAX),LastHitValue INT,Time TIME (7),CombatPlayerId INT)
	INSERT INTO PlayerDeath
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Username,@LastHitSpellOrItem,@LastHitValue,@Time,@CombatPlayerId)
	SELECT * FROM @OutputTbl
END
GO

CREATE PROCEDURE UpdatePlayerDeath (@Id INT,@Username NVARCHAR (MAX),@LastHitSpellOrItem NVARCHAR (MAX),@LastHitValue INT,@Time TIME (7),@CombatPlayerId INT)
AS
BEGIN
	UPDATE PlayerDeath
	SET Username = @Username,LastHitSpellOrItem = @LastHitSpellOrItem,LastHitValue = @LastHitValue,Time = @Time,CombatPlayerId = @CombatPlayerId
	WHERE Id = @Id
END
GO

CREATE PROCEDURE DeletePlayerDeathById (@id INT)
AS
BEGIN
	DELETE FROM PlayerDeath
	WHERE Id = @id
END
GO

CREATE PROCEDURE GetDamageDoneByCombatPlayerIdPagination (@combatPlayerId INT, @page INT, @pageSize INT)
AS
BEGIN
	SELECT * 
	FROM DamageDone
	WHERE CombatPlayerId = @combatPlayerId
	ORDER BY Id
	OFFSET (@page - 1) * @pageSize ROWS
	FETCH NEXT @pageSize ROWS ONLY
END
GO

CREATE PROCEDURE GetHealDoneByCombatPlayerIdPagination (@combatPlayerId INT, @page INT, @pageSize INT)
AS
BEGIN
	SELECT * 
	FROM HealDone
	WHERE CombatPlayerId = @combatPlayerId
	ORDER BY Id
	OFFSET (@page - 1) * @pageSize ROWS
	FETCH NEXT @pageSize ROWS ONLY
END
GO

CREATE PROCEDURE GetDamageTakenByCombatPlayerIdPagination (@combatPlayerId INT, @page INT, @pageSize INT)
AS
BEGIN
	SELECT * 
	FROM DamageTaken
	WHERE CombatPlayerId = @combatPlayerId
	ORDER BY Id
	OFFSET (@page - 1) * @pageSize ROWS
	FETCH NEXT @pageSize ROWS ONLY
END
GO

CREATE PROCEDURE GetResourceRecoveryByCombatPlayerIdPagination (@combatPlayerId INT, @page INT, @pageSize INT)
AS
BEGIN
	SELECT * 
	FROM ResourceRecovery
	WHERE CombatPlayerId = @combatPlayerId
	ORDER BY Id
	OFFSET (@page - 1) * @pageSize ROWS
	FETCH NEXT @pageSize ROWS ONLY
END
GO

CREATE PROCEDURE GetDamageDoneByCombatPlayerId (@combatPlayerId INT)
AS
BEGIN
	SELECT * 
	FROM DamageDone
	WHERE CombatPlayerId = @combatPlayerId
END
GO

CREATE PROCEDURE GetDamageDoneGeneralByCombatPlayerId (@combatPlayerId INT)
AS
BEGIN
	SELECT * 
	FROM DamageDoneGeneral
	WHERE CombatPlayerId = @combatPlayerId
END
GO

CREATE PROCEDURE GetHealDoneByCombatPlayerId (@combatPlayerId INT)
AS
BEGIN
	SELECT * 
	FROM HealDone
	WHERE CombatPlayerId = @combatPlayerId
END
GO

CREATE PROCEDURE GetHealDoneGeneralByCombatPlayerId (@combatPlayerId INT)
AS
BEGIN
	SELECT * 
	FROM HealDoneGeneral
	WHERE CombatPlayerId = @combatPlayerId
END
GO

CREATE PROCEDURE GetDamageTakenByCombatPlayerId (@combatPlayerId INT)
AS
BEGIN
	SELECT * 
	FROM DamageTaken
	WHERE CombatPlayerId = @combatPlayerId
END
GO

CREATE PROCEDURE GetDamageTakenGeneralByCombatPlayerId (@combatPlayerId INT)
AS
BEGIN
	SELECT * 
	FROM DamageTakenGeneral
	WHERE CombatPlayerId = @combatPlayerId
END
GO

CREATE PROCEDURE GetResourceRecoveryByCombatPlayerId (@combatPlayerId INT)
AS
BEGIN
	SELECT * 
	FROM ResourceRecovery
	WHERE CombatPlayerId = @combatPlayerId
END
GO

CREATE PROCEDURE GetResourceRecoveryGeneralByCombatPlayerId (@combatPlayerId INT)
AS
BEGIN
	SELECT * 
	FROM ResourceRecoveryGeneral
	WHERE CombatPlayerId = @combatPlayerId
END
GO

CREATE PROCEDURE GetPlayerDeathByCombatPlayerId (@combatPlayerId INT)
AS
BEGIN
	SELECT * 
	FROM PlayerDeath
	WHERE CombatPlayerId = @combatPlayerId
END
GO

CREATE PROCEDURE GetSpecializationScoreBySpecId (@specId INT, @bossId INT, @difficult INT)
AS
BEGIN
	SELECT * 
	FROM SpecializationScore
	WHERE SpecId = @specId AND BossId = @bossId AND Difficult = @difficult
END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250122141224_InitialCreate', N'9.0.1');

COMMIT;
GO

