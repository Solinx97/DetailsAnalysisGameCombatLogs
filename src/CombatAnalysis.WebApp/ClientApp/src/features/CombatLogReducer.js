import { createSlice } from '@reduxjs/toolkit';

export const combatLogSlice = createSlice({
    name: 'combatLog',
    initialState: {
        value: 0.
    },
    reducers: {
        updateCombatLogId: (state, action) => {
            state.value = action.payload;
        },
    },
});

export const { updateCombatLogId } = combatLogSlice.actions;

export default combatLogSlice.reducer;