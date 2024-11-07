import { UserApi } from "./UserApi";

export const IdentityApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        authorizationCodeExchange: builder.query({
            query: (authorizationCode) => `/Identity?authorizationCode=${authorizationCode}`
        }),
        getUserPrivacy: builder.query({
            query: (id) => `/Identity/userPrivacy/${id}`
        }),
    })
})

export const {
    useLazyAuthorizationCodeExchangeQuery,
    useLazyGetUserPrivacyQuery,
} = IdentityApi;