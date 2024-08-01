import { createSlice } from '@reduxjs/toolkit'

const initialState = {
    value: false,
}

export const communityMenuSlice = createSlice({
    name: 'communityMenu',
    initialState,
    reducers: {
        updateMenu: (state, action) => {
            state.value = action.payload
        },
    },
})

export const { updateMenu } = communityMenuSlice.actions

export default communityMenuSlice.reducer