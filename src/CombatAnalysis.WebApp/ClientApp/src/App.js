import React, { StrictMode, useRef, useState } from 'react';
import { Route, Routes } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import AppRoutes from './AppRoutes';
import Layout from './components/Layout';
import VoiceChatMinimazed from './components/communication/voiceChat/VoiceChatMinimazed';
import { AuthProvider } from './context/AuthProvider';
import { VoiceServiceProvider } from './context/VoiceServiceContext';

import 'react-toastify/dist/ReactToastify.css';
import './custom.css';

const App = () => {
    const callMinimazedData = useRef({
        stream: null,
        peers: [],
        turnOnCamera: false,
        turnOnMicrophone: false,
        screenSharing: false,
        roomId: 0,
        socketId: "",
        roomName: "",
    });

    const [useMinimaze, setUseMinimaze] = useState(false);

    const render = () => {
        return (
            <StrictMode>
                <AuthProvider children={
                    <VoiceServiceProvider value={{ callMinimazedData, useMinimaze, setUseMinimaze }}>
                        <Layout>
                            <Routes>
                                {AppRoutes.map((route, index) => {
                                    const { element, ...rest } = route;
                                    return <Route key={index} {...rest} element={element} />;
                                })}
                            </Routes>
                            {useMinimaze &&
                                <VoiceChatMinimazed />
                            }
                            <ToastContainer />
                        </Layout>
                    </VoiceServiceProvider>
                } />
            </StrictMode>
        );
    }

    return render();
}

export default App;