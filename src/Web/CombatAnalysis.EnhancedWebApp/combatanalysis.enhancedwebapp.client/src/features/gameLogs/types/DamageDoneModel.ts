import type { CombatUnitModel } from './CombatUnitModel';

export type DamageDoneModel = {
    id: number;
    spell: string;
    value: number;
    time: string;
    creator: CombatUnitModel;
    target: CombatUnitModel;
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