import { createSlice } from '@reduxjs/toolkit';

export const combatPlayerSlice = createSlice({
    name: 'combatPlayer',
    initialState: {
        value: 0.
    },
    reducers: {
        updateCombatPlayerId: (state, action) => {
            state.value = action.payload;
        },
    },
});

export const { updateCombatPlayerId } = combatPlayerSlice.actions;

export default combatPlayerSlice.reducer;