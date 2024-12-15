import { CombatParserApi } from "../core/CombatParser.api";

export const ResourcesRecoveryApi = CombatParserApi.injectEndpoints({
    endpoints: builder => ({
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
        getResourceRecoveryGeneralyByPlayerId: builder.query({
            query: (combatPlayerId) => `/ResourceRecoveryGeneral/findByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecoveryGeneral', id })), 'ResourceRecoveryGeneral']
                    : ['ResourceRecoveryGeneral'],
        }),
    })
})

export const {
    useGetResourceRecoveryByPlayerIdQuery,
    useLazyGetResourceRecoveryCountByPlayerIdQuery,
    useLazyGetResourceRecoveryGeneralyByPlayerIdQuery,
} = ResourcesRecoveryApi;