import type { DungeonCharacterModel } from "./DungeonCharacterModel";
import type { MythicKeystoneCurrentPeriodModel } from "./MythicKeystoneCurrentPeriodModel";
import type { MythicKeystoneRaitingModel } from "./MythicKeystoneRaitingModel";
import type { MythicKeystoneSeasonModel } from "./MythicKeystoneSeasonModel";

export type MythicKeystoneModel = {
    currentPeriod: MythicKeystoneCurrentPeriodModel;
    seasons: MythicKeystoneSeasonModel[];
    character: DungeonCharacterModel;
    currentMythicRating: MythicKeystoneRaitingModel;
}