import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const CombatParserApi = createApi({
    reducerPath: 'combatParserAPi',
    tagTyes: [
        'MainInformation',
        'DamageDone',
        'GeneralAnalysis',
        'DetailsSpecificalCombat',
        'CombatPlayerPosition',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getCombatLogs: builder.query({
            query: () => '/MainInformation'
        }),
        getDamageDoneByPlayerId: builder.query({
            query: (arg) => {
                const { combatPlayerId, page, pageSize } = arg;
                return {
                    url: `/DamageDone/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
                }
            }
        }),
        getDamageDoneCountByPlayerId: builder.query({
            query: (playerId) => `/DamageDone/count/${playerId}`
        }),
        getHealDoneByPlayerId: builder.query({
            query: (arg) => {
                const { combatPlayerId, page, pageSize } = arg;
                return {
                    url: `/HealDone/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
                }
            }
        }),
        getHealDoneCountByPlayerId: builder.query({
            query: (playerId) => `/HealDone/count/${playerId}`
        }),
        getDamageTakenByPlayerId: builder.query({
            query: (arg) => {
                const { combatPlayerId, page, pageSize } = arg;
                return {
                    url: `/DamageTaken/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
                }
            }
        }),
        getDamageTakenCountByPlayerId: builder.query({
            query: (playerId) => `/DamageTaken/count/${playerId}`
        }),
        getResourceRecoveryByPlayerId: builder.query({
            query: (arg) => {
                const { combatPlayerId, page, pageSize } = arg;
                return {
                    url: `/ResourceRecovery/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
                }
            }
        }),
        getResourceRecoveryCountByPlayerId: builder.query({
            query: (playerId) => `/ResourceRecovery/count/${playerId}`
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
            query: (id) => `/GeneralAnalysis/findByCombatLogId/${id}`
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
        getCombatPlayerPositionByCombatId: builder.query({
            query: (id) => `/CombatPlayerPosition/findByCombatId/${id}`
        }),
    })
})

export const {
    useGetCombatLogsQuery,
    useLazyGetDamageDoneByPlayerIdQuery,
    useLazyGetDamageDoneCountByPlayerIdQuery,
    useLazyGetHealDoneByPlayerIdQuery,
    useLazyGetHealDoneCountByPlayerIdQuery,
    useLazyGetDamageTakenByPlayerIdQuery,
    useLazyGetDamageTakenCountByPlayerIdQuery,
    useLazyGetResourceRecoveryByPlayerIdQuery,
    useLazyGetResourceRecoveryCountByPlayerIdQuery,
    useLazyGetDamageDoneGeneralyByPlayerIdQuery,
    useLazyGetHealDoneGeneralyByPlayerIdQuery,
    useLazyGetDamageTakenGeneralyByPlayerIdQuery,
    useLazyGetResourceRecoveryGeneralyByPlayerIdQuery,
    useLazyGetPlayersDeathByPlayerIdQuery,
    useGetDamageDoneGeneralyByPlayerIdQuery,
    useLazyGetCombatById,
    useGetGeneralAnalysisByIdQuery,
    useLazyGetGeneralAnalysisByIdQuery,
    useLazyGetCombatPlayersByCombatIdQuery,
    useGetCombatPlayerIdQuery,
    useLazyGetCombatPlayerIdQuery,
    useLazyGetCombatByIdQuery,
    useLazyGetCombatPlayerPositionByCombatIdQuery,
} = CombatParserApi;