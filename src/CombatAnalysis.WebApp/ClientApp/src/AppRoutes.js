import MainInformation from './components/MainInformation';
import Home from './components/Home';
import Chats from './components/chats/Chats';
import GeneralAnalysis from './components/GeneralAnalysis';
import DetailsSpecificalCombat from './components/DetailsSpecificalCombat';
import CombatGeneralDetails from './components/CombatGeneralDetails';
import Registration from './components/account/Registration';
import Login from './components/account/Login';

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
        path: '/chats',
        element: <Chats />
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
