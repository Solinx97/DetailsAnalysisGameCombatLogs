import type { ChartModel } from '../types/chart/ChartModel';
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
        countDamageTaken: builder.query<number, { combatPlayerId: number, target: string, creator: string, spell: string, from: string, to: string }>({
            query: ({ combatPlayerId, target, creator, spell, from, to }) => `/DamageTaken/count?combatPlayerId=${combatPlayerId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}`,
        }),
        getAllDamageTaken: builder.query<DamageTakenModel[], { combatPlayerId: number, target: string, creator: string, spell: string, from: string, to: string, page: number, pageSize: number }>({
            query: ({ combatPlayerId, target, creator, spell, from, to, page, pageSize }) => `/DamageTaken/getAll?combatPlayerId=${combatPlayerId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageTakenGeneral => ({ type: 'DamageTakenGeneral' as const, id: damageTakenGeneral.id })),
                        { type: 'DamageTakenGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'DamageTakenGeneral', id: 'LIST' }]
        }),
        getChartDamageTaken: builder.query<ChartModel[], number>({
            query: combatPlayerId => `/DamageTaken/getChart/${combatPlayerId}`
        }),
        getDamageTakenUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: string }>({
            query: ({ combatPlayerId, filter }) => `/DamageTaken/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
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
    useCountDamageTakenQuery,
    useGetDamageTakenUniqueFilterValuesQuery,
    useGetAllDamageTakenQuery,
    useGetChartDamageTakenQuery,
    useGetDamageTakenGeneralByCombatPlayerIdQuery,
    useLazyGetDamageTakenGeneralByCombatPlayerIdQuery,
} = DamageTakenApi;