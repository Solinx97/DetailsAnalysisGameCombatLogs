import type { MythicKeystoneBestRunModel } from '../mythicKeystone/MythicKeystoneBestRunModel';
import type { MythicKeystoneRaitingModel } from '../mythicKeystone/MythicKeystoneRaitingModel';
import type { WoWGameDataEntityModel } from '../WoWGameDataEntityModel';

export type MythicKeystoneSeasonModel = {
    bestRuns: MythicKeystoneBestRunModel[];
    season: WoWGameDataEntityModel;
    mythicRating: MythicKeystoneRaitingModel;
}