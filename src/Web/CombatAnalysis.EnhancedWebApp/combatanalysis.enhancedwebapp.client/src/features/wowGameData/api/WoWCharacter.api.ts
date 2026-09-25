import type { CharacterMountModel } from '../types/CharacterMountModel';
import type { CharacterReputationModel } from '../types/CharacterReputationModel';
import type { AchievementCategoriesModel } from '../types/achievements/AchievementCategoriesModel';
import type { AchievementSelectedCategoryModel } from '../types/achievements/AchievementSelectedCategoryModel';
import type { WoWCharacterModel } from '../types/character/WoWCharacterModel';
import type { CharacterDungeonModel } from '../types/dungeon/CharacterDungeonModel';
import type { MythicKeystoneModel } from '../types/mythicKeystone/MythicKeystoneModel';
import { BattleNetDataApi } from './BattleNetData.api';

export const WoWCharacterApi = BattleNetDataApi.injectEndpoints({
    endpoints: builder => ({
        getCharacterReputations: builder.query<CharacterReputationModel[], { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getReputations/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterMounts: builder.query<CharacterMountModel[], { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getMounts/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterSummary: builder.query<WoWCharacterModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getProfileSummary/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterMythicKeystone: builder.query<MythicKeystoneModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getMythicKeystone/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterRaids: builder.query<CharacterDungeonModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getRaids/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterDungeons: builder.query<CharacterDungeonModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getDungeons/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getAchievementAllCategory: builder.query<AchievementCategoriesModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getAchievementCategory/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getAchievementsByCategory: builder.query<AchievementSelectedCategoryModel, { categoryId: number, username: string, serverName: string, regionName: string }>({
            query: ({ categoryId, username, serverName, regionName }) => `/WoWCharacter/getAchievementsByCategory/${categoryId}?username=${username}&serverName=${serverName}&regionName=${regionName}`,
        }),
    })
})

export const {
    useLazyGetCharacterReputationsQuery,
    useLazyGetCharacterMountsQuery,
    useLazyGetCharacterSummaryQuery,
    useLazyGetCharacterMythicKeystoneQuery,
    useLazyGetCharacterRaidsQuery,
    useLazyGetCharacterDungeonsQuery,
    useLazyGetAchievementAllCategoryQuery,
    useGetAchievementsByCategoryQuery,
} = WoWCharacterApi;