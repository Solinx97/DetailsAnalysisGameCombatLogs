CREATE PROCEDURE UpdateSpecializationScore (@Id INT,@SpecId INT,@BossId INT,@Difficult INT,@Damage INT,@Heal INT,@Updated DATETIMEOFFSET (7))
	AS UPDATE SpecializationScore
	SET SpecId = @SpecId,BossId = @BossId,Difficult = @Difficult,Damage = @Damage,Heal = @Heal,Updated = @Updated
	WHERE Id = @Id