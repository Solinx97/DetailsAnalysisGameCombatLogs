import type { OptionMode } from '@/shared/types/OptionMode';
import React, { type Dispatch, type SetStateAction } from 'react';

interface WoWGameDataContextValue {
    t: (key: string) => string;
    username: string;
    setUsername: Dispatch<SetStateAction<string>>;
    serversOptions: OptionMode[];
    serverName: string;
    serverId: number;
    setServerName: Dispatch<SetStateAction<OptionMode | null>>;
    regionName: string;
}

const WoWGameDataContext = React.createContext<WoWGameDataContextValue | null>(null);

export default WoWGameDataContext;