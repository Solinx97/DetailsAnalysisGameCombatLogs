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
import Friends from './components/communication/myEnvironment/Friends';
import MyCommunities from './components/communication/myEnvironment/MyCommunities';
import MyFeed from './components/communication/myEnvironment/MyFeed';
import Profile from './components/communication/myEnvironment/Profile';
import People from './components/communication/people/People';
import SelectedUser from './components/communication/people/SelectedUser';
import AuthorizationCallback from './components/identity/AuthorizationCallback';
//import PlayerMovements from './components/combatDetails/actions/PlayerMovements';
import CombatAuras from './components/combatDetails/actions/CombatAuras';

interface Route {
    index?: boolean;
    path?: string;
    element: React.ReactNode;
}

const AppRoutes: Route[] = [
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
        path: '/environment/feed',
        element: <MyFeed />
    },
    {
        path: '/environment/friends',
        element: <Friends />
    },
    {
        path: '/environment/communities',
        element: <MyCommunities />
    },
    {
        path: '/environment/profile',
        element: <Profile />
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
    // {
    //     path: '/player-movements',
    //     element: <PlayerMovements />
    // },
];

export default AppRoutes;
