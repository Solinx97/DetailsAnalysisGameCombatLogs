import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import App from './app/App.tsx';
import { loadConfig } from './app/configLoader.ts';
import Store from './app/Store.ts';
import { APP_CONFIG } from './config/appConfig.ts';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';

import './i18n';

loadConfig().then((config) => {
    Object.assign(APP_CONFIG, config);

    createRoot(document.getElementById('root')!).render(
        <Provider store={Store}>
            <BrowserRouter>
                <App />
            </BrowserRouter>
        </Provider>
    )
});