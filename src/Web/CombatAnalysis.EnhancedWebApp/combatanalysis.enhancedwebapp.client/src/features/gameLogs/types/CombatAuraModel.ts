export type CombatAuraModel = {
    id: number;
    gameAuraId: number;
    name: string;
    creator: string;
    target: string;
    auraCreatorType: number;
    auraType: number;
    startTime: string;
    finishTime: string;
    stacks: number;
    combatId: number;
}