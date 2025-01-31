import { combineReducers, configureStore } from '@reduxjs/toolkit';
import authenticationMiddleware from '../middleware/authenticationMiddleware';
import { ChatApi } from './api/core/Chat.api';
import { CombatParserApi } from './api/core/CombatParser.api';
import { CommunityApi } from './api/core/Community.api';
import { PostApi } from './api/core/Post.api';
import { UserApi } from './api/core/User.api';
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
    [PostApi.reducerPath]: PostApi.reducer,
    [CombatParserApi.reducerPath]: CombatParserApi.reducer,
});

const Store = configureStore({
    reducer: reducers,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware()
            .concat(UserApi.middleware)
            .concat(ChatApi.middleware)
            .concat(CommunityApi.middleware)
            .concat(PostApi.middleware)
            .concat(CombatParserApi.middleware)
            .concat(authenticationMiddleware)
});

export default Store;