import React, { useRef } from 'react';
import { useSelector } from 'react-redux';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import Layout from './components/Layout';
import VoiceChatMinimazed from './components/communication/voiceChat/VoiceChatMinimazed';
import { VoiceServiceProvider } from './context/VoiceServiceContext';

import './custom.css';

const App = () => {
    const callMinimazedData = useRef({
        stream: null,
        peers: []
    });

    const call = useSelector((state) => state.call.value);

    const render = () => {
        return (
            <VoiceServiceProvider value={callMinimazedData}>
                <Layout>
                    <Routes>
                        {AppRoutes.map((route, index) => {
                            const { element, ...rest } = route;
                            return <Route key={index} {...rest} element={element} />;
                        })}
                    </Routes>
                    {call.useMinimaze &&
                        <VoiceChatMinimazed />
                    }
                </Layout>
            </VoiceServiceProvider>
        );
    }

    return render();
}

export default App;