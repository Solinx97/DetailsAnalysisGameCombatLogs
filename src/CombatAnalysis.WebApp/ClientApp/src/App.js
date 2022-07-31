import React from 'react';
import {
    Routes,
    Route
} from 'react-router-dom';
import { Layout } from './components/Layout';
import GeneralAnalysis from './components/GeneralAnalysis';
import DetailsSpecificalCombat from './components/DetailsSpecificalCombat';

import './custom.css'

const App = () => {
    return (
        <Layout>
            <Routes>
                <Route path='/' element={<GeneralAnalysis />} />
                <Route path='/details-specifical-combat' element={<DetailsSpecificalCombat />} />
            </Routes>
        </Layout>
    );
}

export default App;