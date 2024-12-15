import { CombatParserApi } from "../core/CombatParser.api";

export const DamageTakenApi = CombatParserApi.injectEndpoints({
    endpoints: builder => ({
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
        getDamageTakenGeneralyByPlayerId: builder.query({
            query: (combatPlayerId) => `/DamageTakenGeneral/findByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTakenGeneral', id })), 'DamageTakenGeneral']
                    : ['DamageTakenGeneral'],
        }),
    })
})

export const {
    useGetDamageTakenByPlayerIdQuery,
    useLazyGetDamageTakenByPlayerIdQuery,
    useLazyGetDamageTakenCountByPlayerIdQuery,
    useLazyGetDamageTakenGeneralyByPlayerIdQuery,
} = DamageTakenApi;