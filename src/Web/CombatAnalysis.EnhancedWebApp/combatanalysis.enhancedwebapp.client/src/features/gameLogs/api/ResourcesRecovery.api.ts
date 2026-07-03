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
        countResourceRecovery: builder.query<number, { combatPlayerId: number, target: string, creator: string, spell: string, from: string, to: string }>({
            query: ({ combatPlayerId, target, creator, spell, from, to }) => `/ResourceRecovery/count?combatPlayerId=${combatPlayerId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}`,
        }),
        getAllResourceRecovery: builder.query<ResourceRecoveryModel[], { combatPlayerId: number, target: string, creator: string, spell: string, from: string, to: string, page: number, pageSize: number }>({
            query: ({ combatPlayerId, target, creator, spell, from, to, page, pageSize }) => `/ResourceRecovery/getAll?combatPlayerId=${combatPlayerId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(resourceRecoveryGeneral => ({ type: 'ResourceRecoveryGeneral' as const, id: resourceRecoveryGeneral.id })),
                        { type: 'ResourceRecoveryGeneral', id: 'LIST' },
                    ]
                    : [{ type: 'ResourceRecoveryGeneral', id: 'LIST' }]
        }),
        getResourceRecoveryUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: string }>({
            query: ({ combatPlayerId, filter }) => `/ResourceRecovery/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
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
    useCountResourceRecoveryQuery,
    useGetResourceRecoveryUniqueFilterValuesQuery,
    useGetAllResourceRecoveryQuery,
    useGetResourceRecoveryGeneralByCombatPlayerIdQuery,
    useLazyGetResourceRecoveryGeneralByCombatPlayerIdQuery,
} = ResourcesRecoveryApi;