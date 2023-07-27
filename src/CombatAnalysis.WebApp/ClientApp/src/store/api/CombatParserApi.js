import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const CombatParserApi = createApi({
    reducerPath: 'combatParserAPi',
    tagTyes: [
        'MainInformation',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getCombatLogs: builder.query({
            query: () => '/MainInformation'
        }),
        getDamageDoneByPlayerId: builder.query({
            query: (playerId) => `/DamageDone/${playerId}`
        }),
    })
})

export const {
    useGetCombatLogsQuery,
    useGetDamageDoneByPlayerIdQuery,
} = CombatParserApi;