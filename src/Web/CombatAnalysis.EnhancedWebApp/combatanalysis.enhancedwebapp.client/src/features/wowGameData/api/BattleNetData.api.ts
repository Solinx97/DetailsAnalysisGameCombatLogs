import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CharacterReputationModel } from '../types/CharacterReputationModel';
import type { CharacterMountModel } from '../types/CharacterMountModel';

const apiURL = '/api/v1';

export const BattleNetDataApi = createApi({
    reducerPath: 'battleNetDataAPi',
    tagTypes: [],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        battleNetDataToken: builder.mutation<void, void>({
            query: () => ({
                url: `/BattleNetIdentity`,
                method: 'POST'
            }),
        }),
        battleNetDataAuthorizaiton: builder.mutation<{ uri: string }, void>({
            query: () => ({
                url: `/BattleNetIdentity/authorization`,
                method: 'POST'
            }),
        }),
        battleNetDataCodeExchange: builder.mutation<void, { authorizationCode: string }>({
            query: ({ authorizationCode }) => ({
                url: `/BattleNetIdentity/codeExchange?authorizationCode=${authorizationCode}`,
                method: 'POST'
            }),
        }),
        battleNetDataStateValidate: builder.query<void, string>({
            query: (state) => `/BattleNetIdentity/stateValidate?state=${state}`,
        }),
        isAuthorized: builder.query<{ authenticated: boolean }, void>({
            query: () => `/BattleNetIdentity/isAuthorized`,
        }),
        battleNetDisconenct: builder.query<void, void>({
            query: () => `/BattleNetIdentity/disconnect`,
        }),
        getCharacterReputations: builder.query<CharacterReputationModel[], { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getReputations/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getCharacterMounts: builder.query<CharacterMountModel[], { username: string, serverName: string, regionName: string }>({
            query: ({ username, serverName, regionName }) => `/WoWCharacter/getMounts/${username}?serverName=${serverName}&regionName=${regionName}`,
        }),
        getUserMounts: builder.query<CharacterMountModel[], { regionName: string }>({
            query: ({ regionName }) => `/WoWUser/getMounts?regionName=${regionName}`,
        }),
    })
})

export const {
    useBattleNetDataTokenMutation,
    useBattleNetDataAuthorizaitonMutation,
    useBattleNetDataCodeExchangeMutation,
    useLazyBattleNetDataStateValidateQuery,
    useIsAuthorizedQuery,
    useLazyIsAuthorizedQuery,
    useLazyBattleNetDisconenctQuery,
    useLazyGetCharacterReputationsQuery,
    useLazyGetCharacterMountsQuery,
    useLazyGetUserMountsQuery,
} = BattleNetDataApi;