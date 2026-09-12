import type { CombatUnitModel } from './CombatUnitModel';

export type ResourceRecoveryModel = {
    id: number;
    spell: string;
    value: number;
    time: string;
    creator: CombatUnitModel;
    target: CombatUnitModel;
    modificationType: number;
    combatPlayerId: number;
}