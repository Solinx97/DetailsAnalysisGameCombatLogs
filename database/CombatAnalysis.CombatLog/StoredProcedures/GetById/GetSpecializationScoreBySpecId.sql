CREATE PROCEDURE GetSpecializationScoreBySpecId (@specId INT, @bossId INT, @difficult INT)
	AS SELECT * 
	FROM SpecializationScore
	WHERE SpecId = @specId AND BossId = @bossId AND Difficult = @difficult