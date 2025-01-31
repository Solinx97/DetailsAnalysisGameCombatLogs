import { CombatParserApi } from "../core/CombatParser.api";

export const DamageDoneApi = CombatParserApi.injectEndpoints({
    tagTyes: [
        'DamageDone',
        'DamageDoneGeneral',
    ],
    endpoints: builder => ({
        getDamageDoneByCombatPlayerId: builder.query({
            query: ({ combatPlayerId, page, pageSize }) => ({
                url: `/DamageDone/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDone', id })), 'DamageDone']
                    : ['DamageDone'],
        }),
        getDamageDoneCountByCombatPlayerId: builder.query({
            query: (combatPlayerId) => `/DamageDone/count/${combatPlayerId}`,
        }),
        getDamageDoneUniqueFilterValues: builder.query({
            query: ({ combatPlayerId, filter }) => ({
                url: `/DamageDone/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDone', id })), 'DamageDone']
                    : ['DamageDone'],
        }),
        getDamageDoneByFilter: builder.query({
            query: ({ combatPlayerId, filter, filterValue, page, pageSize }) => ({
                url: `/DamageDone/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDone', id })), 'DamageDone']
                    : ['DamageDone'],
        }),
        getDamageDoneCountByFilter: builder.query({
            query: ({ combatPlayerId, filter, filterValue }) => `/DamageDone/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}`,
        }),
        getDamageDoneGeneralByCombatPlayerId: builder.query({
            query: (combatPlayerId) => `/DamageDoneGeneral/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'DamageDoneGeneral', id })), 'DamageDoneGeneral']
                    : ['DamageDoneGeneral'],
        }),
    })
})

export const {
    useGetDamageDoneByCombatPlayerIdQuery,
    useLazyGetDamageDoneCountByCombatPlayerIdQuery,
    useGetDamageDoneUniqueFilterValuesQuery,
    useGetDamageDoneByFilterQuery,
    useGetDamageDoneCountByFilterQuery,
    useGetDamageDoneGeneralByCombatPlayerIdQuery,
    useLazyGetDamageDoneGeneralByCombatPlayerIdQuery,
} = DamageDoneApi;