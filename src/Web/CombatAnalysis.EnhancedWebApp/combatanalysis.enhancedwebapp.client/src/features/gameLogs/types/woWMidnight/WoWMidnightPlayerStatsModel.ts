import type { CombatPlayerStatsModel } from '../CombatPlayerStatsModel';

export interface WoWMidnightPlayerStatsModel extends CombatPlayerStatsModel {
    mastery: number;
    versality: number;
    lifesteal: number;
    avoidance: number;
    movement: number;
}