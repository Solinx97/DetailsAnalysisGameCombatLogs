import { combineReducers, configureStore } from '@reduxjs/toolkit';
import { ChatApi } from './api/ChatApi';
import { CombatParserApi } from './api/CombatParserApi';
import { UserApi } from './api/UserApi';
import customerReducer from './slicers/CustomerSlice';
import userReducer from './slicers/UserSlice';
import authenticationMiddleware from '../middleware/authenticationMiddleware';

const reducers = combineReducers({
    customer: customerReducer,
    user: userReducer,
    [UserApi.reducerPath]: UserApi.reducer,
    [UserApi.reducerPath]: UserApi.reducer,
    [ChatApi.reducerPath]: ChatApi.reducer,
    [CombatParserApi.reducerPath]: CombatParserApi.reducer,
});

const Store = configureStore({
    reducer: reducers,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware()
            .concat(UserApi.middleware)
            .concat(ChatApi.middleware)
            .concat(CombatParserApi.middleware)
            .concat(authenticationMiddleware)
});

export default Store;