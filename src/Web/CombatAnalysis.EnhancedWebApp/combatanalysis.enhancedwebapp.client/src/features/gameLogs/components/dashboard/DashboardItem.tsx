import DashboardContext from '@/context/DashboardContext';
import useNumber from '@/shared/hooks/useNumber';
import React, { useState } from 'react';
import { type ReactNode} from 'react';
import { useTranslation } from 'react-i18next';
import type { DashboardModel } from '../../types/dashboard/DashboardModel';

interface DashboardItemProps {
    dashboards: DashboardModel[];
    item: ReactNode;
    name: string;
}

const DashboardItem: React.FC<DashboardItemProps> = ({ dashboards, item, name }) => {
    const minCount = 3;

    const { t } = useTranslation('combatDetails/dashboard');

    const [contentSize, setContentSize] = useState(minCount);
    const [itemCount, setItemCount] = useState(minCount);
    const [filter, setFilter] = useState(-1);

    const { formatNumber } = useNumber();
    
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
                setContentSize: setContentSize,
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