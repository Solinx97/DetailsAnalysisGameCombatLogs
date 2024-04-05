CREATE PROCEDURE GetDamageDoneGeneralByCombatPlayerId (@combatPlayerId INT)
	AS SELECT * 
	FROM DamageDoneGeneral
	WHERE CombatPlayerId = @combatPlayerId