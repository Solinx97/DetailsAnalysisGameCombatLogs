import React, { useRef, useState } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import Layout from './components/Layout';
import VoiceChatMinimazed from './components/communication/voiceChat/VoiceChatMinimazed';
import { VoiceServiceProvider } from './context/VoiceServiceContext';

import './custom.css';

const App = () => {
    const callMinimazedData = useRef({
        stream: null,
        peers: [],
        turnOnCamera: false,
        turnOnMicrophone: false,
        roomId: 0,
        socketId: "",
        roomName: "",
    });

    const [useMinimaze, setUseMinimaze] = useState(false);

    const render = () => {
        return (
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
                </Layout>
            </VoiceServiceProvider>
        );
    }

    return render();
}

export default App;