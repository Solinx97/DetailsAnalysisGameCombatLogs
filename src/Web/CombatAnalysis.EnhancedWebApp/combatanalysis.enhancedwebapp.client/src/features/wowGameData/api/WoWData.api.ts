import type { SelectedAchievementModel } from '../types/achievements/SelectedAchievementModel';
import type { AuctionModel } from '../types/auction/AuctionModel';
import type { SelectedWoWAccountCollectionItemModel } from '../types/collections/SelectedWoWAccountCollectionItemModel';
import type { SelectedWoWAccountToyItemModel } from '../types/collections/SelectedWoWAccountToyItemModel';
import type { MythicKeystoneLeaderboardModel } from '../types/mythicKeystone/MythicKeystoneLeaderboardModel';
import type { SearchItemResponse } from '../types/SearchItemResponse';
import type { WoWGameDataEntityModel } from '../types/WoWGameDataEntityModel';
import type { WoWRealmModel } from '../types/WoWRealmModel';
import type { WoWTokenModel } from '../types/WoWTokenModel';
import { BattleNetDataApi } from './BattleNetData.api';

export const WoWDataApi = BattleNetDataApi.injectEndpoints({
    endpoints: builder => ({
        getRealms: builder.query<WoWRealmModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWData/getRealms/${regionName}`,
        }),
        getItemClasses: builder.query<WoWGameDataEntityModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWData/getItemClasses/${regionName}`,
        }),
        getItemSubClasses: builder.query<WoWGameDataEntityModel[], { regionName: string, itemClassId: number }>({
            query: ({ regionName, itemClassId }) => `/WoWData/getItemSubClasses/${regionName}?itemClassId=${itemClassId}`,
        }),
        searchItem: builder.query<SearchItemResponse, { name: string, regionName: string, orderBy: string, itemClassId: number, itemSubClassId: number, page: number, pageSize: number }>({
            query: ({ name, regionName, orderBy, itemClassId, itemSubClassId, page, pageSize }) => `/WoWData/searchItem/${name}?regionName=${regionName}&orderBy=${orderBy}&itemClassId=${itemClassId}&itemSubClassId=${itemSubClassId}&page=${page}&pageSize=${pageSize}`,
        }),
        getAchievement: builder.query<SelectedAchievementModel, { achievementId: number, regionName: string }>({
            query: ({ achievementId, regionName }) => `/WoWData/getAchievement/${achievementId}?regionName=${regionName}`,
        }),
        getMount: builder.query<SelectedWoWAccountCollectionItemModel, { mountId: number, regionName: string }>({
            query: ({ mountId, regionName }) => `/WoWData/getMount/${mountId}?regionName=${regionName}`,
        }),
        getPet: builder.query<SelectedWoWAccountCollectionItemModel, { petId: number, regionName: string }>({
            query: ({ petId, regionName }) => `/WoWData/getPet/${petId}?regionName=${regionName}`,
        }),
        getToy: builder.query<SelectedWoWAccountToyItemModel, { toyId: number, regionName: string }>({
            query: ({ toyId, regionName }) => `/WoWData/getToy/${toyId}?regionName=${regionName}`,
        }),
        getDecor: builder.query<SelectedWoWAccountToyItemModel, { decorId: number, regionName: string }>({
            query: ({ decorId, regionName }) => `/WoWData/getDecor/${decorId}?regionName=${regionName}`,
        }),
        getMythicKeystoneLeaderboard: builder.query<MythicKeystoneLeaderboardModel, { connectedRealmId: number, periodId: number, regionName: string }>({
            query: ({ connectedRealmId, periodId, regionName }) => `/WoWData/getMythicKeystoneLeaderboard/${connectedRealmId}?periodId=${periodId}&regionName=${regionName}`,
        }),
        getWoWToken: builder.query<WoWTokenModel, { regionName: string }>({
            query: ({ regionName }) => `/WoWData/getWoWToken/${regionName}`,
        }),
        getAuction: builder.query<AuctionModel[], { regionName: string, itemId: number }>({
            query: ({ regionName, itemId }) => `/WoWData/getAuction/${regionName}?itemId=${itemId}`,
        }),
    })
})

export const {
    useLazyGetRealmsQuery,
    useGetItemClassesQuery,
    useGetItemSubClassesQuery,
    useSearchItemQuery,
    useGetAchievementQuery,
    useGetMountQuery,
    useGetPetQuery,
    useGetToyQuery,
    useGetDecorQuery,
    useGetMythicKeystoneLeaderboardQuery,
    useGetWoWTokenQuery,
    useGetAuctionQuery,
} = WoWDataApi;