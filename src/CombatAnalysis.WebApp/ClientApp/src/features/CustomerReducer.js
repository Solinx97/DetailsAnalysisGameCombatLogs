import { createSlice } from '@reduxjs/toolkit';

export const customerSlice = createSlice({
    name: 'customer',
    initialState: {
        value: null
    },
    reducers: {
        customerUpdate: (state, action) => {
            state.value = action.payload;
        },
    },
});

export const { customerUpdate } = customerSlice.actions;

export default customerSlice.reducer;