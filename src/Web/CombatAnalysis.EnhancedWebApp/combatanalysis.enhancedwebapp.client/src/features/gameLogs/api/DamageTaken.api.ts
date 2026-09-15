import type { ChartModel } from '../types/chart/ChartModel';
import type { DamageTakenGeneralModel } from '../types/DamageTakenGeneralModel';
import type { DamageDoneModel } from '../types/DamageDoneModel';
import { GameLogsApi } from './GameLogs.api';

export const DamageTakenApi = GameLogsApi.injectEndpoints({
    endpoints: builder => ({
        getDamageTakenByCombatPlayerId: builder.query<DamageDoneModel[], { unitId: string, page: number, pageSize: number }>({
            query: ({ unitId, page, pageSize }) => `/DamageTaken/getByCombatPlayerId?unitId=${unitId}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageTakenGeneral => ({ type: 'DamageTakenGeneral' as const, id: damageTakenGeneral.id })),
                        { type: 'DamageTakenGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'DamageTakenGeneral', id: 'LIST' }]
        }),
        countDamageTaken: builder.query<number, { unitId: string, target: string, creator: string, spell: string, from: string, to: string }>({
            query: ({ unitId, target, creator, spell, from, to }) => `/DamageTaken/count?unitId=${unitId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}`,
        }),
        getAllDamageTaken: builder.query<DamageDoneModel[], { unitId: string, target: string, creator: string, spell: string, from: string, to: string, page: number, pageSize: number }>({
            query: ({ unitId, target, creator, spell, from, to, page, pageSize }) => `/DamageTaken/getAll?unitId=${unitId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageTakenGeneral => ({ type: 'DamageTakenGeneral' as const, id: damageTakenGeneral.id })),
                        { type: 'DamageTakenGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'DamageTakenGeneral', id: 'LIST' }]
        }),
        getCombatPlayerChartDamageTaken: builder.query<ChartModel[], number>({
            query: combatPlayerId => `/DamageTaken/getCombatPlayerChart/${combatPlayerId}`
        }),
        getDamageTakenUniqueFilterValues: builder.query<string[], { unitId: string, filter: string }>({
            query: ({ unitId, filter }) => `/DamageTaken/getUniqueFilterValues?unitId=${unitId}&filter=${filter}`,
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
    useGetCombatPlayerChartDamageTakenQuery,
    useGetDamageTakenGeneralByCombatPlayerIdQuery,
    useLazyGetDamageTakenGeneralByCombatPlayerIdQuery,
} = DamageTakenApi;