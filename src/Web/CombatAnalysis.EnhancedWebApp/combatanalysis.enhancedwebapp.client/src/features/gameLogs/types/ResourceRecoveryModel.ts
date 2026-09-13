import type { UnitModel } from './UnitModel';

export type ResourceRecoveryModel = {
    id: number;
    spell: string;
    value: number;
    time: string;
    creator: UnitModel;
    target: UnitModel;
    modificationType: number;
    combatPlayerId: number;
}