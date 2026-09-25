import React from 'react';

interface WoWGameDataContextValue {
    t: (key: string) => string;
    username: string;
    serverName: string;
    regionName: string;
}

const WoWGameDataContext = React.createContext<WoWGameDataContextValue | null>(null);

export default WoWGameDataContext;