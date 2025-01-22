CREATE PROCEDURE GetSpecializationScoreById (@id INT)
	AS SELECT * 
	FROM SpecializationScore
	WHERE Id = @id