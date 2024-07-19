import { UserApi } from "./UserApi";

export const CustomerApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        getCustomerById: builder.query({
            query: (id) => `/Customer/${id}`,
        }),
        createAsync: builder.mutation({
            query: customer => ({
                body: customer,
                url: '/Customer',
                method: 'POST'
            })
        }),
        editCustomerAsync: builder.mutation({
            query: customer => ({
                body: customer,
                url: '/Customer',
                method: 'PUT'
            })
        }),
        searchByUserIdAsync: builder.query({
            query: (id) => `/Customer/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'Customer', id }],
        }),
    })
})

export const {
    useGetCustomerByIdQuery,
    useCreateAsyncMutation,
    useLazyGetCustomerByIdQuery,
    useEditCustomerAsyncMutation,
    useLazySearchByUserIdAsyncQuery
} = CustomerApi;