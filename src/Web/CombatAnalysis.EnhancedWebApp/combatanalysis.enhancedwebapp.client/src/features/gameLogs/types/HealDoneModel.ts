import type { UnitModel } from './UnitModel';

export type HealDoneModel = {
    id: number;
    spell: string;
    value: number;
    overheal: number;
    time: string;
    creator: UnitModel;
    target: UnitModel;
    modificationType: number;
    combatPlayerId: number;
}