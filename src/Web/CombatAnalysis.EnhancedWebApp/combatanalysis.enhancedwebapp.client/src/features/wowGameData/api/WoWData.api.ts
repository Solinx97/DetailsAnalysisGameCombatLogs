import type { SelectedAchievementModel } from '../types/achievements/SelectedAchievementModel';
import type { SelectedMountModel } from '../types/collections/SelectedMountModel';
import type { MythicKeystoneLeaderboardModel } from '../types/mythicKeystone/MythicKeystoneLeaderboardModel';
import type { WoWRealmModel } from '../types/WoWRealmModel';
import { BattleNetDataApi } from './BattleNetData.api';

export const WoWDataApi = BattleNetDataApi.injectEndpoints({
    endpoints: builder => ({
        getRealms: builder.query<WoWRealmModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWData/getRealms/${regionName}`,
        }),
        getAchievement: builder.query<SelectedAchievementModel, { achievementId: number, regionName: string }>({
            query: ({ achievementId, regionName }) => `/WoWData/getAchievement/${achievementId}?regionName=${regionName}`,
        }),
        getMount: builder.query<SelectedMountModel, { mountId: number, regionName: string }>({
            query: ({ mountId, regionName }) => `/WoWData/getMount/${mountId}?regionName=${regionName}`,
        }),
        getMythicKeystoneLeaderboard: builder.query<MythicKeystoneLeaderboardModel, { connectedRealmId: number, periodId: number, regionName: string }>({
            query: ({ connectedRealmId, periodId, regionName }) => `/WoWData/getMythicKeystoneLeaderboard/${connectedRealmId}?periodId=${periodId}&regionName=${regionName}`,
        }),
    })
})

export const {
    useLazyGetRealmsQuery,
    useGetAchievementQuery,
    useLazyGetMountQuery,
    useGetMythicKeystoneLeaderboardQuery,
} = WoWDataApi;