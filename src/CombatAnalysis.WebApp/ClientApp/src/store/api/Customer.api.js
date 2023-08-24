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
    useCreateAsyncMutation,
    useGetCustomerByIdQuery,
    useLazyGetCustomerByIdQuery,
    useEditCustomerAsyncMutation,
    useLazySearchByUserIdAsyncQuery
} = CustomerApi;