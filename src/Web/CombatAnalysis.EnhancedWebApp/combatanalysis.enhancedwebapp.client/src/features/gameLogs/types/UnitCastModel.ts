export type UnitCastModel = {
    id: string;
    ownerGmaeId: string;
    gameSpellId: number;
    spell: string;
    time: string;
    finishTime: string | null;
    targetGameId: string | null;
    isImmediatly: boolean;
    isSuccess: boolean;
    combatUnitId: string;
}