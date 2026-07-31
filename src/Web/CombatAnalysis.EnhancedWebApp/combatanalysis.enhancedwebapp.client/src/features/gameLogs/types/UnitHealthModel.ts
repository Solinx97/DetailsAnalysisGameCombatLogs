export type UnitHealthModel = {
    id: string;
    creatorGameId: string;
    currentHealth: number;
    maxHealth: number;
    time: string;
    isDead: boolean;
    combatId: number;
}