import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CombatPlayerAuraModel } from '../types/CombatPlayerAuraModel';
import type { CombatLogModel } from '../types/CombatLogModel';
import type { CombatModel } from '../types/CombatModel';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import type { CombatPlayerDeathModel } from '../types/CombatPlayerDeathModel';
import type { CombatAbilityModel } from '../types/CombatAbilityModel';
import type { CombatPlayerPreAuraModel } from '../types/CombatPlayerPreAuraModel';
import type { UnitPositionModel } from '../types/UnitPositionModel';
import type { BossMapModel } from '../types/BossMapModel';
import type { DashboardModel } from '../types/dashboard/DashboardModel';
import type { UnitCastModel } from '../types/UnitCastModel';
import type { UnitHealthModel } from '../types/UnitHealthModel';
import type { CombatUnitModel } from '../types/CombatUnitModel';

const apiURL = '/api/v1';

export const GameLogsApi = createApi({
    reducerPath: 'combatParserAPi',
    tagTypes: [
        'CombatAbility',
        'CombatLog',
        'Combat',
        'BossMap',
        'UnitHealth',
        'UnitPosition',
        'CombatUnit',
        'CombatPlayer',
        'CombatPlayerAura',
        'CombatPlayerDeath',
        'CombatPlayerCast',
        'DamageDone',
        'DamageDoneGeneral',
        'DamageTaken',
        'DamageTakenGeneral',
        'HealDone',
        'HealDoneGeneral',
        'ResourceRecovery',
        'ResourceRecoveryGeneral',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getCombatAbilities: builder.query<CombatAbilityModel[], { combatPlayerId: number, query: string }>({
            query: ({ combatPlayerId, query }) => `/CombatAbility?combatPlayerId=${combatPlayerId}&${query}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(ability => ({ type: 'CombatAbility' as const, id: ability.id })),
                        { type: 'CombatAbility', id: 'LIST' },
                    ]
                    : [{ type: 'CombatAbility', id: 'LIST' }]
        }),
        getCombatLogs: builder.query<CombatLogModel[], { logType: number, appUserId: string | null }>({
            query: ({ logType, appUserId }) => `/CombatLog/getByLogType?logType=${logType}&appUserId=${appUserId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(combatLog => ({ type: 'CombatLog' as const, id: combatLog.id })),
                        { type: 'CombatLog', id: 'LIST' },
                    ]
                    : [{ type: 'CombatLog', id: 'LIST' }]
        }),
        removeCombatLog: builder.mutation<void, number>({
            query: id => ({
                url: `/CombatLog/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'CombatLog', id }]
        }),
        getCombatPlayersDeathByCombatPlayerId: builder.query<CombatPlayerDeathModel[], number>({
            query: combatPlayerId => `/CombatPlayerDeath/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(playerDeath => ({ type: 'CombatPlayerDeath' as const, id: playerDeath.id })),
                        { type: 'CombatPlayerDeath', id: 'LIST' },
                    ]
                    : [{ type: 'CombatPlayerDeath', id: 'LIST' }]
        }),
        getCombatsByCombatLogId: builder.query<CombatModel[], number>({
            query: combatLogId => `/Combat/getByCombatLogId/${combatLogId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(combat => ({ type: 'Combat' as const, id: combat.id })),
                        { type: 'Combat', id: 'LIST' },
                    ]
                    : [{ type: 'Combat', id: 'LIST' }]
        }),
        getCombatsDashboard: builder.query<DashboardModel[], number>({
            query: combatLogId => `/Combat/getDashboards/${combatLogId}`
        }),
        getCombatsDamageSpells: builder.query<Map<string, number>, number>({
            query: combatLogId => `/Combat/getDamageSpells/${combatLogId}`
        }),
        getCombatsHealSpells: builder.query<Map<string, number>, number>({
            query: combatLogId => `/Combat/getHealSpells/${combatLogId}`
        }),
        getPotions: builder.query<Map<string, number>, number>({
            query: combatLogId => `/Combat/getPotions/${combatLogId}`
        }),
        getCombatById: builder.query<CombatModel, number>({
            query: id => `/Combat/${id}`,
            providesTags: result => result ? [{ type: 'Combat', id: result.id }] : [],
        }),
        getBossMapById: builder.query<BossMapModel, number>({
            query: id => `/BossMap/${id}`,
            providesTags: result => result ? [{ type: 'BossMap', id: result.id }] : [],
        }),
        getCombatPlayersByCombatId: builder.query<CombatPlayerModel[], number>({
            query: combatId => `/CombatPlayer/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(combatPlayer => ({ type: 'CombatPlayer' as const, id: combatPlayer.id })),
                        { type: 'CombatPlayer', id: 'LIST' },
                    ]
                    : [{ type: 'CombatPlayer', id: 'LIST' }]
        }),
        getCombatPlayerById: builder.query<CombatPlayerModel, number>({
            query: id => `/CombatPlayer/${id}`,
            providesTags: result => result ? [{ type: 'CombatPlayer', id: result.id }] : [],
        }),
        getCombatByPreAura: builder.query<CombatPlayerPreAuraModel[], { combatId: number, combatPlayerId: number }>({
            query: ({ combatId, combatPlayerId }) => `/CombatPlayerPreAura/getByCombatId?combatId=${combatId}&combatPlayerId=${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(preAura => ({ type: 'CombatPlayerAura' as const, id: preAura.id })),
                        { type: 'CombatPlayerAura', id: 'LIST' },
                    ]
                    : [{ type: 'CombatPlayerAura', id: 'LIST' }]
        }),
        getCombatPlayerAurasByCombatId: builder.query<CombatPlayerAuraModel[], { combatId: number, combatPlayerId: number }>({
            query: ({ combatId, combatPlayerId }) => `/CombatPlayerAura/getByCombatId?combatId=${combatId}&combatPlayerId=${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(combatPlayerAura => ({ type: 'CombatPlayerAura' as const, id: combatPlayerAura.id })),
                        { type: 'CombatPlayerAura', id: 'LIST' },
                    ]
                    : [{ type: 'CombatPlayerAura', id: 'LIST' }],
            keepUnusedDataFor: 0,
        }),
        getCombatUnitsByCombatId: builder.query<CombatUnitModel[], number>({
            query: combatId => `/CombatUnit/getByCombatId/${combatId}`,
        }),
        getUnitCastsByCombatPlayerId: builder.query<Map<string, UnitCastModel[]>, number>({
            query: combatId => `/UnitCast/getByCombatId/${combatId}`,
        }),
        getUnitPositionsByCombatId: builder.query<Map<string, UnitPositionModel[]>, number>({
            query: combatId => `/UnitPosition/getByCombatId/${combatId}`,
        }),
        getUnitsHealthByCombatId: builder.query<Map<string, UnitHealthModel[]>, number>({
            query: combatId => `/UnitHealth/getByCombatId/${combatId}`,
        }),
    })
})

export const {
    useLazyGetCombatAbilitiesQuery,
    useGetCombatLogsQuery,
    useRemoveCombatLogMutation,
    useLazyGetCombatPlayersDeathByCombatPlayerIdQuery,
    useLazyGetCombatsByCombatLogIdQuery,
    useGetCombatsDashboardQuery,
    useGetCombatsDamageSpellsQuery,
    useGetCombatsHealSpellsQuery,
    useGetPotionsQuery,
    useLazyGetCombatByIdQuery,
    useLazyGetBossMapByIdQuery,
    useLazyGetCombatPlayersByCombatIdQuery,
    useLazyGetCombatPlayerByIdQuery,
    useGetCombatPlayerAurasByCombatIdQuery,
    useGetCombatByPreAuraQuery,
    useLazyGetCombatUnitsByCombatIdQuery,
    useLazyGetUnitCastsByCombatPlayerIdQuery,
    useLazyGetUnitPositionsByCombatIdQuery,
    useLazyGetUnitsHealthByCombatIdQuery,
} = GameLogsApi;