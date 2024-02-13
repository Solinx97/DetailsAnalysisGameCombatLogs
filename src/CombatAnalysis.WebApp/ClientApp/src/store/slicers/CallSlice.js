import { createSlice } from '@reduxjs/toolkit';

const initialState = {
    value: {
        useMinimaze: false,
        roomId: 0,
        roomName: "",
        turnOnCamera: false,
        turnOnMicrophone: false,
    }
}

export const callSlice = createSlice({
    name: 'call',
    initialState,
    reducers: {
        updateCall: (state, action) => {
            state.value = action.payload
        },
    },
})

export const { updateCall } = callSlice.actions

export default callSlice.reducer