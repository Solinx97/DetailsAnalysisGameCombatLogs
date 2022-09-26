import { configureStore } from '@reduxjs/toolkit';
import combatLogReducer from '../features/CombatLogReducer';
import combatReducer from '../features/CombatReducer';
import combatPlayerReducer from '../features/CombatPlayerReducer';
import userReducer from '../features/UserReducer';
import authenticationReducer from '../features/AuthenticationReducer';

export default configureStore({
    reducer: {
        combatLog: combatLogReducer,
        combat: combatReducer,
        combatPlayer: combatPlayerReducer,
        user: userReducer,
        authentication: authenticationReducer,
    },
});