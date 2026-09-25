import type { SelectedAchievementModel } from '../types/achievements/SelectedAchievementModel';
import type { RealmModel } from '../types/RealmModel';
import { BattleNetDataApi } from './BattleNetData.api';

export const WoWDataApi = BattleNetDataApi.injectEndpoints({
    endpoints: builder => ({
        getRealms: builder.query<RealmModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWData/getRealms/${regionName}`,
        }),
        getAchievement: builder.query<SelectedAchievementModel, { achievementId: number, regionName: string }>({
            query: ({ achievementId, regionName }) => `/WoWData/getAchievement/${achievementId}?regionName=${regionName}`,
        }),
    })
})

export const {
    useLazyGetRealmsQuery,
    useGetAchievementQuery,
} = WoWDataApi;