CREATE PROCEDURE GetDamageTakenGeneralByCombatPlayerId (@combatPlayerId INT)
	AS SELECT * 
	FROM DamageTakenGeneral
	WHERE CombatPlayerId = @combatPlayerId