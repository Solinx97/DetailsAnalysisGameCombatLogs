import { createSlice } from '@reduxjs/toolkit';

export const authenticationSlice = createSlice({
    name: 'authentication',
    initialState: {
        value: false
    },
    reducers: {
        checkAuth: (state, action) => {
            state.value = action.payload;
        },
    },
});

export const { checkAuth } = authenticationSlice.actions;

export default authenticationSlice.reducer;