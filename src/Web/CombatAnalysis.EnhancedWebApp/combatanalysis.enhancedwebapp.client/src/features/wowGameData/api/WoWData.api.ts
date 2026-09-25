import type { AchievementCategoriesModel } from '../types/achievements/AchievementCategoriesModel';
import type { AchievementSelectedCategoryModel } from '../types/achievements/AchievementSelectedCategoryModel';
import type { SelectedAchievementModel } from '../types/achievements/SelectedAchievementModel';
import type { RealmModel } from '../types/RealmModel';
import { BattleNetDataApi } from './BattleNetData.api';

export const WoWDataApi = BattleNetDataApi.injectEndpoints({
    endpoints: builder => ({
        getRealms: builder.query<RealmModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWData/getRealms/${regionName}`,
        }),
        getAchievementAllCategory: builder.query<AchievementCategoriesModel, { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWData/getAchievementCategory/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getAchievementsByCategory: builder.query<AchievementSelectedCategoryModel, { categoryId: number, username: string, serverName: string, regionName: string }>({
            query: ({ categoryId, username, serverName, regionName }) => `/WoWData/getAchievementsByCategory/${categoryId}?username=${username}&serverName=${serverName}&regionName=${regionName}`,
        }),
        getAchievement: builder.query<SelectedAchievementModel, { achievementId: number, regionName: string }>({
            query: ({ achievementId, regionName }) => `/WoWData/getAchievement/${achievementId}?regionName=${regionName}`,
        }),
    })
})

export const {
    useLazyGetRealmsQuery,
    useLazyGetAchievementAllCategoryQuery,
    useGetAchievementsByCategoryQuery,
    useGetAchievementQuery,
} = WoWDataApi;