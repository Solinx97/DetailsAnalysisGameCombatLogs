import { UserApi } from "./UserApi";

export const IdentityApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        authorizationCodeExchange: builder.query({
            query: (authorizationCode) => `/Identity?authorizationCode=${authorizationCode}`
        }),
    })
})

export const {
    useLazyAuthorizationCodeExchangeQuery,
} = IdentityApi;