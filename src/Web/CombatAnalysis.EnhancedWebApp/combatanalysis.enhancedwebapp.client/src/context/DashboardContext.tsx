import type { DashboardModel } from '@/features/gameLogs/types/dashboard/DashboardModel';
import type { Dispatch, SetStateAction } from 'react';
import React from 'react';

interface DashboardContextValue {
    dashboards: DashboardModel[];
    setItemCount: Dispatch<SetStateAction<number>>;
    itemCount: number;
    setContentSize: Dispatch<SetStateAction<number>>;
    formatNumber: (value: number | string | undefined) => string;
    compare: (boardA: DashboardModel, boardB: DashboardModel) => number;
    setFilter: Dispatch<SetStateAction<number>>;
    filter: number;
}

const DashboardContext = React.createContext<DashboardContextValue | null>(null);

export default DashboardContext;