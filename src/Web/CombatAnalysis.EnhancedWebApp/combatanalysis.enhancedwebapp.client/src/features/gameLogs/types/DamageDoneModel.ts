export type DamageDoneModel = {
    id: number;
    spell: string;
    value: number;
    time: string;
    creatorGameId: string;
    targetGameId: string;
    targetHash: string;
    targetCurrentHealth: number;
    modificationType: number;
    damageType: number;
    resisted: number;
    absorbed: number;
    blocked: number;
    realDamage: number;
    overkill: number;
    mitigated: number;
    combatPlayerId: number;
}