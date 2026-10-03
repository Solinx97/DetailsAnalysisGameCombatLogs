import type { OptionModel } from '@/shared/types/OptionModel';
import React, { type Dispatch, type SetStateAction } from 'react';

interface WoWGameDataContextValue {
    t: (key: string) => string;
    username: string;
    setUsername: Dispatch<SetStateAction<string>>;
    serversOptions: OptionModel[];
    serverName: string;
    serverId: number;
    setServerName: Dispatch<SetStateAction<OptionModel | null>>;
    regionName: string;
}

const WoWGameDataContext = React.createContext<WoWGameDataContextValue | null>(null);

export default WoWGameDataContext;