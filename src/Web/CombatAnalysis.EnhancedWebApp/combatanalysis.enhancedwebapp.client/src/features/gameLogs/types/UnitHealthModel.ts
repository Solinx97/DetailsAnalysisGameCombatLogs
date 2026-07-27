export type UnitHealthModel = {
    id: string;
    gameId: string;
    currentHealth: number;
    maxHealth: number;
    time: string;
    isDead: boolean;
    combatId: number;
}