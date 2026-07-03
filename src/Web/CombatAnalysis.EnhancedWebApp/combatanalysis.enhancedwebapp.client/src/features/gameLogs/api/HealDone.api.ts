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
        countHealDone: builder.query<number, { combatPlayerId: number, target: string, creator: string, spell: string, from: string, to: string }>({
            query: ({ combatPlayerId, target, creator, spell, from, to }) => `/HealDone/count?combatPlayerId=${combatPlayerId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}`,
        }),
        getAllHealDone: builder.query<HealDoneModel[], { combatPlayerId: number, creator: string, target: string, spell: string, from: string, to: string, page: number, pageSize: number }>({
            query: ({ combatPlayerId, target, creator, spell, from, to, page, pageSize }) => `/HealDone/getAll?combatPlayerId=${combatPlayerId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(healDone => ({ type: 'HealDone' as const, id: healDone.id })),
                        { type: 'HealDone', id: 'LIST' },
                    ]
                    : [{ type: 'HealDone', id: 'LIST' }]
        }),
        getHealDoneUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: string }>({
            query: ({ combatPlayerId, filter }) => `/HealDone/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
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
    useCountHealDoneQuery,
    useGetHealDoneUniqueFilterValuesQuery,
    useGetAllHealDoneQuery,
    useGetHealDoneGeneralByCombatPlayerIdQuery,
    useLazyGetHealDoneGeneralByCombatPlayerIdQuery,
} = HealDoneApi;