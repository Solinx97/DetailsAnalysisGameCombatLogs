import type { CombatUnitModel } from './CombatUnitModel';

export type ResourceRecoveryModel = {
    id: number;
    spell: string;
    value: number;
    time: string;
    creator: CombatUnitModel;
    target: CombatUnitModel;
    combatPlayerId: number;
}