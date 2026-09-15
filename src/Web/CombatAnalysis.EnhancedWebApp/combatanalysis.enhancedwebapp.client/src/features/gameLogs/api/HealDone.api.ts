import type { ChartModel } from '../types/chart/ChartModel';
import type { HealDoneGeneralModel } from '../types/HealDoneGeneralModel';
import type { HealDoneModel } from '../types/HealDoneModel';
import { GameLogsApi } from './GameLogs.api';

export const HealDoneApi = GameLogsApi.injectEndpoints({
    endpoints: builder => ({
        getHealDoneByCombatPlayerId: builder.query<HealDoneModel[], { unitId: string, page: number, pageSize: number }>({
            query: ({ unitId, page, pageSize }) => `/HealDone/getByUnitId?unitId=${unitId}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(healDone => ({ type: 'HealDone' as const, id: healDone.id })),
                        { type: 'HealDone', id: 'LIST' },
                    ]
                    : [{ type: 'HealDone', id: 'LIST' }]
        }),
        countHealDone: builder.query<number, { unitId: string, target: string, creator: string, spell: string, from: string, to: string }>({
            query: ({ unitId, target, creator, spell, from, to }) => `/HealDone/count?unitId=${unitId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}`,
        }),
        getAllHealDone: builder.query<HealDoneModel[], { unitId: string, creator: string, target: string, spell: string, from: string, to: string, page: number, pageSize: number }>({
            query: ({ unitId, target, creator, spell, from, to, page, pageSize }) => `/HealDone/getAll?unitId=${unitId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(healDone => ({ type: 'HealDone' as const, id: healDone.id })),
                        { type: 'HealDone', id: 'LIST' },
                    ]
                    : [{ type: 'HealDone', id: 'LIST' }]
        }),
        getCombatPlayerChartHealDone: builder.query<ChartModel[], number>({
            query: combatPlayerId => `/HealDone/getCombatPlayerChart/${combatPlayerId}`
        }),
        getGenericChartHealDone: builder.query<Map<string, ChartModel[]>, number>({
            query: combatId => `/HealDone/getGenericChart/${combatId}`
        }),
        getHealDoneUniqueFilterValues: builder.query<string[], { unitId: string, filter: string }>({
            query: ({ unitId, filter }) => `/HealDone/getUniqueFilterValues?unitId=${unitId}&filter=${filter}`,
        }),
        getHealDoneGeneralByUnitId: builder.query<HealDoneGeneralModel[], string>({
            query: unitId => `/HealDoneGeneral/getByUnitId/${unitId}`,
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
    useGetCombatPlayerChartHealDoneQuery,
    useGetHealDoneGeneralByUnitIdQuery,
    useGetGenericChartHealDoneQuery,
    useLazyGetHealDoneGeneralByUnitIdQuery,
} = HealDoneApi;