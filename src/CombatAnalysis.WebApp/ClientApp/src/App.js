import React from 'react';
import {
    Routes,
    Route
} from 'react-router-dom';
import Layout from './components/Layout';
import MainInformation from './components/MainInformation';
import GeneralAnalysis from './components/GeneralAnalysis';
import DetailsSpecificalCombat from './components/DetailsSpecificalCombat';
import CombatGeneralDetails from './components/CombatGeneralDetails';
import Registration from './components/account/Registration';
import Login from './components/account/Login';

import './custom.css'

const App = () => {
    return <Layout>
            <Routes>
                <Route path='/' element={<MainInformation />} />
                <Route path='/general-analysis' element={<GeneralAnalysis />} />
                <Route path='/details-specifical-combat' element={<DetailsSpecificalCombat />} />
                <Route path='/combat-general-details' element={<CombatGeneralDetails />} />
                <Route path='/registration' element={<Registration />} />
                <Route path='/login' element={<Login />} />
            </Routes>
        </Layout>;
}

export default App;