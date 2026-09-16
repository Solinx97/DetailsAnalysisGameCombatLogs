import DashboardContext from '@/context/DashboardContext';
import useNumber from '@/shared/hooks/useNumber';
import React, { useEffect, useState } from 'react';
import { type ReactNode } from 'react';
import { useTranslation } from 'react-i18next';
import type { DashboardModel } from '../../types/dashboard/DashboardModel';
import { CombatUnitType } from '@/shared/helpers/EnumHelper';

interface DashboardItemProps {
    dashboards: DashboardModel[];
    item: ReactNode;
    name: string;
}

const DashboardItem: React.FC<DashboardItemProps> = ({ dashboards, item, name }) => {
    const minCount = 3;

    const { t } = useTranslation('combatDetails/dashboard');

    const [onlyPlayers, setOnlyPlayers] = useState(false);
    const [contentSize, setContentSize] = useState(minCount);
    const [dashboardsSize, setDashboardsSize] = useState(dashboards.length);
    const [filter, setFilter] = useState(-1);
    const [filteredDashboardItem, setFilteredDashboardItem] = useState<DashboardModel[]>([]);

    const { formatNumber } = useNumber();

    const compare = (boardA: DashboardModel, boardB: DashboardModel): number => {
        const keys: (keyof DashboardModel)[] = ['averageDPS', 'averageHPS', 'deathCount'];
        const key = keys[filter < 0 ? 0 : filter];

        if (boardA[key] > boardB[key]) {
            return -1;
        }
        if (boardA[key] < boardB[key]) {
            return 1;
        }

        return 0;
    }

    useEffect(() => {
        if (!dashboards) {
            return;
        }

        if (onlyPlayers) {
            const result = Array.from([...dashboards].sort(compare).filter(x => x.type === CombatUnitType["Player"]).slice(0, contentSize));
            setFilteredDashboardItem([...result]);
        }
        else {
            const result = Array.from([...dashboards].sort(compare).slice(0, contentSize));
            setFilteredDashboardItem([...result]);
        }
    }, [filter, contentSize, onlyPlayers, dashboards]);

    return (
        <DashboardContext.Provider
            value={{
                dashboards: filteredDashboardItem,
                dashboardsSize: dashboardsSize,
                setDashboardsSize: setDashboardsSize,
                contentSize: contentSize,
                setContentSize: setContentSize,
                formatNumber: formatNumber,
                compare: compare,
                setFilter: setFilter,
                filter: filter,
                onlyPlayers: onlyPlayers
            }}
        >
            <li className="item">
                <div className="form-check form-switch">
                    <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setOnlyPlayers((item) => !item)} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{onlyPlayers ? t("AnyUnits") : t("OnlyPlayers")}</label>
                </div>
                <div className="header">{name}</div>
                <span className="content">{item}</span>
                <div className="extend" onClick={() => setContentSize(contentSize === minCount ? dashboardsSize : minCount)}>
                    {contentSize === minCount ? t("More") : t("Less")}
                </div>
            </li>
        </DashboardContext.Provider>
    );
}

export default DashboardItem;