import { createSlice } from '@reduxjs/toolkit'

const initialState = {
    value: false,
}

export const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        update: (state, action) => {
            state.value = action.payload
        },
    },
})

export const { update } = authSlice.actions

export default authSlice.reducer