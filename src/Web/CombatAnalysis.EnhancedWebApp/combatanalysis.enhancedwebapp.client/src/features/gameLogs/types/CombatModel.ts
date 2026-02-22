import type { BossModel } from "./BossModel";

export type CombatModel = {
    id: number;
    dungeonName: string;
    bossHealthPercentage: number;
    resourcesRecovery: number;
    damageDone: number;
    healDone: number;
    damageTaken: number;
    isWin: boolean;
    startDate: string;
    finishDate: string;
    duration: string;
    combatLogId: number;
    boss: BossModel;
}