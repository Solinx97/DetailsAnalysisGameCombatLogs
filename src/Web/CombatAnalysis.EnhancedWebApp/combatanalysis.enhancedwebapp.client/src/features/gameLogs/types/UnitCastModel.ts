export type UnitCastModel = {
    id: string;
    creatorGmaeId: string;
    gameSpellId: number;
    spell: string;
    time: string;
    finishTime: string;
    targetGameId: string | null;
    isImmediatly: boolean;
    isSuccess: boolean;
    combatId: number;
}