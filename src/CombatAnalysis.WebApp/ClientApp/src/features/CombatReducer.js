import { createSlice } from '@reduxjs/toolkit';

export const combatSlice = createSlice({
    name: 'combat',
    initialState: {
        value: 0.
    },
    reducers: {
        updateCombatId: (state, action) => {
            state.value = action.payload;
        },
    },
});

export const { updateCombatId } = combatSlice.actions;

export default combatSlice.reducer;