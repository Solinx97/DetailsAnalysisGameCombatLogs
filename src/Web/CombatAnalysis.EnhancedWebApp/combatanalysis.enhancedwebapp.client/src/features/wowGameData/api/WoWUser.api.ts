import type { CharacterMountModel } from '../types/CharacterMountModel';
import { BattleNetDataApi } from './BattleNetData.api';

export const WoWUserApi = BattleNetDataApi.injectEndpoints({
    endpoints: builder => ({
        getUserMounts: builder.query<CharacterMountModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWUser/getMounts?regionName=${regionName}`,
        }),
    })
})

export const {
    useLazyGetUserMountsQuery,
} = WoWUserApi;