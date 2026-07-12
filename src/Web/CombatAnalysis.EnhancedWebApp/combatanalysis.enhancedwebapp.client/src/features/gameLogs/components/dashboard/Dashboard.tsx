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
import DashboardDamageSpellsItem from './DashboardDamageSpellsItem';
import DashboardHealSpellsItem from './DashboardHealSpellsItem';
import Loading from '@/shared/components/Loading.tsx';

import './Dashboard.scss';
import DashboardPotionsItem from './DashboardPotionsItem';

interface DashboardProps {
    combatLogId: number;
    allUniqueCombats: Map<string, CombatModel[]>;
}

const Dashboard: React.FC<DashboardProps> = ({ combatLogId, allUniqueCombats }) => {
    const { t } = useTranslation('combatDetails/dashboard');

    const { data: dashboards, isLoading } = useGetCombatsDashboardQuery(combatLogId);

    if (allUniqueCombats.size === 0 || isLoading || !dashboards) {
        return (<Loading />);
    }

    return (
        <div className="dashboard">
            <h5>{t("OverallByRaid")}</h5>
            <ul className="dashboard__items">
                <DashboardItem
                    dashboards={dashboards}
                    item={
                        <DashboardDurationItem
                            allUniqueCombats={allUniqueCombats}
                        />
                    }
                    name={t("AverageDuration")}
                />
                <DashboardItem
                    dashboards={dashboards}
                    item={
                        <DashboardDamageDoneItem
                        />
                    }
                    name={t("AverageDPS")}
                />
                <DashboardItem
                    dashboards={dashboards}
                    item={
                        <DashboardHealDoneItem
                        />
                    }
                    name={t("AverageHPS")}
                />
                <DashboardItem
                    dashboards={dashboards}
                    item={
                        <DashboardDeathItem
                        />
                    }
                    name={t("OverallDeaths")}
                />
                <DashboardItem
                    dashboards={dashboards}
                    item={
                        <DashboardDamageSpellsItem
                            combatLogId={combatLogId}
                        />
                    }
                    name={t("OverallDamageSpells")}
                />
                <DashboardItem
                    dashboards={dashboards}
                    item={
                        <DashboardHealSpellsItem
                            combatLogId={combatLogId}
                        />
                    }
                    name={t("OverallHealSpells")}
                />
                <DashboardItem
                    dashboards={dashboards}
                    item={
                        <DashboardPotionsItem
                            combatLogId={combatLogId}
                        />
                    }
                    name={t("OverallEfficiencyPotionsUsed")}
                />
            </ul>
        </div>
    );
}

export default memo(Dashboard);