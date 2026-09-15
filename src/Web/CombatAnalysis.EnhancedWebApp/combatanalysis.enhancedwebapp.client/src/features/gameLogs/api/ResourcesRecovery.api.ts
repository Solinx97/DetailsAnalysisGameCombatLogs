import type { ChartModel } from '../types/chart/ChartModel';
import type { ResourceRecoveryGeneralModel } from '../types/ResourceRecoveryGeneralModel';
import type { ResourceRecoveryModel } from '../types/ResourceRecoveryModel';
import { GameLogsApi } from './GameLogs.api';

export const ResourcesRecoveryApi = GameLogsApi.injectEndpoints({
    endpoints: builder => ({
        getResourceRecoveryByCombatPlayerId: builder.query<ResourceRecoveryModel[], { unitId: string, page: number, pageSize: number }>({
            query: ({ unitId, page, pageSize }) => `/ResourceRecovery/getByCombatPlayerId?unitId=${unitId}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(resourceRecoveryGeneral => ({ type: 'ResourceRecoveryGeneral' as const, id: resourceRecoveryGeneral.id })),
                        { type: 'ResourceRecoveryGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'ResourceRecoveryGeneral', id: 'LIST' }]
        }),
        countResourceRecovery: builder.query<number, { unitId: string, target: string, creator: string, spell: string, from: string, to: string }>({
            query: ({ unitId, target, creator, spell, from, to }) => `/ResourceRecovery/count?unitId=${unitId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}`,
        }),
        getAllResourceRecovery: builder.query<ResourceRecoveryModel[], { unitId: string, target: string, creator: string, spell: string, from: string, to: string, page: number, pageSize: number }>({
            query: ({ unitId, target, creator, spell, from, to, page, pageSize }) => `/ResourceRecovery/getAll?unitId=${unitId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(resourceRecoveryGeneral => ({ type: 'ResourceRecoveryGeneral' as const, id: resourceRecoveryGeneral.id })),
                        { type: 'ResourceRecoveryGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'ResourceRecoveryGeneral', id: 'LIST' }]
        }),
        getCombatPlayerChartResourceRecovery: builder.query<ChartModel[], number>({
            query: combatPlayerId => `/ResourceRecovery/getCombatPlayerChart/${combatPlayerId}`
        }),
        getResourceRecoveryUniqueFilterValues: builder.query<string[], { unitId: string, filter: string }>({
            query: ({ unitId, filter }) => `/ResourceRecovery/getUniqueFilterValues?unitId=${unitId}&filter=${filter}`,
        }),
        getResourceRecoveryGeneralByUnitId: builder.query<ResourceRecoveryGeneralModel[], string>({
            query: unitId => `/ResourceRecoveryGeneral/getByUnitId/${unitId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(resourceRecoveryGeneral => ({ type: 'ResourceRecoveryGeneral' as const, id: resourceRecoveryGeneral.id })),
                        { type: 'ResourceRecoveryGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'ResourceRecoveryGeneral', id: 'LIST' }]
        }),
    })
})

export const {
    useGetResourceRecoveryByCombatPlayerIdQuery,
    useCountResourceRecoveryQuery,
    useGetResourceRecoveryUniqueFilterValuesQuery,
    useGetAllResourceRecoveryQuery,
    useGetCombatPlayerChartResourceRecoveryQuery,
    useGetResourceRecoveryGeneralByUnitIdQuery,
    useLazyGetResourceRecoveryGeneralByUnitIdQuery,
} = ResourcesRecoveryApi;