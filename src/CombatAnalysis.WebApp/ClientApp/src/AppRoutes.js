import Login from './components/account/Login';
import Registration from './components/account/Registration';
import CombatGeneralDetails from './components/combatDetails/CombatGeneralDetails';
import DetailsSpecificalCombat from './components/combatDetails/DetailsSpecificalCombat';
import GeneralAnalysis from './components/combatDetails/GeneralAnalysis';
import MainInformation from './components/combatDetails/MainInformation';
import Communication from './components/communication/Communication';
import SelectedCommunity from './components/communication/community/SelectedCommunity';
import Home from './components/Home';

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/main-information',
        element: <MainInformation />
    },
    {
        path: '/communication',
        element: <Communication />
    },
    {
        path: '/community',
        element: <SelectedCommunity />
    },
    {
        path: '/general-analysis',
        element: <GeneralAnalysis />
    },
    {
        path: '/details-specifical-combat',
        element: <DetailsSpecificalCombat />
    },
    {
        path: '/combat-general-details',
        element: <CombatGeneralDetails />
    },
    {
        path: '/registration',
        element: <Registration />
    },
    {
        path: '/login',
        element: <Login />
    }
];

export default AppRoutes;
