import React from 'react';
import { useSelector } from 'react-redux';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import Layout from './components/Layout';
import VoiceChatMinimazed from './components/communication/voiceChat/VoiceChatMinimazed';

import './custom.css';

const App = () => {
    const call = useSelector((state) => state.call.value);

    const render = () => {
        return (
            <Layout>
                <Routes>
                    {AppRoutes.map((route, index) => {
                        const { element, ...rest } = route;
                        return <Route key={index} {...rest} element={element} />;
                    })}
                </Routes>
                {call.useMinimaze &&
                    <VoiceChatMinimazed
                        call={call}
                    />
                }
            </Layout>
        );
    }

    return render();
}

export default App;