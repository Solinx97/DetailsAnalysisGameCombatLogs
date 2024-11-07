import { combineReducers, configureStore } from '@reduxjs/toolkit';
import authenticationMiddleware from '../middleware/authenticationMiddleware';
import { ChatApi } from './api/ChatApi';
import { CombatParserApi } from './api/CombatParserApi';
import { CommunityApi } from './api/CommunityApi';
import { UserApi } from './api/UserApi';
import communityMenuReducer from './slicers/CommunityMenuSlice';
import customerReducer from './slicers/CustomerSlice';
import userPrivacyReducer from './slicers/UserPrivacySlice';
import userReducer from './slicers/UserSlice';

const reducers = combineReducers({
    customer: customerReducer,
    user: userReducer,
    userPrivacy: userPrivacyReducer,
    communityMenu: communityMenuReducer,
    [UserApi.reducerPath]: UserApi.reducer,
    [ChatApi.reducerPath]: ChatApi.reducer,
    [CommunityApi.reducerPath]: CommunityApi.reducer,
    [CombatParserApi.reducerPath]: CombatParserApi.reducer,
});

const Store = configureStore({
    reducer: reducers,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware()
            .concat(UserApi.middleware)
            .concat(ChatApi.middleware)
            .concat(CommunityApi.middleware)
            .concat(CombatParserApi.middleware)
            .concat(authenticationMiddleware)
});

export default Store;