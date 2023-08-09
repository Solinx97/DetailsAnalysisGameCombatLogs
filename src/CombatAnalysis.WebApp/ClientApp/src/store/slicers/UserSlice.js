import { createSlice } from '@reduxjs/toolkit'

const initialState = {
    value: null,
}

export const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        updateUser: (state, action) => {
            state.value = action.payload
        },
    },
})

export const { updateUser } = userSlice.actions

export default userSlice.reducer