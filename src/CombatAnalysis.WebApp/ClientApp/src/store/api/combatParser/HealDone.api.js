import { CombatParserApi } from "../core/CombatParser.api";

export const HealDoneApi = CombatParserApi.injectEndpoints({
    endpoints: builder => ({
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
        getHealDoneGeneralyByPlayerId: builder.query({
            query: (combatPlayerId) => `/HealDoneGeneral/findByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDoneGeneral', id })), 'HealDoneGeneral']
                    : ['HealDoneGeneral'],
        }),
    })
})

export const {
    useGetHealDoneByPlayerIdQuery,
    useLazyGetHealDoneCountByPlayerIdQuery,
    useGetHealDoneGeneralyByPlayerIdQuery,
    useLazyGetHealDoneGeneralyByPlayerIdQuery,
} = HealDoneApi;