import type { WoWGameDataEntityModel } from "../WoWGameDataEntityModel";
import type { DungeonCharacterModel } from "./DungeonCharacterModel";
import type { MythicKeystoneCurrentPeriodModel } from "./MythicKeystoneCurrentPeriodModel";
import type { MythicKeystoneRaitingModel } from "./MythicKeystoneRaitingModel";

export type MythicKeystoneModel = {
    currentPeriod: MythicKeystoneCurrentPeriodModel;
    seasons: WoWGameDataEntityModel[];
    character: DungeonCharacterModel;
    currentMythicRating: MythicKeystoneRaitingModel;
}