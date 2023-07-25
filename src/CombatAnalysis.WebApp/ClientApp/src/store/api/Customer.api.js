import { UserApi } from "./UserApi";

export const CustomerApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        createAsync: builder.mutation({
            query: customer => ({
                body: customer,
                url: '/Customer',
                method: 'POST'
            })
        }),
        getCustomerById: builder.query({
            query: (id) => `/Customer/${id}`,
        }),
        searchByUserIdAsync: builder.query({
            query: (id) => `/Customer/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'Customer', id }],
        }),
    })
})

export const {
    useCreateAsyncMutation,
    useGetCustomerByIdQuery,
    useLazySearchByUserIdAsyncQuery
} = CustomerApi;