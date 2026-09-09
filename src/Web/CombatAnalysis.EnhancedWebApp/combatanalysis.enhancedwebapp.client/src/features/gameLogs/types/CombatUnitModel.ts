export type CombatUnitModel = {
    id: string;
    gameId: string;
    name: string;
    health: number;
    unitHash: string;
    creatorGameId: string | null;
    combatId: number;
}