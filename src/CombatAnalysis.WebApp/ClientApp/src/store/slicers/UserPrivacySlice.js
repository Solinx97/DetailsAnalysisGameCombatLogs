import { createSlice } from '@reduxjs/toolkit'

const initialState = {
    value: null,
}

export const userPrivacySlice = createSlice({
    name: 'userPrivacy',
    initialState,
    reducers: {
        updateUserPrivacy: (state, action) => {
            state.value = action.payload
        },
    },
})

export const { updateUserPrivacy } = userPrivacySlice.actions

export default userPrivacySlice.reducer