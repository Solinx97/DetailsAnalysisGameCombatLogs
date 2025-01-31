import { CombatParserApi } from "../core/CombatParser.api";

export const DamageTakenApi = CombatParserApi.injectEndpoints({
    tagTyes: [
        'DamageTaken',
        'DamageTakenGeneral',
    ],
    endpoints: builder => ({
        getDamageTakenByCombatPlayerId: builder.query({
            query: ({ combatPlayerId, page, pageSize }) => ({
                url: `/DamageTaken/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTaken', id })), 'DamageTaken']
                    : ['DamageTaken'],
        }),
        getDamageTakenCountByCombatPlayerId: builder.query({
            query: (combatPlayerId) => `/DamageTaken/count/${combatPlayerId}`,
        }),
        getDamageTakenUniqueFilterValues: builder.query({
            query: ({ combatPlayerId, filter }) => ({
                url: `/DamageTaken/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTaken', id })), 'DamageTaken']
                    : ['DamageTaken'],
        }),
        getDamageTakenByFilter: builder.query({
            query: ({ combatPlayerId, filter, filterValue, page, pageSize }) => ({
                url: `/DamageTaken/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTaken', id })), 'DamageTaken']
                    : ['DamageTaken'],
        }),
        getDamageTakenCountByFilter: builder.query({
            query: ({ combatPlayerId, filter, filterValue }) => `/DamageTaken/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}`,
        }),
        getDamageTakenGeneralByCombatPlayerId: builder.query({
            query: (combatPlayerId) => `/DamageTakenGeneral/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageTakenGeneral', id })), 'DamageTakenGeneral']
                    : ['DamageTakenGeneral'],
        }),
    })
})

export const {
    useGetDamageTakenByCombatPlayerIdQuery,
    useLazyGetDamageTakenByCombatPlayerIdQuery,
    useLazyGetDamageTakenCountByCombatPlayerIdQuery,
    useGetDamageTakenUniqueFilterValuesQuery,
    useGetDamageTakenByFilterQuery,
    useGetDamageTakenCountByFilterQuery,
    useGetDamageTakenGeneralByCombatPlayerIdQuery,
    useLazyGetDamageTakenGeneralByCombatPlayerIdQuery,
} = DamageTakenApi;