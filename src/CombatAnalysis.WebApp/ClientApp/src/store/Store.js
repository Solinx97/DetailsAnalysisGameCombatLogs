import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { UserApi } from './api/UserApi';
import { ChatApi } from './api/ChatApi';

const reducers = combineReducers({
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