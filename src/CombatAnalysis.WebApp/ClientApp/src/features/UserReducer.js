import { createSlice } from '@reduxjs/toolkit';

export const userSlice = createSlice({
    name: 'user',
    initialState: {
        value: false
    },
    reducers: {
        updateAuthorizationState: (state, action) => {
            state.value = action.payload;
        },
    },
});

export const { updateAuthorizationState } = userSlice.actions;

export default userSlice.reducer;