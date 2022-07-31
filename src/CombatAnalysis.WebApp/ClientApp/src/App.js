import React from 'react';
import {
    Routes,
    Route
} from 'react-router-dom';
import { Layout } from './components/Layout';
import GeneralAnalysis from './components/GeneralAnalysis';
import DetailsSpecificalCombat from './components/DetailsSpecificalCombat';
import DamageDoneDetails from './components/DamageDoneDetails';
import DamageTakenDetails from './components/DamageTakenDetails';
import HealDoneDetails from './components/HealDoneDetails';
import ResourceRecoveryDetails from './components/ResourceRecoveryDetails';

import './custom.css'

const App = () => {
    return (
        <Layout>
            <Routes>
                <Route path='/' element={<GeneralAnalysis />} />
                <Route path='/details-specifical-combat' element={<DetailsSpecificalCombat />} />
                <Route path='/damage-done-details' element={<DamageDoneDetails />} />
                <Route path='/damage-taken-details' element={<DamageTakenDetails />} />
                <Route path='/heal-done-details' element={<HealDoneDetails />} />
                <Route path='/resource-recovery-details' element={<ResourceRecoveryDetails />} />
            </Routes>
        </Layout>
    );
}

export default App;