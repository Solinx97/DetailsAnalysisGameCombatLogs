import type { WoWMountModel } from '../types/WoWMountModel';
import { BattleNetDataApi } from './BattleNetData.api';

export const WoWUserApi = BattleNetDataApi.injectEndpoints({
    endpoints: builder => ({
        getUserMounts: builder.query<WoWMountModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWUser/getMounts?regionName=${regionName}`,
        }),
    })
})

export const {
    useGetUserMountsQuery,
} = WoWUserApi;