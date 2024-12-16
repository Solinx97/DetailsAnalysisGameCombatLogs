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
        getDamageDoneTargetByPlayerId: builder.query({
            query: ({ combatPlayerId, target, page, pageSize }) => ({
                url: `/DamageDone/getByTarget?combatPlayerId=${combatPlayerId}&target=${target}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDone', id })), 'DamageDone']
                    : ['DamageDone'],
        }),
        getUniqueTargets: builder.query({
            query: (combatPlayerId) => ({
                url: `/DamageDone/getUniqueTargets/${combatPlayerId}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDone', id })), 'DamageDone']
                    : ['DamageDone'],
        }),
        getDamageDoneCountByPlayerId: builder.query({
            query: (combatPlayerId) => `/DamageDone/count/${combatPlayerId}`,
        }),
        getDamageDoneCountTargetsByPlayerId: builder.query({
            query: ({ combatPlayerId, target }) => `/DamageDone/countByTarget?combatPlayerId=${combatPlayerId}&target=${target}`,
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
    useGetDamageDoneTargetByPlayerIdQuery,
    useGetUniqueTargetsQuery,
    useLazyGetDamageDoneCountByPlayerIdQuery,
    useGetDamageDoneCountTargetsByPlayerIdQuery,
    useGetDamageDoneGeneralyByPlayerIdQuery,
    useLazyGetDamageDoneGeneralyByPlayerIdQuery,
} = DamageDoneApi;