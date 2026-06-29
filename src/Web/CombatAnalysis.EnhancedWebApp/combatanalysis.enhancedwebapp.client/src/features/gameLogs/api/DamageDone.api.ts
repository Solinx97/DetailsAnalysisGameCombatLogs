import type { DamageDoneGeneralModel } from '../types/DamageDoneGeneralModel';
import type { DamageDoneModel } from '../types/DamageDoneModel';
import type { CombatTargetModel } from '../types/CombatTargetModel';
import { GameLogsApi } from './GameLogs.api';

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
        getDamageDoneCountByCombatPlayerId: builder.query<number, number>({
            query: combatPlayerId => `/DamageDone/count/${combatPlayerId}`,
        }),
        getDamageDoneUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: string }>({
            query: ({ combatPlayerId, filter }) => `/DamageDone/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
        }),
        getDamageDoneByFilter: builder.query<DamageDoneModel[], { combatPlayerId: number, filter: string, target: string, spell: string, page: number, pageSize: number }>({
            query: ({ combatPlayerId, filter, target, spell, page, pageSize }) => `/DamageDone/getByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&target=${target}&spell=${spell}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageDone => ({ type: 'DamageDone' as const, id: damageDone.id })),
                        { type: 'DamageDone', id: 'LIST' },
                    ]
                    : [{ type: 'DamageDone', id: 'LIST' }]
        }),
        getDamageDoneValueByTarget: builder.query<number, { combatPlayerId: number, target: string }>({
            query: ({ combatPlayerId, target }) => `/DamageDone/getValueByTarget?combatPlayerId=${combatPlayerId}&target=${target}`,
        }),
        getDamageDoneDamageByEachTarget: builder.query<Array<CombatTargetModel[]>, number>({
            query: combatId => `/DamageDone/getDamageByEachTarget/${combatId}`,
        }),
        getDamageDoneCountByFilter: builder.query<number, { combatPlayerId: number, filter: string, target: string, spell: string }>({
            query: ({ combatPlayerId, filter, target, spell }) => `/DamageDone/countByFilter?combatPlayerId=${combatPlayerId}&filter=${filter}&target=${target}&spell=${spell}`,
        }),
        getDamageDoneGeneralByCombatPlayerId: builder.query<DamageDoneGeneralModel[], number>({
            query: combatPlayerId => `/DamageDoneGeneral/getByCombatPlayerId/${combatPlayerId}`,
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
    useLazyGetDamageDoneCountByCombatPlayerIdQuery,
    useLazyGetDamageDoneUniqueFilterValuesQuery,
    useGetDamageDoneUniqueFilterValuesQuery,
    useGetDamageDoneValueByTargetQuery,
    useLazyGetDamageDoneValueByTargetQuery,
    useGetDamageDoneByFilterQuery,
    useLazyGetDamageDoneDamageByEachTargetQuery,
    useGetDamageDoneCountByFilterQuery,
    useGetDamageDoneGeneralByCombatPlayerIdQuery,
    useLazyGetDamageDoneGeneralByCombatPlayerIdQuery,
} = DamageDoneApi;