import Login from './components/account/Login';
import Registration from './components/account/Registration';
import CombatGeneralDetails from './components/combatDetails/CombatGeneralDetails';
import DetailsSpecificalCombat from './components/combatDetails/DetailsSpecificalCombat';
import GeneralAnalysis from './components/combatDetails/GeneralAnalysis';
import MainInformation from './components/combatDetails/MainInformation';
import Chats from './components/communication/chats/Chats';
import AllCommunities from './components/communication/community/AllCommunities';
import SelectedCommunity from './components/communication/community/SelectedCommunity';
import Feed from './components/communication/Feed';
import MyEnvironment from './components/communication/myEnvironment/MyEnvironment';
import People from './components/communication/people/People';
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
        path: '/feed',
        element: <Feed />
    },
    {
        path: '/chats',
        element: <Chats />
    },
    {
        path: '/communities',
        element: <AllCommunities />
    },
    {
        path: '/people',
        element: <People />
    },
    {
        path: '/environment',
        element: <MyEnvironment />
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
