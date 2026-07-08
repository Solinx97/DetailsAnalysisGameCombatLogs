import { memo } from 'react';
import { useTranslation } from 'react-i18next';
import type { CombatModel } from "../../types//CombatModel";
import DashboardDurationItem from './DashboardDurationItem';
import DashboardItem from './DashboardItem';
import React from 'react';
import DashboardDamageDoneItem from './DashboardDamageDoneItem';
import DashboardHealDoneItem from './DashboardHealDoneItem';
import DashboardDeathItem from './DashboardDeathItem';
import { useGetCombatsDashboardQuery } from '../../api/GameLogs.api';

import './Dashboard.scss';

interface DashboardProps {
    combatLogId: number;
    allUniqueCombats: Map<string, CombatModel[]>;
}

const Dashboard: React.FC<DashboardProps> = ({ combatLogId, allUniqueCombats }) => {
    const { t } = useTranslation('combatDetails/dashboard');

    const { data: dashboards, isLoading } = useGetCombatsDashboardQuery(combatLogId);

    if (allUniqueCombats.size === 0 || isLoading || !dashboards) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="dashboard">
            <ul className="dashboard__items">
                <DashboardItem
                    dashboards={dashboards}
                    item={
                        <DashboardDurationItem
                            allUniqueCombats={allUniqueCombats}
                        />
                    }
                    name={t("CombatsAverageDuration")}
                    contentSize={dashboards.length}
                />
                <DashboardItem
                    dashboards={dashboards}
                    item={
                        <DashboardDamageDoneItem
                        />
                    }
                    name={t("CombatsAverageDPS")}
                    contentSize={dashboards.length}
                />
                <DashboardItem
                    dashboards={dashboards}
                    item={
                        <DashboardHealDoneItem
                        />
                    }
                    name={t("CombatsAverageHPS")}
                    contentSize={dashboards.length}
                />
                <DashboardItem
                    dashboards={dashboards}
                    item={
                        <DashboardDeathItem
                        />
                    }
                    name={t("CombatsAverageDeaths")}
                    contentSize={dashboards.length}
                />
            </ul>
        </div>
    );
}

export default memo(Dashboard);