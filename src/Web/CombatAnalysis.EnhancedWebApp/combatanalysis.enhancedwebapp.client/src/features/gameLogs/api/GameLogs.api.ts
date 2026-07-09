import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CombatPlayerAuraModel } from '../types/CombatPlayerAuraModel';
import type { CombatLogModel } from '../types/CombatLogModel';
import type { CombatModel } from '../types/CombatModel';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import type { CombatPlayerDeathModel } from '../types/CombatPlayerDeathModel';
import type { CombatAbilityModel } from '../types/CombatAbilityModel';
import type { CombatPlayerPreAuraModel } from '../types/CombatPlayerPreAuraModel';
import type { CombatPlayerPositionModel } from '../types/CombatPlayerPositionModel';
import type { BossMapModel } from '../types/BossMapModel';
import type { DashboardModel } from '../types/dashboard/DashboardModel';

const apiURL = '/api/v1';

export const GameLogsApi = createApi({
    reducerPath: 'combatParserAPi',
    tagTypes: [
        'CombatAbility',
        'CombatLog',
        'Combat',
        'BossMap',
        'CombatPlayer',
        'CombatPlayerAura',
        'CombatPlayerPosition',
        'DamageDone',
        'DamageDoneGeneral',
        'DamageTaken',
        'DamageTakenGeneral',
        'HealDone',
        'HealDoneGeneral',
        'ResourceRecovery',
        'ResourceRecoveryGeneral',
        'PlayerDeath',
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
        getCombatLogs: builder.query<CombatLogModel[], void>({
            query: () => '/CombatLog',
            providesTags: result =>
                result
                    ? [
                        ...result.map(combatLog => ({ type: 'CombatLog' as const, id: combatLog.id })),
                        { type: 'CombatLog', id: 'LIST' },
                    ]
                    : [{ type: 'CombatLog', id: 'LIST' }]
        }),
        getPlayersDeathByPlayerId: builder.query<CombatPlayerDeathModel[], number>({
            query: combatPlayerId => `/PlayerDeath/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(playerDeath => ({ type: 'PlayerDeath' as const, id: playerDeath.id })),
                        { type: 'PlayerDeath', id: 'LIST' },
                    ]
                    : [{ type: 'PlayerDeath', id: 'LIST' }]
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
            query: ({ combatId, combatPlayerId }) => `/PreAura/getByCombatId?combatId=${combatId}&combatPlayerId=${combatPlayerId}`,
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
        getCombatPlayerPositionsByCombatPlayerId: builder.query<CombatPlayerPositionModel[], number>({
            query: combatPlayerId => `/CombatPlayerPosition/getByCombatPlayerId/${combatPlayerId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(combatPlayerPosition => ({ type: 'CombatPlayerPosition' as const, id: combatPlayerPosition.id })),
                        { type: 'CombatPlayerPosition', id: 'LIST' },
                    ]
                    : [{ type: 'CombatPlayerPosition', id: 'LIST' }]
        }),
    })
})

export const {
    useLazyGetCombatAbilitiesQuery,
    useGetCombatLogsQuery,
    useLazyGetPlayersDeathByPlayerIdQuery,
    useLazyGetCombatsByCombatLogIdQuery,
    useGetCombatsDashboardQuery,
    useGetCombatsDamageSpellsQuery,
    useGetCombatsHealSpellsQuery,
    useLazyGetCombatByIdQuery,
    useLazyGetBossMapByIdQuery,
    useLazyGetCombatPlayersByCombatIdQuery,
    useLazyGetCombatPlayerByIdQuery,
    useGetCombatPlayerAurasByCombatIdQuery,
    useGetCombatByPreAuraQuery,
    useLazyGetCombatPlayerPositionsByCombatPlayerIdQuery
} = GameLogsApi;