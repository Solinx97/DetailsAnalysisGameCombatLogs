import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CharacterMountModel } from '../types/CharacterMountModel';
import type { CharacterReputationModel } from '../types/CharacterReputationModel';
import type { WoWCharacterModel } from '../types/character/WoWCharacterModel';
import type { MythicKeystoneModel } from '../types/mythicKeystone/MythicKeystoneModel';
import type { CharacterDungeonModel } from '../types/dungeon/CharacterDungeonModel';

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
    useLazyGetCharacterSummaryQuery,
    useLazyGetCharacterMythicKeystoneQuery,
    useLazyGetCharacterRaidsQuery,
    useLazyGetCharacterDungeonsQuery,
    useLazyGetUserMountsQuery,
} = BattleNetDataApi;