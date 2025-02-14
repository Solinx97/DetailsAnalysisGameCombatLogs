import Home from './components/Home';
import CombatDetails from './components/combatDetails/CombatDetails';
import DetailsSpecificalCombat from './components/combatDetails/DetailsSpecificalCombat';
import GeneralAnalysis from './components/combatDetails/GeneralAnalysis';
import MainInformation from './components/combatDetails/MainInformation';
import Feed from './components/communication/Feed';
import Chats from './components/communication/chats/Chats';
import VoiceChat from './components/communication/chats/voiceChat/VoiceChat';
import AllCommunities from './components/communication/community/AllCommunities';
import SelectedCommunity from './components/communication/community/SelectedCommunity';
import MyEnvironment from './components/communication/myEnvironment/MyEnvironment';
import People from './components/communication/people/People';
import SelectedUser from './components/communication/people/SelectedUser';
import AuthorizationCallback from './components/identity/AuthorizationCallback';
//import PlayerMovements from './components/combatDetails/actions/PlayerMovements';
import CombatAuras from './components/combatDetails/actions/CombatAuras';

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
        path: '/communities',
        element: <AllCommunities />
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
        path: '/general-analysis/auras',
        element: <CombatAuras />
    },
    {
        path: '/details-specifical-combat',
        element: <DetailsSpecificalCombat />
    },
    {
        path: '/combat-details',
        element: <CombatDetails />
    },
    //{
    //    path: '/player-movements',
    //    element: <PlayerMovements />
    //},
];

export default AppRoutes;
