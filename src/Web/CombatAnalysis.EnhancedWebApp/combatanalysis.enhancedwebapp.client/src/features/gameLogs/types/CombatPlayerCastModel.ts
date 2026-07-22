export type CombatPlayerCastModel = {
    id: number;
    gameSpellId: number;
    spell: string;
    startTime: string;
    finishTime: string;
    creator: string;
    target: string;
    isImmediatly: boolean;
    isSuccess: boolean;
    combatPlayerId: number;
}