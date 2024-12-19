import { CombatParserApi } from "../core/CombatParser.api";

export const HealDoneApi = CombatParserApi.injectEndpoints({
    tagTyes: [
        'HealDone',
        'HealDoneGeneral',
    ],
    endpoints: builder => ({
        getHealDoneByCombatPlayerId: builder.query({
            query: ({ combatPlayerId, page, pageSize }) => ({
                url: `/HealDone/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDone', id })), 'HealDone']
                    : ['HealDone'],
        }),
        getHealDoneCountByCombatPlayerId: builder.query({
            query: (combatPlayerId) => `/HealDone/count/${combatPlayerId}`,
        }),
        getHealDoneUniqueFilterValues: builder.query({
            query: ({ combatPlayerId, filter }) => ({
                url: `/HealDone/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDone', id })), 'HealDone']
                    : ['HealDone'],
        }),
        getHealDoneByFilter: builder.query({
            query: ({ combatPlayerId, filter, filterValue, page, pageSize }) => ({
                url: `/HealDone/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}&page=${page}&pageSize=${pageSize}`,
            }),
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDone', id })), 'HealDone']
                    : ['HealDone'],
        }),
        getHealDoneCountByFilter: builder.query({
            query: ({ combatPlayerId, filter, filterValue }) => `/HealDone/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&filterValue=${filterValue}`,
        }),
        getHealDoneGeneralByCombatPlayerId: builder.query({
            query: (combatPlayerId) => `/HealDoneGeneral/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'HealDoneGeneral', id })), 'HealDoneGeneral']
                    : ['HealDoneGeneral'],
        }),
    })
})

export const {
    useGetHealDoneByCombatPlayerIdQuery,
    useLazyGetHealDoneCountByCombatPlayerIdQuery,
    useGetHealDoneUniqueFilterValuesQuery,
    useGetHealDoneByFilterQuery,
    useGetHealDoneCountByFilterQuery,
    useGetHealDoneGeneralByCombatPlayerIdQuery,
    useLazyGetHealDoneGeneralByCombatPlayerIdQuery,
} = HealDoneApi;