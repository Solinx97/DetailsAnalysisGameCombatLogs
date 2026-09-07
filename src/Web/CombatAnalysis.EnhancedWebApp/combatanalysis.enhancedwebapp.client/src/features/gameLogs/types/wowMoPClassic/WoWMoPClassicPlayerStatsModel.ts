import type { CombatPlayerStatsModel } from '../CombatPlayerStatsModel';

export interface WoWMoPClassicPlayerStatsModel extends CombatPlayerStatsModel {
    spirit: number;
    hit: number;
    expertise: number;
}