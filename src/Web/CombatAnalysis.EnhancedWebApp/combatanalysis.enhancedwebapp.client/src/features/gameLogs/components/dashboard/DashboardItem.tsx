import { CombatUnitType } from '@/shared/helpers/EnumHelper';
import useNumber from '@/shared/hooks/useNumber';
import { faClose } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetDashboardQuery } from '../../api/GameLogs.api';
import type { DashboardItemModel } from '../../types/dashboard/DashboardItemModel';

interface DashboardItemProps {
    requestName: string;
    valueType: number;
    combatLogId: number;
    combatId: number;
    unitName: string;
    name: string;
    setCloseDashboardItem: () => void;
}

const DashboardItem: React.FC<DashboardItemProps> = ({ requestName, valueType, combatLogId, combatId, unitName, name, setCloseDashboardItem }) => {
    const minCount = 5;

    const { t } = useTranslation('combatDetails/dashboard');

    const { formatNumber } = useNumber();

    const { data: dashboard, isLoading } = useGetDashboardQuery({ dahsboardName: requestName, combatLogId, combatId, unitName, valueType });

    const [onlyPlayers, setOnlyPlayers] = useState(false);
    const [contentSize, setContentSize] = useState(minCount);
    const [dashboardItems, setDashboardItems] = useState<DashboardItemModel[]>([]);

    useEffect(() => {
        if (!dashboard || onlyPlayers) {
            return;
        }

        setDashboardItems(dashboard.items.slice(0, contentSize));
    }, [onlyPlayers, dashboard, contentSize]);

    useEffect(() => {
        if (!dashboard || !onlyPlayers) {
            return;
        }

        setDashboardItems(dashboard.items.filter(x => x.unitType === CombatUnitType["Player"]).slice(0, contentSize));
    }, [onlyPlayers, dashboard, contentSize]);

    if (isLoading || !dashboard) {
        return (<div>Loading...</div>);
    }

    return (
        <>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setOnlyPlayers((item) => !item)} />
                <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{onlyPlayers ? t("AnyUnits") : t("OnlyPlayers")}</label>
            </div>
            <div className="header">
                <div>{name}</div>
                <FontAwesomeIcon
                    className="close"
                    icon={faClose}
                    onClick={setCloseDashboardItem}
                />
            </div>
            <span className="content">
                <ul className="details">
                    {onlyPlayers
                        ? dashboardItems.filter(x => x.unitType === CombatUnitType["Player"]).map((item, index) => (
                            <li key={index} className="details-item">
                                <div>{item.valueName}</div>
                                <div>{formatNumber(item.value)}</div>
                            </li>
                        ))
                        : dashboardItems.map((item, index) => (
                            <li key={index} className="details-item">
                                <div>{item.valueName}</div>
                                <div>{formatNumber(item.value)}</div>
                            </li>
                        ))
                    }
                </ul>
            </span>
            <div className="extend" onClick={() => setContentSize(contentSize === minCount ? dashboard.items.length : minCount)}>
                {contentSize === minCount ? t("More") : t("Less")}
            </div>
        </>
    );
}

export default DashboardItem;