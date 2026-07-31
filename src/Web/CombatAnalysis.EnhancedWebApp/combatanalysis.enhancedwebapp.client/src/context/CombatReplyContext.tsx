import type { Dispatch, SetStateAction } from 'react';
import React from 'react';

interface CombatReplyContextValue {
    t: (key: string) => string;
    selectedGameId: string;
    setSelectedGameId: Dispatch<SetStateAction<string>>;
    selectedTargetGameId: string;
    setSelectedTargetGameId: Dispatch<SetStateAction<string>>;
    currentTime: number;
    colors: Map<string, string>;
}

const CombatReplyContext = React.createContext<CombatReplyContextValue | null>(null);

export default CombatReplyContext;