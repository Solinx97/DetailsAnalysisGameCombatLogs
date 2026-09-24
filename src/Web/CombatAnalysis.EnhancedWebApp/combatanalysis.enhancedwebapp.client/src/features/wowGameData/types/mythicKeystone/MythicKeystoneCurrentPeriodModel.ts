import type { MythicKeystoneBestRunModel } from './MythicKeystoneBestRunModel';
import type { MythicKeystoneSeasonModel } from './MythicKeystoneSeasonModel';

export type MythicKeystoneCurrentPeriodModel = {
    period: MythicKeystoneSeasonModel;
    bestRuns: MythicKeystoneBestRunModel[];
}