CREATE PROCEDURE GetDamageDoneByCombatPlayerId (@combatPlayerId INT)
	AS SELECT * 
	FROM DamageDone
	WHERE CombatPlayerId = @combatPlayerId