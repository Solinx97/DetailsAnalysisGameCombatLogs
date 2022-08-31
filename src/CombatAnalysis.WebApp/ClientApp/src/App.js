import React from 'react';
import {
    Routes,
    Route
} from 'react-router-dom';
import Layout from './components/Layout';
import MainInformation from './components/MainInformation';
import GeneralAnalysis from './components/GeneralAnalysis';
import DetailsSpecificalCombat from './components/DetailsSpecificalCombat';
import DamageDoneGeneralDetails from './components/DamageDoneGeneralDetails';
import DamageDoneDetails from './components/DamageDoneDetails';
import DamageTakenGeneralDetails from './components/DamageTakenGeneralDetails';
import HealDoneGeneralDetails from './components/HealDoneGeneralDetails';
import ResourceRecoveryGeneralDetails from './components/ResourceRecoveryGeneralDetails';

import './custom.css'

const App = () => {
    return <Layout>
            <Routes>
                <Route path='/' element={<MainInformation />} />
                <Route path='/general-analysis' element={<GeneralAnalysis />} />
                <Route path='/details-specifical-combat' element={<DetailsSpecificalCombat />} />
                <Route path='/damage-done-general-details' element={<DamageDoneGeneralDetails />} />
                <Route path='/damage-done-details' element={<DamageDoneDetails />} />
                <Route path='/damage-taken-general-details' element={<DamageTakenGeneralDetails />} />
                <Route path='/heal-done-general-details' element={<HealDoneGeneralDetails />} />
                <Route path='/resource-recovery-general-details' element={<ResourceRecoveryGeneralDetails />} />
            </Routes>
        </Layout>;
}

export default App;