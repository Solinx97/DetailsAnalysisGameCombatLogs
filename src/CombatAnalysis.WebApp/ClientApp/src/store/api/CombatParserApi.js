import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const CombatParserApi = createApi({
    reducerPath: 'combatParserAPi',
    tagTyes: [
        'MainInformation',
        'DamageDone',
        'GeneralAnalysis',
        'DetailsSpecificalCombat',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getCombatLogs: builder.query({
            query: () => '/MainInformation'
        }),
        getDamageDoneByPlayerId: builder.query({
            query: (playerId) => `/DamageDone/${playerId}`
        }),
        getHealDoneByPlayerId: builder.query({
            query: (playerId) => `/HealDone/${playerId}`
        }),
        getDamageTakenByPlayerId: builder.query({
            query: (playerId) => `/DamageTaken/${playerId}`
        }),
        getResourceRecoveryByPlayerId: builder.query({
            query: (playerId) => `/ResourceRecovery/${playerId}`
        }),
        getDamageDoneGeneralyByPlayerId: builder.query({
            query: (playerId) => `/DamageDoneGeneral/${playerId}`
        }),
        getHealDoneGeneralyByPlayerId: builder.query({
            query: (playerId) => `/HealDoneGeneral/${playerId}`
        }),
        getDamageTakenGeneralyByPlayerId: builder.query({
            query: (playerId) => `/DamageTakenGeneral/${playerId}`
        }),
        getResourceRecoveryGeneralyByPlayerId: builder.query({
            query: (playerId) => `/ResourceRecoveryGeneral/${playerId}`
        }),
        getPlayersDeathByPlayerId: builder.query({
            query: (playerId) => `/PlayerDeath/${playerId}`
        }),
        getGeneralAnalysisById: builder.query({
            query: (id) => `/GeneralAnalysis/${id}`
        }),
        getCombatPlayersByCombatId: builder.query({
            query: (id) => `/DetailsSpecificalCombat/combatPlayersByCombatId/${id}`
        }),
        getCombatPlayerId: builder.query({
            query: (id) => `/DetailsSpecificalCombat/combatPlayerById/${id}`
        }),
        getCombatById: builder.query({
            query: (id) => `/DetailsSpecificalCombat/combatById/${id}`
        }),
    })
})

export const {
    useGetCombatLogsQuery,
    useLazyGetDamageDoneByPlayerIdQuery,
    useLazyGetHealDoneByPlayerIdQuery,
    useLazyGetDamageTakenByPlayerIdQuery,
    useLazyGetResourceRecoveryByPlayerIdQuery,
    useLazyGetDamageDoneGeneralyByPlayerIdQuery,
    useLazyGetHealDoneGeneralyByPlayerIdQuery,
    useLazyGetDamageTakenGeneralyByPlayerIdQuery,
    useLazyGetResourceRecoveryGeneralyByPlayerIdQuery,
    useLazyGetPlayersDeathByPlayerIdQuery,
    useGetDamageDoneGeneralyByPlayerIdQuery,
    useGetGeneralAnalysisByIdQuery,
    useLazyGetGeneralAnalysisByIdQuery,
    useLazyGetCombatPlayersByCombatIdQuery,
    useGetCombatPlayerIdQuery,
    useLazyGetCombatPlayerIdQuery,
    useLazyGetCombatByIdQuery,
} = CombatParserApi;