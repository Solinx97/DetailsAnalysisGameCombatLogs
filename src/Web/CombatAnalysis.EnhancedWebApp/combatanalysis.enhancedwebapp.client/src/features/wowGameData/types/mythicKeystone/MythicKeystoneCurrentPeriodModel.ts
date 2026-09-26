import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';
import type { MythicKeystoneBestRunModel } from './MythicKeystoneBestRunModel';

export type MythicKeystoneCurrentPeriodModel = {
    period: WoWGameDataEntityModel;
    bestRuns: MythicKeystoneBestRunModel[];
}