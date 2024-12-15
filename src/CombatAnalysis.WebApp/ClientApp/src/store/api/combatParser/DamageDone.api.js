import { CombatParserApi } from "../core/CombatParser.api";

export const DamageDoneApi = CombatParserApi.injectEndpoints({
    tagTyes: [
        'DamageDone',
        'DamageDoneGeneral',
    ],
    endpoints: builder => ({
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
        getDamageDoneGeneralyByPlayerId: builder.query({
            query: (combatPlayerId) => `/DamageDoneGeneral/findByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDoneGeneral', id })), 'DamageDoneGeneral']
                    : ['DamageDoneGeneral'],
        }),
    })
})

export const {
    useGetDamageDoneByPlayerIdQuery,
    useLazyGetDamageDoneCountByPlayerIdQuery,
    useGetDamageDoneGeneralyByPlayerIdQuery,
    useLazyGetDamageDoneGeneralyByPlayerIdQuery,
} = DamageDoneApi;