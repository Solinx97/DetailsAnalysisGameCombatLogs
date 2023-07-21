import { ChatApi } from "./ChatApi";

export const RequestToConnectApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createRequestAsync: builder.mutation({
            query: request => ({
                body: request,
                url: '/RequestToConnect',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'RequestToConnect', arg }],
        }),
        searchByToUserId: builder.query({
            query: (id) => `/RequestToConnect/searchByToUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'RequestToConnect', id }],
        }),
        searchByOwnerId: builder.query({
            query: (id) => `/RequestToConnect/searchByOwnerId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'RequestToConnect', id })), 'RequestToConnect']
                    : ['RequestToConnect'],
        }),
        removeRequestAsync: builder.mutation({
            query: id => ({ 
                url: `/RequestToConnect/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'RequestToConnect', arg }],
        })
    })
})

export const {
    useCreateRequestAsyncMutation,
    useSearchByToUserIdQuery,
    useSearchByOwnerIdQuery,
    useRemoveRequestAsyncMutation,
} = RequestToConnectApi;