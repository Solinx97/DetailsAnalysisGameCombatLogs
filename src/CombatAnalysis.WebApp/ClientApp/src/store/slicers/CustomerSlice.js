import { createSlice } from '@reduxjs/toolkit'

const initialState = {
    value: null,
}

export const customerSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        updateCustomer: (state, action) => {
            state.value = action.payload
        },
    },
})

export const { updateCustomer } = customerSlice.actions

export default customerSlice.reducer