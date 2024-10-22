import Home from './components/Home';
import CombatGeneralDetails from './components/combatDetails/CombatGeneralDetails';
import DetailsSpecificalCombat from './components/combatDetails/DetailsSpecificalCombat';
import GeneralAnalysis from './components/combatDetails/GeneralAnalysis';
import MainInformation from './components/combatDetails/MainInformation';
import Feed from './components/communication/Feed';
import Chats from './components/communication/chats/Chats';
import AllCommunities from './components/communication/community/AllCommunities';
import SelectedCommunity from './components/communication/community/SelectedCommunity';
import CreateCommunity from './components/communication/create/CreateCommunity';
import CreateGroupChat from './components/communication/create/CreateGroupChat';
import MyEnvironment from './components/communication/myEnvironment/MyEnvironment';
import People from './components/communication/people/People';
import SelectedUser from './components/communication/people/SelectedUser';
import VoiceChat from './components/communication/voiceChat/VoiceChat';
import AuthorizationCallback from './components/identity/AuthorizationCallback';

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/callback',
        element: <AuthorizationCallback />
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
        path: '/chats/voice/:roomId/:chatName',
        element: <VoiceChat />
    },
    {
        path: '/chats/create',
        element: <CreateGroupChat />
    },
    {
        path: '/communities',
        element: <AllCommunities />
    },
    {
        path: '/communities/create',
        element: <CreateCommunity />
    },
    {
        path: '/people',
        element: <People />
    },
    {
        path: '/user',
        element: <SelectedUser />
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
];

export default AppRoutes;
