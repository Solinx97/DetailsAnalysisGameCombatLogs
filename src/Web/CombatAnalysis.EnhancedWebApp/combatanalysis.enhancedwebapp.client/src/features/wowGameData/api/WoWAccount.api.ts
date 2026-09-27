import type { WoWAccountRespone } from '../types/account/WoWAccountRespone';
import type { WoWMountModel } from '../types/WoWMountModel';
import { BattleNetDataApi } from './BattleNetData.api';

export const WoWAccountApi = BattleNetDataApi.injectEndpoints({
    endpoints: builder => ({
        getAccountCharacters: builder.query<WoWAccountRespone, { regionName: string }>({
            query: ({ regionName }) => `/WoWAccount/getCharacters?regionName=${regionName}`,
        }),
        getAccountMounts: builder.query<WoWMountModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWAccount/getMounts?regionName=${regionName}`,
        }),
    })
})

export const {
    useGetAccountCharactersQuery,
    useGetAccountMountsQuery,
} = WoWAccountApi;