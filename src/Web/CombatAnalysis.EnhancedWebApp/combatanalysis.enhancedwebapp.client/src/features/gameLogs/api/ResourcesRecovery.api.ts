import type { ResourceRecoveryGeneralModel } from '../types/ResourceRecoveryGeneralModel';
import type { ResourceRecoveryModel } from '../types/ResourceRecoveryModel';
import { GameLogsApi } from './GameLogs.api';

export const ResourcesRecoveryApi = GameLogsApi.injectEndpoints({
    endpoints: builder => ({
        getResourceRecoveryByCombatPlayerId: builder.query<ResourceRecoveryModel[], { combatPlayerId: number, page: number, pageSize: number }>({
            query: ({ combatPlayerId, page, pageSize }) => `/ResourceRecovery/getByCombatPlayerId?combatPlayerId=${combatPlayerId}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(resourceRecoveryGeneral => ({ type: 'ResourceRecoveryGeneral' as const, id: resourceRecoveryGeneral.id })),
                        { type: 'ResourceRecoveryGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'ResourceRecoveryGeneral', id: 'LIST' }]
        }),
        getResourceRecoveryCountByCombatPlayerId: builder.query<number, number>({
            query: combatPlayerId => `/ResourceRecovery/count/${combatPlayerId}`,
        }),
        getResourceRecoveryUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: string }>({
            query: ({ combatPlayerId, filter }) => `/ResourceRecovery/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
        }),
        getResourceRecoveryByFilter: builder.query<ResourceRecoveryModel[], { combatPlayerId: number, filter: string, creator: string, spell: string, page: number, pageSize: number }>({
            query: ({ combatPlayerId, filter, creator, spell, page, pageSize }) => `/ResourceRecovery/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&creator=${creator}&spell=${spell}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(resourceRecoveryGeneral => ({ type: 'ResourceRecoveryGeneral' as const, id: resourceRecoveryGeneral.id })),
                        { type: 'ResourceRecoveryGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'ResourceRecoveryGeneral', id: 'LIST' }]
        }),
        getResourceRecoveryCountByFilter: builder.query<number, { combatPlayerId: number, filter: string, creator: string, spell: string }>({
            query: ({ combatPlayerId, filter, creator, spell }) => `/ResourceRecovery/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&creator=${creator}&spell=${spell}`,
        }),
        getResourceRecoveryGeneralByCombatPlayerId: builder.query<ResourceRecoveryGeneralModel[], number>({
            query: combatPlayerId => `/ResourceRecoveryGeneral/getByCombatPlayerId/${combatPlayerId}`,
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
    useLazyGetResourceRecoveryCountByCombatPlayerIdQuery,
    useGetResourceRecoveryUniqueFilterValuesQuery,
    useGetResourceRecoveryByFilterQuery,
    useGetResourceRecoveryCountByFilterQuery,
    useGetResourceRecoveryGeneralByCombatPlayerIdQuery,
    useLazyGetResourceRecoveryGeneralByCombatPlayerIdQuery,
} = ResourcesRecoveryApi;