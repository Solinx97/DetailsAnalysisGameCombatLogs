import { createSlice } from '@reduxjs/toolkit';

export const userSlice = createSlice({
    name: 'user',
    initialState: {
        value: null
    },
    reducers: {
        userUpdate: (state, action) => {
            state.value = action.payload;
        },
    },
});

export const { userUpdate } = userSlice.actions;

export default userSlice.reducer;