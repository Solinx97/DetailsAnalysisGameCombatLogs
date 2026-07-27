export type CombatUnitModel = {
    id: string;
    gameId: string;
    username: string;
    creatorGameId: string | null;
    unitType: string | null;
    combatId: number;
}