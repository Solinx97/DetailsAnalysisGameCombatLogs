import useTime from '@/shared/hooks/useTime';
import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { BossMapModel } from '../types/BossMapModel';
import type { CombatAbilityModel } from '../types/CombatAbilityModel';
import type { CombatLogModel } from '../types/CombatLogModel';
import type { CombatModel } from '../types/CombatModel';
import type { CombatPlayerAuraModel } from '../types/CombatPlayerAuraModel';
import type { CombatPlayerDeathModel } from '../types/CombatPlayerDeathModel';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import type { DashboardModel } from '../types/dashboard/DashboardModel';
import type { UnitCastModel } from '../types/UnitCastModel';
import type { UnitHealthModel } from '../types/UnitHealthModel';
import type { UnitModel } from '../types/UnitModel';
import type { UnitPositionModel } from '../types/UnitPositionModel';
import type { UnitPreAuraModel } from '../types/UnitPreAuraModel';
import type { WoWMidnightPlayerStatsModel } from '../types/woWMidnight/WoWMidnightPlayerStatsModel';
import type { WoWMoPClassicPlayerStatsModel } from '../types/wowMoPClassic/WoWMoPClassicPlayerStatsModel';

const apiURL = '/api/v1';

const setTimeToms = (units: UnitModel[]): UnitModel[] => {
    const { timeToMs } = useTime();

    return units.map(unit => ({
        ...unit,
        unitPositions: unit.unitPositions.map(p => ({
            ...p,
            timeMs: timeToMs(p.time)
        }))
    }));
}

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
        getCombatLogs: builder.query<CombatLogModel[], { logType: number, gameVersion: number, appUserId: string | null }>({
            query: ({ logType, gameVersion, appUserId }) => `/CombatLog/getByLogType?logType=${logType}&gameVersion=${gameVersion}&appUserId=${appUserId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(combatLog => ({ type: 'CombatLog' as const, id: combatLog.id })),
                        { type: 'CombatLog', id: 'LIST' },
                    ]
                    : [{ type: 'CombatLog', id: 'LIST' }]
        }),
        addCombatLogStatus: builder.mutation<void, { combatLogId: number, status: number }>({
            query: ({ combatLogId, status }) => ({
                url: `/CombatLog/addStatus/${combatLogId}?status=${status}`,
                method: 'POST'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'CombatLog', id: args.combatLogId }]
        }),
        removeCombatLog: builder.mutation<void, number>({
            query: id => ({
                url: `/CombatLog/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'CombatLog', id }]
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
        getUniqueCombatsByCombatLogId: builder.query<Map<string, CombatModel[]>, number>({
            query: combatLogId => `/Combat/getUniquByCombatLogId/${combatLogId}`,
        }),
        getCombatPlayerDeathCount: builder.query<number, string>({
            query: unitId => `/CombatPlayer/getPlayerDeathCount/${unitId}`,
        }),
        getWhenCombatPlayerDeath: builder.query<string, { unitId: string, skipCount: number }>({
            query: ({ unitId, skipCount }) => `/CombatPlayer/getWhenPlayerDeath/${unitId}?skipCount=${skipCount}`,
        }),
        getCombatPlayerDeath: builder.query<CombatPlayerDeathModel[], { unitId: string, whenDied: string }>({
            query: ({ unitId, whenDied }) => `/CombatPlayer/getPlayerDeath/${unitId}?whenDied=${whenDied}`,
        }),
        getDashboard: builder.query<DashboardModel, { dahsboardName: string, combatLogId: number, combatId: number, unitName: string, valueType: number }>({
            query: ({ dahsboardName, combatLogId, combatId, unitName, valueType }) => `/Dashboard/${dahsboardName}/${combatLogId}?combatId=${combatId}&unitName=${unitName}&valueType=${valueType}`
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
        getUniqueCombatPlayerNames: builder.query<string[], number>({
            query: combatLogId => `/CombatPlayer/getUniquePlayerNames/${combatLogId}`,
        }),
        getCombatPlayerById: builder.query<CombatPlayerModel, number>({
            query: id => `/CombatPlayer/${id}`,
            providesTags: result => result ? [{ type: 'CombatPlayer', id: result.id }] : [],
        }),
        getCombatPreAuras: builder.query<UnitPreAuraModel[], number>({
            query: combatId => `/UnitPreAura/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(preAura => ({ type: 'CombatPlayerAura' as const, id: preAura.id })),
                        { type: 'CombatPlayerAura', id: 'LIST' },
                    ]
                    : [{ type: 'CombatPlayerAura', id: 'LIST' }]
        }),
        getUnitPreAuras: builder.query<UnitPreAuraModel[], { combatId: number, unitId: string }>({
            query: ({ combatId, unitId }) => `/UnitPreAura/getByUnitId/${combatId}?unitId=${unitId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(preAura => ({ type: 'CombatPlayerAura' as const, id: preAura.id })),
                        { type: 'CombatPlayerAura', id: 'LIST' },
                    ]
                    : [{ type: 'CombatPlayerAura', id: 'LIST' }]
        }),
        getCombatPlayerAurasByCombatId: builder.query<CombatPlayerAuraModel[], number>({
            query: combatId => `/UnitAura/getByCombatId/${combatId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(combatPlayerAura => ({ type: 'CombatPlayerAura' as const, id: combatPlayerAura.id })),
                        { type: 'CombatPlayerAura', id: 'LIST' },
                    ]
                    : [{ type: 'CombatPlayerAura', id: 'LIST' }],
            keepUnusedDataFor: 0,
        }),
        getCombatUnitsByCombatId: builder.query<UnitModel[], number>({
            query: combatId => `/Unit/getByCombatId/${combatId}`,
            transformResponse: setTimeToms
        }),
        getUnitCastsByCombatUnitId: builder.query<UnitCastModel[], string>({
            query: combatUnitId => `/UnitCast/getByCombatUnitId/${combatUnitId}`,
        }),
        getUnitPositionsByCombatId: builder.query<Map<string, UnitPositionModel[]>, number>({
            query: combatId => `/UnitPosition/getByCombatId/${combatId}`,
        }),
        getUnitsHealthByCombatId: builder.query<Map<string, UnitHealthModel[]>, number>({
            query: combatId => `/UnitHealth/getByCombatId/${combatId}`,
        }),
        getUnitsHealthByInterval: builder.query<UnitHealthModel[], { unitId: string, whenDied: string }>({
            query: ({ unitId: combatId, whenDied }) => `/UnitHealth/getBeforeDied/${combatId}?whenDied=${whenDied}`,
        }),
        getPlayerStatsByCombatPlayerId: builder.query<WoWMoPClassicPlayerStatsModel | WoWMidnightPlayerStatsModel, { combatPlayerId: number, gameVersion: number }>({
            query: ({ combatPlayerId, gameVersion }) => `/CombatPlayer/getPlayerStats/${combatPlayerId}?gameVersion=${gameVersion}`,
        }),
    })
})

export const {
    useLazyGetCombatAbilitiesQuery,
    useGetCombatLogsQuery,
    useAddCombatLogStatusMutation,
    useRemoveCombatLogMutation,
    useLazyGetCombatsByCombatLogIdQuery,
    useGetUniqueCombatsByCombatLogIdQuery,
    useLazyGetUniqueCombatsByCombatLogIdQuery,
    useLazyGetCombatPlayerDeathCountQuery,
    useLazyGetWhenCombatPlayerDeathQuery,
    useLazyGetCombatPlayerDeathQuery,
    useGetDashboardQuery,
    useLazyGetCombatByIdQuery,
    useLazyGetBossMapByIdQuery,
    useLazyGetCombatPlayersByCombatIdQuery,
    useGetUniqueCombatPlayerNamesQuery,
    useLazyGetCombatPlayerByIdQuery,
    useGetCombatPlayerAurasByCombatIdQuery,
    useGetCombatPreAurasQuery,
    useGetUnitPreAurasQuery,
    useGetCombatUnitsByCombatIdQuery,
    useLazyGetUnitCastsByCombatUnitIdQuery,
    useLazyGetUnitPositionsByCombatIdQuery,
    useLazyGetUnitsHealthByCombatIdQuery,
    useLazyGetUnitsHealthByIntervalQuery,
    useGetPlayerStatsByCombatPlayerIdQuery,
} = GameLogsApi;