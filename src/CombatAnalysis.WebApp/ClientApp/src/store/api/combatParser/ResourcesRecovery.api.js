import { CombatParserApi } from "../core/CombatParser.api";

export const ResourcesRecoveryApi = CombatParserApi.injectEndpoints({
    tagTyes: [
        'ResourceRecovery',
        'ResourceRecoveryGeneral',
    ],
    endpoints: builder => ({
        getResourceRecoveryByPlayerId: builder.query({
            query: ({ combatPlayerId, page, pageSize }) => ({
                url: `/ResourceRecovery/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecovery', id })), 'ResourceRecovery']
                    : ['ResourceRecovery'],
        }),
        getResourceRecoveryCreatorByPlayerId: builder.query({
            query: ({ combatPlayerId, creator, page, pageSize }) => ({
                url: `/ResourceRecovery/getByCreator?combatPlayerId=${combatPlayerId}&creator=${creator}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecovery', id })), 'ResourceRecovery']
                    : ['ResourceRecovery'],
        }),
        getResourceRecoveryUniqueCreators: builder.query({
            query: (combatPlayerId) => ({
                url: `/ResourceRecovery/getUniqueCreators/${combatPlayerId}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecovery', id })), 'ResourceRecovery']
                    : ['ResourceRecovery'],
        }),
        getResourceRecoveryCountByPlayerId: builder.query({
            query: (combatPlayerId) => `/ResourceRecovery/count/${combatPlayerId}`,
        }),
        getResourceRecoveryCountCreatorByPlayerId: builder.query({
            query: ({ combatPlayerId, creator }) => `/ResourceRecovery/countByCreator?combatPlayerId=${combatPlayerId}&creator=${creator}`,
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
    useGetResourceRecoveryCreatorByPlayerIdQuery,
    useGetResourceRecoveryUniqueCreatorsQuery,
    useLazyGetResourceRecoveryCountByPlayerIdQuery,
    useGetResourceRecoveryCountCreatorByPlayerIdQuery,
    useGetResourceRecoveryGeneralyByPlayerIdQuery,
    useLazyGetResourceRecoveryGeneralyByPlayerIdQuery,
} = ResourcesRecoveryApi;