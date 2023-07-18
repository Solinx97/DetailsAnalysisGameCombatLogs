import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { UserApi } from './api/UserApi';

const reducers = combineReducers({
    [UserApi.reducerPath]: UserApi.reducer
});

const Store = configureStore({
    reducer: reducers,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(UserApi.middleware),
});

export default Store;