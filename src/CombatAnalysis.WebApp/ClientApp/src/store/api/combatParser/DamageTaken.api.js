import { CombatParserApi } from "../core/CombatParser.api";

export const DamageTakenApi = CombatParserApi.injectEndpoints({
    tagTyes: [
        'DamageTaken',
        'DamageTakenGeneral',
    ],
    endpoints: builder => ({
        getDamageTakenByPlayerId: builder.query({
            query: ({ combatPlayerId, page, pageSize }) => ({
                url: `/DamageTaken/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTaken', id })), 'DamageTaken']
                    : ['DamageTaken'],
        }),
        getDamageTakenCreatorByPlayerId: builder.query({
            query: ({ combatPlayerId, creator, page, pageSize }) => ({
                url: `/DamageTaken/getByCreator?combatPlayerId=${combatPlayerId}&creator=${creator}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTaken', id })), 'DamageTaken']
                    : ['DamageTaken'],
        }),
        getDamageTakenUniqueCreators: builder.query({
            query: (combatPlayerId) => ({
                url: `/DamageTaken/getUniqueCreators/${combatPlayerId}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTaken', id })), 'DamageTaken']
                    : ['DamageTaken'],
        }),
        getDamageTakenCountByPlayerId: builder.query({
            query: (combatPlayerId) => `/DamageTaken/count/${combatPlayerId}`,
        }),
        getDamageTakenCountCreatorByPlayerId: builder.query({
            query: ({ combatPlayerId, creator }) => `/DamageTaken/countByCreator?combatPlayerId=${combatPlayerId}&creator=${creator}`,
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
    useLazyGetDamageTakenByPlayerIdQuery,
    useGetDamageTakenByPlayerIdQuery,
    useGetDamageTakenCreatorByPlayerIdQuery,
    useGetDamageTakenUniqueCreatorsQuery,
    useLazyGetDamageTakenCountByPlayerIdQuery,
    useGetDamageTakenCountCreatorByPlayerIdQuery,
    useGetDamageTakenGeneralyByPlayerIdQuery,
    useLazyGetDamageTakenGeneralyByPlayerIdQuery,
} = DamageTakenApi;