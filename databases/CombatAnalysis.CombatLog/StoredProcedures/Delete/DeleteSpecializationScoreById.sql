CREATE PROCEDURE DeleteSpecializationScoreById (@id INT)
	AS DELETE FROM SpecializationScore
	WHERE Id = @id