import type { DashboardModel } from '@/features/gameLogs/types/dashboard/DashboardModel';
import type { Dispatch, SetStateAction } from 'react';
import React from 'react';

interface DashboardContextValue {
    dashboards: DashboardModel[];
    dashboardsSize: number;
    setDashboardsSize: Dispatch<SetStateAction<number>>;
    contentSize: number;
    setContentSize: Dispatch<SetStateAction<number>>;
    formatNumber: (value: number | string | undefined) => string;
    compare: (boardA: DashboardModel, boardB: DashboardModel) => number;
    setFilter: Dispatch<SetStateAction<number>>;
    filter: number;
    onlyPlayers: boolean;
}

const DashboardContext = React.createContext<DashboardContextValue | null>(null);

export default DashboardContext;