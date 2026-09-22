import { CombatUnitType } from '@/shared/helpers/EnumHelper';
import useCombatLogs from '@/shared/hooks/useCombatLogs';
import useNumber from '@/shared/hooks/useNumber';
import { faClose, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetDashboardQuery } from '../../api/GameLogs.api';
import type { DashboardItemModel } from '../../types/dashboard/DashboardItemModel';
import type { DashboardModel } from '../../types/dashboard/DashboardModel';

interface DashboardItemProps {
    requestName: string;
    valueType: number;
    combatLogId: number;
    bossName: string,
    combatId: number;
    creatorName: string,
    targetName: string;
    name: string;
    refetch: Object | null;
    setCloseDashboardItem: () => void;
}

const DashboardItem: React.FC<DashboardItemProps> = ({ requestName, valueType, combatLogId, bossName, combatId, creatorName, targetName, name, refetch, setCloseDashboardItem }) => {
    const minCount = 5;

    const { t } = useTranslation('combatDetails/dashboard');

    const { formatNumber } = useNumber();
    const { removeServerName } = useCombatLogs();

    const [useGetDashboard] = useLazyGetDashboardQuery();

    const [dashboard, setDashboard] = useState<DashboardModel | null>(null);
    const [onlyPlayers, setOnlyPlayers] = useState(false);
    const [contentSize, setContentSize] = useState(minCount);
    const [dashboardItems, setDashboardItems] = useState<DashboardItemModel[]>([]);

    useEffect(() => {
        loadAsync();
    }, []);

    useEffect(() => {
        setDashboard(null);
        loadAsync();
    }, [refetch]);

    useEffect(() => {
        if (!dashboard || onlyPlayers || dashboard.items.length === 0) {
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

    const loadAsync = async () => {
        try {
            const receivedDashboard = await useGetDashboard({ dahsboardName: requestName, combatLogId, bossName, combatId, creatorName, targetName, valueType }).unwrap();
            setDashboard(receivedDashboard);
            setDashboardItems(receivedDashboard.items);
        } catch (error) {
            console.error("Failed to fetch dashboard:", error);
        }
    }

    if (!dashboard) {
        return (
            <>
                <div className="header">
                    <div>{name}</div>
                    <FontAwesomeIcon
                        className="close"
                        icon={faClose}
                        onClick={setCloseDashboardItem}
                    />
                </div>
                <span className="content">
                    Loading...
                </span>
            </>
        );
    }

    if (dashboardItems.length === 0) {
        return (
            <>
                <div className="header">
                    <div>{name}</div>
                    <FontAwesomeIcon
                        className="close"
                        icon={faClose}
                        onClick={setCloseDashboardItem}
                    />
                </div>
                <span>
                    {t("NoData")}
                </span>
            </>
        );
    }

    return (
        <>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setOnlyPlayers((item) => !item)} />
                <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{onlyPlayers ? t("Any") : t("Players")}</label>
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
                                <div className="type">
                                    {item.unitType === CombatUnitType["Player"] &&
                                        <FontAwesomeIcon
                                            icon={faUser}
                                        />
                                    }
                                    <span>{removeServerName(item.valueName)}</span>
                                </div>
                                <div>{formatNumber(item.value)}</div>
                            </li>
                        ))
                        : dashboardItems.map((item, index) => (
                            <li key={index} className="details-item">
                                <div className="type">
                                    {item.unitType === CombatUnitType["Player"] &&
                                        <FontAwesomeIcon
                                            icon={faUser}
                                        />
                                    }
                                    <span>{removeServerName(item.valueName)}</span>
                                </div>
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