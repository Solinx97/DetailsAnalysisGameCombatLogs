import type { HealDoneGeneralModel } from '../types/HealDoneGeneralModel';
import type { HealDoneModel } from '../types/HealDoneModel';
import { GameLogsApi } from './GameLogs.api';

export const HealDoneApi = GameLogsApi.injectEndpoints({
    endpoints: builder => ({
        getHealDoneByCombatPlayerId: builder.query<HealDoneModel[], { combatPlayerId: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, page, pageSize }) => `/HealDone/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(healDone => ({ type: 'HealDone' as const, id: healDone.id })),
                        { type: 'HealDone', id: 'LIST' },
                    ]
                    : [{ type: 'HealDone', id: 'LIST' }]
        }),
        getHealDoneCountByCombatPlayerId: builder.query<number, number>({
            query: combatPlayerId => `/HealDone/count/${combatPlayerId}`,
        }),
        getHealDoneUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: string }>({
            query: ({ combatPlayerId, filter }) => `/HealDone/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
        }),
        getHealDoneByFilter: builder.query<HealDoneModel[], { combatPlayerId: number, filter: string, target: string, spell: string, page: number, pageSize: number }>({
            query: ({ combatPlayerId, filter, target, spell, page, pageSize }) => `/HealDone/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&target=${target}&spell=${spell}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(healDone => ({ type: 'HealDone' as const, id: healDone.id })),
                        { type: 'HealDone', id: 'LIST' },
                    ]
                    : [{ type: 'HealDone', id: 'LIST' }]
        }),
        getHealDoneCountByFilter: builder.query<number, { combatPlayerId: number, filter: string, target: string, spell: string }>({
            query: ({ combatPlayerId, filter, target, spell }) => `/HealDone/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&target=${target}&spell=${spell}`,
        }),
        getHealDoneGeneralByCombatPlayerId: builder.query<HealDoneGeneralModel[], number>({
            query: combatPlayerId => `/HealDoneGeneral/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(healDone => ({ type: 'HealDone' as const, id: healDone.id })),
                        { type: 'HealDone', id: 'LIST' },
                    ]
                    : [{ type: 'HealDone', id: 'LIST' }]
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