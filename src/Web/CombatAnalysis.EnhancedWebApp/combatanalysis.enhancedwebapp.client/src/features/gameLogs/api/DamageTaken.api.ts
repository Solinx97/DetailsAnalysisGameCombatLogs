import type { ChartModel } from '../types/chart/ChartModel';
import type { DamageDoneModel } from '../types/DamageDoneModel';
import { GameLogsApi } from './GameLogs.api';
import type { DamageDoneGeneralModel } from '../types/DamageDoneGeneralModel';

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
        getCombatPlayerChartDamageTaken: builder.query<ChartModel[], string>({
            query: unitId => `/DamageTaken/getUnitChart/${unitId}`
        }),
        getDamageTakenUniqueFilterValues: builder.query<string[], { unitId: string, filter: string }>({
            query: ({ unitId, filter }) => `/DamageTaken/getUniqueFilterValues?unitId=${unitId}&filter=${filter}`,
        }),
        getDamageTakenGeneralByUnitId: builder.query<DamageDoneGeneralModel[], { unitId: string, combatId: number }>({
            query: ({ unitId, combatId }) => `/DamageDoneGeneral/getDamageTakenByUnitId/${unitId}?combatId=${combatId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageTaken => ({ type: 'DamageTaken' as const, id: damageTaken.id })),
                        { type: 'DamageTaken', id: 'LIST' },
                    ]
                    : [{ type: 'DamageTaken', id: 'LIST' }]
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
    useGetDamageTakenGeneralByUnitIdQuery,
    useLazyGetDamageTakenGeneralByUnitIdQuery,
} = DamageTakenApi;