import { lazy } from 'react';

type Route = {
    index?: boolean;
    path?: string;
    element: React.ReactNode;
}

const Home = lazy(() => import('../shared/components/Home'));
const VoiceChat = lazy(() => import('../features/voiceChat/components/VoiceChat'));
const UserFeed = lazy(() => import('../features/user/components/userEnvironment/UserFeed'));
const Profile = lazy(() => import('../features/user/components/userEnvironment/Profile'));
const Friends = lazy(() => import('../features/user/components/userEnvironment/Friends'));
const SelectedUser = lazy(() => import('../features/user/components/selectedUser/SelectedUser'));
const People = lazy(() => import('../features/user/components/people/People'));
const AuthorizationCallback = lazy(() => import('../features/user/components/identity/AuthorizationCallback'));
const SelectedCombat = lazy(() => import('../features/gameLogs/components/SelectedCombat'));
const GeneralAnalysis = lazy(() => import('../features/gameLogs/components/GeneralAnalysis'));
const GameCombatLogs = lazy(() => import('../features/gameLogs/components/GameCombatLogs'));
const CombatDetails = lazy(() => import('../features/gameLogs/components/details/CombatDetails'));
const CombatAuras = lazy(() => import('../features/gameLogs/components/auras/CombatAuras'));
const Feed = lazy(() => import('../features/feed/components/Feed'));
const UserEnvironmentCommunities = lazy(() => import('../features/community/components/userEnvironment/UserEnvironmentCommunities'));
const SelectedCommunity = lazy(() => import('../features/community/components/selectedCommunity/SelectedCommunity'));
const AllCommunities = lazy(() => import('../features/community/components/AllCommunities'));
const ChatApp = lazy(() => import('../features/chat/components/ChatApp'));
const SelectWoWGameData = lazy(() => import('../features/wowGameData/components/SelectWoWGameData'));
const BattleNetAuthorizationCallback = lazy(() => import('../features/wowGameData/components/auth/BattleNetAuthorizationCallback'));
const CombatReply = lazy(() => import('../features/gameLogs/components/reply/CombatReply'));
const PlayerDiethDetails = lazy(() => import('../features/gameLogs/components/player/PlayerDiethDetails'));

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
        path: '/game-combat-logs/:gameVersion',
        element: <GameCombatLogs />
    },
    {
        path: '/feed',
        element: <Feed />
    },
    {
        path: '/chats',
        element: <ChatApp />
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
        path: '/people/user/:userId',
        element: <SelectedUser />
    },
    {
        path: '/environment/feed',
        element: <UserFeed />
    },
    {
        path: '/environment/friends',
        element: <Friends />
    },
    {
        path: '/environment/communities',
        element: <UserEnvironmentCommunities />
    },
    {
        path: '/environment/profile',
        element: <Profile />
    },
    {
        path: '/community/:communityId',
        element: <SelectedCommunity />
    },
    {
        path: '/general-analysis/:combatLogId',
        element: <GeneralAnalysis />
    },
    {
        path: '/general-analysis/auras/:combatId',
        element: <CombatAuras />
    },
    {
        path: '/general-analysis/watch/:combatId',
        element: <CombatReply />
    },
    {
        path: '/selected-combat/:combatId',
        element: <SelectedCombat />
    },
    {
        path: '/combat-details/:combatId',
        element: <CombatDetails />
    },
    {
        path: '/player-dieth-details',
        element: <PlayerDiethDetails />
    },
    {
        path: '/wow-game-data',
        element: <SelectWoWGameData />
    },
    {
        path: '/wow-game-data/callback',
        element: <BattleNetAuthorizationCallback />
    },
];

export default AppRoutes;
