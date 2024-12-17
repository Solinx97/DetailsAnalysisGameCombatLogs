import { CombatParserApi } from "../core/CombatParser.api";

export const HealDoneApi = CombatParserApi.injectEndpoints({
    tagTyes: [
        'HealDone',
        'HealDoneGeneral',
    ],
    endpoints: builder => ({
        getHealDoneByPlayerId: builder.query({
            query: ({ combatPlayerId, page, pageSize }) => ({
                url: `/HealDone/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDone', id })), 'HealDone']
                    : ['HealDone'],
        }),
        getHealDoneTargetByPlayerId: builder.query({
            query: ({ combatPlayerId, target, page, pageSize }) => ({
                url: `/HealDone/getByTarget?combatPlayerId=${combatPlayerId}&target=${target}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDone', id })), 'HealDone']
                    : ['HealDone'],
        }),
        getHealDoneUniqueTargets: builder.query({
            query: (combatPlayerId) => ({
                url: `/HealDone/getUniqueTargets/${combatPlayerId}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDone', id })), 'HealDone']
                    : ['HealDone'],
        }),
        getHealDoneCountByPlayerId: builder.query({
            query: (combatPlayerId) => `/HealDone/count/${combatPlayerId}`,
        }),
        getHealDoneCountTargetsByPlayerId: builder.query({
            query: ({ combatPlayerId, target }) => `/HealDone/countByTarget?combatPlayerId=${combatPlayerId}&target=${target}`,
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
    useGetHealDoneTargetByPlayerIdQuery,
    useGetHealDoneUniqueTargetsQuery,
    useLazyGetHealDoneCountByPlayerIdQuery,
    useGetHealDoneCountTargetsByPlayerIdQuery,
    useGetHealDoneGeneralyByPlayerIdQuery,
    useLazyGetHealDoneGeneralyByPlayerIdQuery,
} = HealDoneApi;