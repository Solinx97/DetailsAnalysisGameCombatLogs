import type { UnitModel } from './UnitModel';

export type ResourceRecoveryModel = {
    id: number;
    spell: string;
    value: number;
    time: string;
    unit: UnitModel;
    target: UnitModel;
    modificationType: number;
    combatPlayerId: number;
}