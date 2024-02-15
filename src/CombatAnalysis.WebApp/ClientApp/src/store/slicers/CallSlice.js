import { createSlice } from '@reduxjs/toolkit';

const initialState = {
    value: {
        useMinimaze: false,
        roomId: 0,
        socketId: "",
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
        clear: (state) => {
            state.value = initialState;
        },
    },
})

export const { updateCall, clear } = callSlice.actions

export default callSlice.reducer