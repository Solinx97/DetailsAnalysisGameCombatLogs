export type UnitHealthModel = {
    id: string;
    gamePlayerId: string;
    currentHealth: number;
    maxHealth: number;
    time: string;
    isDead: boolean;
    combatId: number;
}