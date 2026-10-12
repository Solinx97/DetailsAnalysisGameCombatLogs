import { AuthProvider } from '@/context/AuthProvider';
import NotificationProvider from '@/providers/NotificationProvider';
import Layout from '@/shared/components/Layout';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './Routes';

import './App.css';

const App: React.FC = () => {
    return (
        <AuthProvider>
            <NotificationProvider>
                <Layout>
                    <Routes>
                        {AppRoutes.map((route, index) => {
                            const { element, ...rest } = route;
                            return <Route key={index} {...rest} element={element} />;
                        })}
                    </Routes>
                </Layout>
            </NotificationProvider>
        </AuthProvider>
    );
}

export default App;