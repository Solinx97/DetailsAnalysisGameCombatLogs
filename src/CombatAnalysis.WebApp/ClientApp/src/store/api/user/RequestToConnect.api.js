import { UserApi } from "../core/User.api";

export const RequestToConnectApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        createRequestAsync: builder.mutation({
            query: request => ({
                body: request,
                url: '/RequestToConnect',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'RequestToConnect', arg }]
        }),
        requestIsExist: builder.query({
            query: (arg) => {
                const { userId, targetUserId } = arg;
                return {
                    url: `/RequestToConnect/isExist?initiatorId=${userId}&companionId=${targetUserId}`,
                }
            },
            providesTags: (result, error, id) => [{ type: 'RequestToConnect', id }]
        }),
        searchByOwnerId: builder.query({
            query: (id) => `/RequestToConnect/searchByOwnerId/${id}`,
            providesTags: (result, error, id) => [{ type: 'RequestToConnect', id }]
        }),
        searchByToUserId: builder.query({
            query: (id) => `/RequestToConnect/searchByToUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'RequestToConnect', id }],
        }),
        removeRequestAsync: builder.mutation({
            query: id => ({ 
                url: `/RequestToConnect/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'RequestToConnect', arg }]
        })
    })
})

export const {
    useCreateRequestAsyncMutation,
    useRequestIsExistQuery,
    useLazyRequestIsExistQuery,
    useSearchByOwnerIdQuery,
    useLazySearchByOwnerIdQuery,
    useRemoveRequestAsyncMutation,
    useSearchByToUserIdQuery,
} = RequestToConnectApi;