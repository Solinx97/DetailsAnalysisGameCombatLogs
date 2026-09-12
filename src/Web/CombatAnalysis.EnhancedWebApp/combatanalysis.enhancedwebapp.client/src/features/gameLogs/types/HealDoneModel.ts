import type { CombatUnitModel } from './CombatUnitModel';

export type HealDoneModel = {
    id: number;
    spell: string;
    value: number;
    overheal: number;
    time: string;
    creator: CombatUnitModel;
    target: CombatUnitModel;
    modificationType: number;
    combatPlayerId: number;
}