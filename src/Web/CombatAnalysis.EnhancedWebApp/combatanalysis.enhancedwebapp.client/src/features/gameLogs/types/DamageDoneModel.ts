import type { UnitModel } from './UnitModel';

export type DamageDoneModel = {
    id: string;
    spell: string;
    value: number;
    time: string;
    creator: UnitModel;
    target: UnitModel;
    modificationType: number;
    damageType: number;
    resisted: number;
    absorbed: number;
    blocked: number;
    realDamage: number;
    overkill: number;
    mitigated: number;
    unitId: string;
}