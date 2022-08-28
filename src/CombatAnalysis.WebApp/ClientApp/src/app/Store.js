import { configureStore } from '@reduxjs/toolkit';
import combatLogReducer from '../features/CombatLogReducer';
import combatReducer from '../features/CombatReducer';
import combatPlayerReducer from '../features/CombatPlayerReducer';

export default configureStore({
    reducer: {
        combatLog: combatLogReducer,
        combat: combatReducer,
        combatPlayer: combatPlayerReducer
    },
});