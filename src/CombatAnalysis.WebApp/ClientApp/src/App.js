import React from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import Layout from './components/Layout';
import { AuthProvider } from './context/AuthProvider';
import { ChatHubProvider } from './context/ChatHubProvider';

import './i18n';

import './custom.css';

const App = () => {
    const render = () => {
        return (
            <AuthProvider>
                <ChatHubProvider>
                    <Layout>
                        <Routes>
                            {AppRoutes.map((route, index) => {
                                const { element, ...rest } = route;
                                return <Route key={index} {...rest} element={element} />;
                            })}
                        </Routes>
                    </Layout>
                </ChatHubProvider>
            </AuthProvider>
        );
    }

    return render();
}

export default App;