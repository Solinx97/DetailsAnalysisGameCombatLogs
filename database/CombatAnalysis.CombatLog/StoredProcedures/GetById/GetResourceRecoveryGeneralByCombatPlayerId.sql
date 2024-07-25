CREATE PROCEDURE GetResourceRecoveryGeneralByCombatPlayerId (@combatPlayerId INT)
	AS SELECT * 
	FROM ResourceRecoveryGeneral
	WHERE CombatPlayerId = @combatPlayerId