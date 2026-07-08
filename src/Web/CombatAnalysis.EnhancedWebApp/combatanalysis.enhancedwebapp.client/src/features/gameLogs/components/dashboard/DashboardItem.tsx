import React, { useState } from 'react';
import { type Dispatch, type ReactNode, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { DashboardModel } from '../../types/dashboard/DashboardModel';

interface DashboardContextValue {
    dashboards: DashboardModel[];
    setItemCount: Dispatch<SetStateAction<number>>;
    itemCount: number;
    formatNumber: (value: number | string | undefined) => string;
    compare: (boardA: DashboardModel, boardB: DashboardModel) => number;
    setFilter: Dispatch<SetStateAction<number>>;
    filter: number;
}

export const DashboardContext = React.createContext<DashboardContextValue | null>(null);

interface DashboardItemProps {
    dashboards: DashboardModel[];
    item: ReactNode;
    name: string;
    contentSize: number;
}

const DashboardItem: React.FC<DashboardItemProps> = ({ dashboards, item, name, contentSize }) => {
    const minCount = 3;

    const { t } = useTranslation('combatDetails/dashboard');

    const [itemCount, setItemCount] = useState(minCount);
    const [filter, setFilter] = useState(-1);

    const formatNumber = (value: number | string | undefined): string => {
        if (value == null) {
            return "";
        }

        const num = Number(value);

        if (num >= 1_000_000_000) {
            return `${(num / 1_000_000_000).toFixed(1)}B`;
        }

        if (num >= 1_000_000) {
            return `${(num / 1_000_000).toFixed(1)}M`;
        }

        if (num >= 1_000) {
            return `${(num / 1_000).toFixed(1)}K`;
        }

        return num.toString();
    }

    const compare = (boardA: DashboardModel, boardB: DashboardModel): number => {
        const keys: (keyof DashboardModel)[] = ['averageDPS', 'averageHPS', 'averageDeaths'];
        const key = keys[filter < 0 ? 0 : filter];

        if (boardA[key] > boardB[key]) {
            return -1;
        }
        if (boardA[key] < boardB[key]) {
            return 1;
        }

        return 0;
    }

    return (
        <DashboardContext.Provider
            value={{
                dashboards: dashboards,
                setItemCount: setItemCount,
                itemCount: itemCount,
                formatNumber: formatNumber,
                compare: compare,
                setFilter: setFilter,
                filter: filter,
            }}
        >
            <li className="item">
                <div className="header">{name}</div>
                <span className="content">{item}</span>
                <div className="extend" onClick={() => setItemCount(itemCount === minCount ? contentSize : minCount)}>
                    {itemCount === minCount ? t("More") : t("Less")}
                </div>
            </li>
        </DashboardContext.Provider>
    );
}

export default DashboardItem;