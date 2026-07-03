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
        countDamageDone: builder.query<number, { combatPlayerId: number, target: string, creator: string, spell: string, from: string, to: string }>({
            query: ({ combatPlayerId, target, creator, spell, from, to }) => `/DamageDone/count?combatPlayerId=${combatPlayerId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}`,
        }),
        getAllDamageDone: builder.query<DamageDoneModel[], { combatPlayerId: number, creator: string, target: string, spell: string, from: string, to: string, page: number, pageSize: number }>({
            query: ({ combatPlayerId, target, creator, spell, from, to, page, pageSize }) => `/DamageDone/getAll?combatPlayerId=${combatPlayerId}&target=${target}&creator=${creator}&spell=${spell}&from=${from}&to=${to}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(damageDone => ({ type: 'DamageDone' as const, id: damageDone.id })),
                        { type: 'DamageDone', id: 'LIST' },
                    ]
                    : [{ type: 'DamageDone', id: 'LIST' }]
        }),
        getDamageDoneUniqueFilterValues: builder.query<string[], { combatPlayerId: number, filter: string }>({
            query: ({ combatPlayerId, filter }) => `/DamageDone/getUniqueFilterValues?combatPlayerId=${combatPlayerId}&filter=${filter}`,
        }),
        getDamageDoneDamageByEachTarget: builder.query<Array<CombatTargetModel[]>, number>({
            query: combatId => `/DamageDone/getDamageByEachTarget/${combatId}`,
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
    useCountDamageDoneQuery,
    useLazyGetDamageDoneUniqueFilterValuesQuery,
    useGetDamageDoneUniqueFilterValuesQuery,
    useGetAllDamageDoneQuery,
    useLazyGetDamageDoneDamageByEachTargetQuery,
    useGetDamageDoneGeneralByCombatPlayerIdQuery,
    useLazyGetDamageDoneGeneralByCombatPlayerIdQuery,
} = DamageDoneApi;