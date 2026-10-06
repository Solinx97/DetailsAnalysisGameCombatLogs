import type { WoWCharacterSummaryModel } from '../character/WoWCharacterSummaryModel';

export type AccountDetailsModel = {
    summary: WoWCharacterSummaryModel;
    mythicKeystoneRating: number;
    achievementsReceived: number;
    achievementsCount: number;
    mountsReceived: number;
    mountsCount: number;
    petsReceived: number;
    petsCount: number;
    toysReceived: number;
    toysCount: number;
    decorsReceived: number;
    decorsCount: number;
    setTransmogsReceived: number;
    setTransmogsCount: number;
    transmogsReceived: number;
    transmogsCount: number;
    charactersCount: number;
    maxLevelCharactersCount: number;
}