import type { DamageTakenGeneralModel } from '../types/DamageTakenGeneralModel';
import type { DamageTakenModel } from '../types/DamageTakenModel';
import { GameLogsApi } from './GameLogs.api';

export const DamageTakenApi = GameLogsApi.injectEndpoints({
    endpoints: builder => ({
        getDamageTakenByCombatPlayerId: builder.query<DamageTakenModel[], { combatPlayerId: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, page, pageSize }) => `/DamageTaken/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageTakenGeneral => ({ type: 'DamageTakenGeneral' as const, id: damageTakenGeneral.id })),
                        { type: 'DamageTakenGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'DamageTakenGeneral', id: 'LIST' }]
        }),
        getDamageTakenCountByCombatPlayerId: builder.query<number, number>({
            query: combatPlayerId => `/DamageTaken/count/${combatPlayerId}`,
        }),
        getDamageTakenUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: string }>({
            query: ({ combatPlayerId, filter }) => `/DamageTaken/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
        }),
        getDamageTakenByFilter: builder.query<DamageTakenModel[], { combatPlayerId: number, filter: string, creator: string, spell: string, page: number, pageSize: number }>({
            query: ({ combatPlayerId, filter, creator, spell, page, pageSize }) => `/DamageTaken/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&creator=${creator}&spell=${spell}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageTakenGeneral => ({ type: 'DamageTakenGeneral' as const, id: damageTakenGeneral.id })),
                        { type: 'DamageTakenGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'DamageTakenGeneral', id: 'LIST' }]
        }),
        getDamageTakenCountByFilter: builder.query<number, { combatPlayerId: number, filter: string, creator: string, spell: string }>({
            query: ({ combatPlayerId, filter, creator, spell })  => `/DamageTaken/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&creator=${creator}&spell=${spell}`,
        }),
        getDamageTakenGeneralByCombatPlayerId: builder.query<DamageTakenGeneralModel[], number>({
            query: combatPlayerId => `/DamageTakenGeneral/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageTakenGeneral => ({ type: 'DamageTakenGeneral' as const, id: damageTakenGeneral.id })),
                        { type: 'DamageTakenGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'DamageTakenGeneral', id: 'LIST' }]
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