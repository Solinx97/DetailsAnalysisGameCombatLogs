import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { UserApi } from './api/UserApi';
import { ChatApi } from './api/ChatApi';
import authReducer from './slicers/AuthSlice'
import customerReducer from './slicers/CustomerSlice'

const reducers = combineReducers({
    auth: authReducer,
    customer: customerReducer,
    [UserApi.reducerPath]: UserApi.reducer,
    [ChatApi.reducerPath]: ChatApi.reducer
});

const Store = configureStore({
    reducer: reducers,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware()
            .concat(UserApi.middleware)
            .concat(ChatApi.middleware),
});

export default Store;