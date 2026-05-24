import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CombatPlayerAuraModel } from '../types/CombatPlayerAuraModel';
import type { CombatLogModel } from '../types/CombatLogModel';
import type { CombatModel } from '../types/CombatModel';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import type { CombatPlayerDeathModel } from '../types/CombatPlayerDeathModel';
import type { CombatAbilityModel } from '../types/CombatAbilityModel';

const apiURL = '/api/v1';

export const GameLogsApi = createApi({
    reducerPath: 'combatParserAPi',
    tagTypes: [
        'CombatAbility',
        'CombatLog',
        'Combat',
        'CombatPlayer',
        'CombatPlayerAura',
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
        getCombatAbilities: builder.query<CombatAbilityModel[], { combatPlayerId: number, abilityType: number }>({
            query: ({ combatPlayerId, abilityType }) => `/CombatAbility?combatPlayerId=${combatPlayerId}&abilityType=${abilityType}`,
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
        getCombatById: builder.query<CombatModel, number>({
            query: id => `/Combat/${id}`,
            providesTags: result => result ? [{ type: 'Combat', id: result.id }] : [],
        }),
        getCombatPlayerAurasByCombatId: builder.query<CombatPlayerAuraModel[], number>({
            query: combatId => `/CombatPlayerAura/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(combatPlayerAura => ({ type: 'CombatPlayerAura' as const, id: combatPlayerAura.id })),
                        { type: 'CombatPlayerAura', id: 'LIST' },
                    ]
                    : [{ type: 'CombatPlayerAura', id: 'LIST' }]
        }),
    })
})

export const {
    useLazyGetCombatAbilitiesQuery,
    useGetCombatLogsQuery,
    useLazyGetPlayersDeathByPlayerIdQuery,
    useLazyGetCombatsByCombatLogIdQuery,
    useLazyGetCombatPlayersByCombatIdQuery,
    useLazyGetCombatPlayerByIdQuery,
    useLazyGetCombatByIdQuery,
    useLazyGetCombatPlayerAurasByCombatIdQuery,
} = GameLogsApi;