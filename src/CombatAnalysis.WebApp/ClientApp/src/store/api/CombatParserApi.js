import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const CombatParserApi = createApi({
    reducerPath: 'combatParserAPi',
    tagTyes: [
        'CombatLog',
        'DamageDone',
        'HealDone',
        'DamageTaken',
        'ResourceRecovery',
        'DamageDoneGeneral',
        'HealDoneGeneral',
        'DamageTakenGeneral',
        'ResourceRecoveryGeneral',
        'PlayerDeath',
        'Combat',
        'CombatPlayer',
        'CombatPlayerPosition',
        'CombatAura',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    keepUnusedDataFor: 120,
    endpoints: builder => ({
        getCombatLogs: builder.query({
            query: () => '/CombatLog',
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatLog', id })), 'CombatLog']
                    : ['CombatLog'],
        }),
        getDamageDoneByPlayerId: builder.query({
            query: ({ combatPlayerId, page, pageSize }) => ({
                url: `/DamageDone/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDone', id })), 'DamageDone']
                    : ['DamageDone'],
        }),
        getDamageDoneCountByPlayerId: builder.query({
            query: (playerId) => `/DamageDone/count/${playerId}`,
        }),
        getHealDoneByPlayerId: builder.query({
            query: (arg) => {
                const { combatPlayerId, page, pageSize } = arg;
                return {
                    url: `/HealDone/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
                }
            },
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDone', id })), 'HealDone']
                    : ['HealDone'],
        }),
        getHealDoneCountByPlayerId: builder.query({
            query: (playerId) => `/HealDone/count/${playerId}`,
        }),
        getDamageTakenByPlayerId: builder.query({
            query: (arg) => {
                const { combatPlayerId, page, pageSize } = arg;
                return {
                    url: `/DamageTaken/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
                }
            },
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTaken', id })), 'DamageTaken']
                    : ['DamageTaken'],
        }),
        getDamageTakenCountByPlayerId: builder.query({
            query: (playerId) => `/DamageTaken/count/${playerId}`,
        }),
        getResourceRecoveryByPlayerId: builder.query({
            query: (arg) => {
                const { combatPlayerId, page, pageSize } = arg;
                return {
                    url: `/ResourceRecovery/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
                }
            },
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecovery', id })), 'ResourceRecovery']
                    : ['ResourceRecovery'],
        }),
        getResourceRecoveryCountByPlayerId: builder.query({
            query: (playerId) => `/ResourceRecovery/count/${playerId}`,
        }),
        getDamageDoneGeneralyByPlayerId: builder.query({
            query: (combatPlayerId) => `/DamageDoneGeneral/findByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDoneGeneral', id })), 'DamageDoneGeneral']
                    : ['DamageDoneGeneral'],
        }),
        getHealDoneGeneralyByPlayerId: builder.query({
            query: (combatPlayerId) => `/HealDoneGeneral/findByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDoneGeneral', id })), 'HealDoneGeneral']
                    : ['HealDoneGeneral'],
        }),
        getDamageTakenGeneralyByPlayerId: builder.query({
            query: (combatPlayerId) => `/DamageTakenGeneral/findByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTakenGeneral', id })), 'DamageTakenGeneral']
                    : ['DamageTakenGeneral'],
        }),
        getResourceRecoveryGeneralyByPlayerId: builder.query({
            query: (combatPlayerId) => `/ResourceRecoveryGeneral/findByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecoveryGeneral', id })), 'ResourceRecoveryGeneral']
                    : ['ResourceRecoveryGeneral'],
        }),
        getPlayersDeathByPlayerId: builder.query({
            query: (combatPlayerId) => `/PlayerDeath/findByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'PlayerDeath', id })), 'PlayerDeath']
                    : ['PlayerDeath'],
        }),
        getCombatsByCombatLogId: builder.query({
            query: (combatLogId) => `/Combat/findByCombatLogId/${combatLogId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Combat', id })), 'Combat']
                    : ['Combat'],
        }),
        getCombatPlayersByCombatId: builder.query({
            query: (combatId) => `/CombatPlayer/findByCombatId/${combatId}`,
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
            query: (combatId) => `/CombatPlayerPosition/findByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatPlayerPosition', id })), 'CombatPlayerPosition']
                    : ['CombatPlayerPosition'],
        }),
        getCombatAurasByCombatId: builder.query({
            query: (combatId) => `/CombatAura/findByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CombatAura', id })), 'CombatAura']
                    : ['CombatAura'],
        }),
    })
})

export const {
    useGetCombatLogsQuery,
    useGetDamageDoneByPlayerIdQuery,
    useLazyGetDamageDoneCountByPlayerIdQuery,
    useGetHealDoneByPlayerIdQuery,
    useLazyGetHealDoneCountByPlayerIdQuery,
    useGetDamageTakenByPlayerIdQuery,
    useLazyGetDamageTakenByPlayerIdQuery,
    useLazyGetDamageTakenCountByPlayerIdQuery,
    useGetResourceRecoveryByPlayerIdQuery,
    useLazyGetResourceRecoveryCountByPlayerIdQuery,
    useLazyGetDamageDoneGeneralyByPlayerIdQuery,
    useLazyGetHealDoneGeneralyByPlayerIdQuery,
    useLazyGetDamageTakenGeneralyByPlayerIdQuery,
    useLazyGetResourceRecoveryGeneralyByPlayerIdQuery,
    useLazyGetPlayersDeathByPlayerIdQuery,
    useGetDamageDoneGeneralyByPlayerIdQuery,
    useLazyGetCombatsByCombatLogIdQuery,
    useLazyGetCombatPlayersByCombatIdQuery,
    useLazyGetCombatPlayerByIdQuery,
    useLazyGetCombatByIdQuery,
    useLazyGetCombatPlayerPositionByCombatIdQuery,
    useLazyGetCombatAurasByCombatIdQuery,
} = CombatParserApi;