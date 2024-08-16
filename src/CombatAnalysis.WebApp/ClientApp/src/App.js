import React from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import Layout from './components/Layout';
import { AuthProvider } from './context/AuthProvider';

import './i18n';

import './custom.css';

const App = () => {
    const render = () => {
        return (
            <AuthProvider children={
                <Layout>
                    <Routes>
                        {AppRoutes.map((route, index) => {
                            const { element, ...rest } = route;
                            return <Route key={index} {...rest} element={element} />;
                        })}
                    </Routes>
                </Layout>
            } />
        );
    }

    return render();
}

export default App;