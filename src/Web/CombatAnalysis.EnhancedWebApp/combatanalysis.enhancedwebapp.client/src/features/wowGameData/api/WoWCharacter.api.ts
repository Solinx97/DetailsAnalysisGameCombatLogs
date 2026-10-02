import type { CharacterReputationModel } from '../types/CharacterReputationModel';
import type { AchievementCategoriesModel } from '../types/achievements/AchievementCategoriesModel';
import type { AchievementSelectedCategoryModel } from '../types/achievements/AchievementSelectedCategoryModel';
import type { CharacterAchievementStatisticsCategoryModel } from '../types/achievements/CharacterAchievementStatisticsCategoryModel';
import type { WoWCharacterSummaryModel } from '../types/character/WoWCharacterSummaryModel';
import type { WoWAccountCollectionItemModel } from '../types/collections/WoWAccountCollectionItemModel';
import type { CharacterDungeonModel } from '../types/dungeon/CharacterDungeonModel';
import type { MythicKeystoneSeasonModel } from '../types/dungeon/MythicKeystoneSeasonModel';
import type { CharacterEquipmentsResponse } from '../types/equipments/CharacterEquipmentsResponse';
import type { CharacterStatsModel } from '../types/equipments/CharacterStatsModel';
import type { MythicKeystoneModel } from '../types/mythicKeystone/MythicKeystoneModel';
import type { CharacterProfessionsResponse } from '../types/professions/CharacterProfessionsResponse';
import { BattleNetDataApi } from './BattleNetData.api';

export const WoWCharacterApi = BattleNetDataApi.injectEndpoints({
    endpoints: builder => ({
        getCharacterReputations: builder.query<CharacterReputationModel[], { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getReputations/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterSummary: builder.query<WoWCharacterSummaryModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getProfileSummary/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterEquipments: builder.query<CharacterEquipmentsResponse, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getEquipments/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterStats: builder.query<CharacterStatsModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getStats/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterMythicKeystone: builder.query<MythicKeystoneModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getMythicKeystone/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterMythicKeystoneSeason: builder.query<MythicKeystoneSeasonModel, { username: string, seasonId: number, serverName: string, regionName: string }>({
            query: ({ username, seasonId, serverName, regionName }) => `/WoWCharacter/getMythicKeystoneSeason/${username}?seasonId=${seasonId}&serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterRaids: builder.query<CharacterDungeonModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getRaids/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterDungeons: builder.query<CharacterDungeonModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getDungeons/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getAchievementAllCategory: builder.query<AchievementCategoriesModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getAchievementsCategory/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getAchievementsByCategory: builder.query<AchievementSelectedCategoryModel, { categoryId: number, username: string, serverName: string, regionName: string }>({
            query: ({ categoryId, username, serverName, regionName }) => `/WoWCharacter/getAchievementsByCategory/${categoryId}?username=${username}&serverName=${serverName}&regionName=${regionName}`,
        }),
        getProfessions: builder.query<CharacterProfessionsResponse, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getProfessions/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getAchievementsStatistics: builder.query<CharacterAchievementStatisticsCategoryModel[], { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getAchievementsStatisticsCategory/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getDecors: builder.query<WoWAccountCollectionItemModel[], { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getDecors/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
    })
})

export const {
    useGetCharacterReputationsQuery,
    useGetCharacterSummaryQuery,
    useGetCharacterEquipmentsQuery,
    useGetCharacterStatsQuery,
    useGetCharacterMythicKeystoneQuery,
    useGetCharacterMythicKeystoneSeasonQuery,
    useGetCharacterRaidsQuery,
    useGetCharacterDungeonsQuery,
    useGetAchievementAllCategoryQuery,
    useGetAchievementsByCategoryQuery,
    useGetProfessionsQuery,
    useGetAchievementsStatisticsQuery,
    useGetDecorsQuery,
} = WoWCharacterApi;