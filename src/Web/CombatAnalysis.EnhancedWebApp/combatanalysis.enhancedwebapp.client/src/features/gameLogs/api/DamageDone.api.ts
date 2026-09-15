import type { DamageDoneGeneralModel } from '../types/DamageDoneGeneralModel';
import type { DamageDoneModel } from '../types/DamageDoneModel';
import { GameLogsApi } from './GameLogs.api';
import type { ChartModel } from '../types/chart/ChartModel';

export const DamageDoneApi = GameLogsApi.injectEndpoints({
    endpoints: builder => ({
        getDamageDoneByCombatPlayerId: builder.query<DamageDoneModel[], { combatPlayerId: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, page, pageSize }) => `/DamageDone/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageDone => ({ type: 'DamageDone' as const, id: damageDone.id })),
                        { type: 'DamageDone', id: 'LIST' },
                    ]
                    : [{ type: 'DamageDone', id: 'LIST' }]
        }),
        countDamageDone: builder.query<number, { unitId: string, target: string, creator: string, spell: string, from: string, to: string }>({
            query: ({ unitId, target, creator, spell, from, to }) => `/DamageDone/count?unitId=${unitId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}`,
        }),
        getAllDamageDone: builder.query<DamageDoneModel[], { unitId: string, creator: string, target: string, spell: string, from: string, to: string, page: number, pageSize: number }>({
            query: ({ unitId, target, creator, spell, from, to, page, pageSize }) => `/DamageDone/getAll?unitId=${unitId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageDone => ({ type: 'DamageDone' as const, id: damageDone.id })),
                        { type: 'DamageDone', id: 'LIST' },
                    ]
                    : [{ type: 'DamageDone', id: 'LIST' }]
        }),
        getCombatPlayerChartDamageDone: builder.query<ChartModel[], number>({
            query: combatPlayerId => `/DamageDone/getCombatPlayerChart/${combatPlayerId}`
        }),
        getGenericChartDamageDone: builder.query<Map<string, ChartModel[]>, number>({
            query: combatId => `/DamageDone/getGenericChart/${combatId}`
        }),
        getDamageDoneUniqueFilterValues: builder.query<string[], { unitId: string, filter: string }>({
            query: ({ unitId: combatPlayerId, filter }) => `/DamageDone/getUniqueFilterValues?unitId=${combatPlayerId}&filter=${filter}`,
        }),
        getDamageDoneGeneralByUnitId: builder.query<DamageDoneGeneralModel[], { unitId: string, combatId: number }>({
            query: ({ unitId, combatId }) => `/DamageDoneGeneral/getByUnitId/${unitId}?combatId=${combatId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageDone => ({ type: 'DamageDone' as const, id: damageDone.id })),
                        { type: 'DamageDone', id: 'LIST' },
                    ]
                    : [{ type: 'DamageDone', id: 'LIST' }]
        }),
    })
})

export const {
    useGetDamageDoneByCombatPlayerIdQuery,
    useCountDamageDoneQuery,
    useLazyGetDamageDoneUniqueFilterValuesQuery,
    useGetDamageDoneUniqueFilterValuesQuery,
    useGetAllDamageDoneQuery,
    useGetCombatPlayerChartDamageDoneQuery,
    useLazyGetCombatPlayerChartDamageDoneQuery,
    useGetGenericChartDamageDoneQuery,
    useGetDamageDoneGeneralByUnitIdQuery,
    useLazyGetDamageDoneGeneralByUnitIdQuery,
} = DamageDoneApi;