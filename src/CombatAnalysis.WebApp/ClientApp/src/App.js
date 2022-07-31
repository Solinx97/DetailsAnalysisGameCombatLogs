import React from 'react';
import {
    Routes,
    Route
} from 'react-router-dom';
import { Layout } from './components/Layout';
import Home from './components/Home';
import GeneralAnalysis from './components/GeneralAnalysis';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';

import './custom.css'

const App = () => {
    return (
        <Layout>
            <Routes>
                <Route path='/' element={<GeneralAnalysis />} />
                <Route path='/counter' element={<Counter />} />
                <Route path='/fetch-data' element={<FetchData />} />
            </Routes>
        </Layout>
    );
}

export default App;