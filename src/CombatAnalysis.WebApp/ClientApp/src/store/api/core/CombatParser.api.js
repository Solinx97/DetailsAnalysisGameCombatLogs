import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const CombatParserApi = createApi({
    reducerPath: 'combatParserAPi',
    tagTyes: [
        'CombatLog',
        'PlayerDeath',
        'Combat',
        'CombatPlayer',
        'CombatPlayerPosition',
        'CombatAura',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getCombatLogs: builder.query({
            query: () => '/CombatLog',
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatLog', id })), 'CombatLog']
                    : ['CombatLog'],
        }),
        getPlayersDeathByPlayerId: builder.query({
            query: (combatPlayerId) => `/PlayerDeath/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'PlayerDeath', id })), 'PlayerDeath']
                    : ['PlayerDeath'],
        }),
        getCombatsByCombatLogId: builder.query({
            query: (combatLogId) => `/Combat/getByCombatLogId/${combatLogId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Combat', id })), 'Combat']
                    : ['Combat'],
        }),
        getCombatPlayersByCombatId: builder.query({
            query: (combatId) => `/CombatPlayer/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatPlayer', id })), 'CombatPlayer']
                    : ['CombatPlayer'],
        }),
        getCombatPlayerById: builder.query({
            query: (id) => `/CombatPlayer/${id}`,
            providesTags: result =>
                result ? [{ type: 'CombatPlayer', id: result.id }] : ['CombatPlayer'],
        }),
        getCombatById: builder.query({
            query: (id) => `/Combat/${id}`,
            providesTags: result =>
                result ? [{ type: 'Combat', id: result.id }] : ['Combat'],
        }),
        getCombatPlayerPositionByCombatId: builder.query({
            query: (combatId) => `/CombatPlayerPosition/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatPlayerPosition', id })), 'CombatPlayerPosition']
                    : ['CombatPlayerPosition'],
        }),
        getCombatAurasByCombatId: builder.query({
            query: (combatId) => `/CombatAura/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatAura', id })), 'CombatAura']
                    : ['CombatAura'],
        }),
    })
})

export const {
    useGetCombatLogsQuery,
    useLazyGetPlayersDeathByPlayerIdQuery,
    useLazyGetCombatsByCombatLogIdQuery,
    useLazyGetCombatPlayersByCombatIdQuery,
    useLazyGetCombatPlayerByIdQuery,
    useLazyGetCombatByIdQuery,
    useLazyGetCombatPlayerPositionByCombatIdQuery,
    useLazyGetCombatAurasByCombatIdQuery,
} = CombatParserApi;