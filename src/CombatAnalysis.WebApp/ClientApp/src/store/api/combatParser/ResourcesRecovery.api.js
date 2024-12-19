import { CombatParserApi } from "../core/CombatParser.api";

export const ResourcesRecoveryApi = CombatParserApi.injectEndpoints({
    tagTyes: [
        'ResourceRecovery',
        'ResourceRecoveryGeneral',
    ],
    endpoints: builder => ({
        getResourceRecoveryByCombatPlayerId: builder.query({
            query: ({ combatPlayerId, page, pageSize }) => ({
                url: `/ResourceRecovery/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecovery', id })), 'ResourceRecovery']
                    : ['ResourceRecovery'],
        }),
        getResourceRecoveryCountByCombatPlayerId: builder.query({
            query: (combatPlayerId) => `/ResourceRecovery/count/${combatPlayerId}`,
        }),
        getResourceRecoveryUniqueFilterValues: builder.query({
            query: ({ combatPlayerId, filter }) => ({
                url: `/ResourceRecovery/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecovery', id })), 'ResourceRecovery']
                    : ['ResourceRecovery'],
        }),
        getResourceRecoveryByFilter: builder.query({
            query: ({ combatPlayerId, filter, filterValue, page, pageSize }) => ({
                url: `/ResourceRecovery/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecovery', id })), 'ResourceRecovery']
                    : ['ResourceRecovery'],
        }),
        getResourceRecoveryCountByFilter: builder.query({
            query: ({ combatPlayerId, filter, filterValue }) => `/ResourceRecovery/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}`,
        }),
        getResourceRecoveryGeneralByCombatPlayerId: builder.query({
            query: (combatPlayerId) => `/ResourceRecoveryGeneral/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'ResourceRecoveryGeneral', id })), 'ResourceRecoveryGeneral']
                    : ['ResourceRecoveryGeneral'],
        }),
    })
})

export const {
    useGetResourceRecoveryByCombatPlayerIdQuery,
    useLazygetResourceRecoveryCountByCombatPlayerIdQuery,
    useGetResourceRecoveryUniqueFilterValuesQuery,
    useGetResourceRecoveryByFilterQuery,
    useGetResourceRecoveryCountByFilterQuery,
    useGetResourceRecoveryGeneralByCombatPlayerIdQuery,
    useLazyGetResourceRecoveryGeneralByCombatPlayerIdQuery,
} = ResourcesRecoveryApi;