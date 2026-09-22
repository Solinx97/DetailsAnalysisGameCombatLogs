import { DashboardValueType } from '@/shared/helpers/EnumHelper';
import React, { memo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import DashboardItem from './DashboardItem';
import useDashboardFilters from '../../hooks/useDashboardFilters';

import './Dashboard.scss';

interface DashboardItemValue {
    requestName: string;
    valueType: number;
    name: string;
}

interface DashboardItemModel {
    requestName: string;
    valueType: number;
    name: string;
}

const Dashboard: React.FC<{ combatLogId: number }> = ({ combatLogId }) => {
    const { t } = useTranslation('combatDetails/dashboard');

    const dashboards: DashboardItemValue[] = [
        { requestName: "getDamage", valueType: DashboardValueType.Value, name: t("Damage") },
        { requestName: "getDamage", valueType: DashboardValueType.AverageValue, name: t("AverageDamage") },
        { requestName: "getDamage", valueType: DashboardValueType.MaxValue, name: t("MaxDamage") },
        { requestName: "getDamage", valueType: DashboardValueType.MinValue, name: t("MinDamage") },
        { requestName: "getDamage", valueType: DashboardValueType.ValuePerSecond, name: t("DPS") },
        { requestName: "getDamage", valueType: DashboardValueType.AverageValuePerSecond, name: t("AverageDPS") },
        { requestName: "getDamage", valueType: DashboardValueType.MaxValuePerSecond, name: t("MaxDPS") },
        { requestName: "getDamage", valueType: DashboardValueType.MinValuePerSecond, name: t("MinDPS") },
        { requestName: "getHeal", valueType: DashboardValueType.Value, name: t("Heal") },
        { requestName: "getHeal", valueType: DashboardValueType.AverageValue, name: t("AverageHeal") },
        { requestName: "getHeal", valueType: DashboardValueType.MaxValue, name: t("MaxHeal") },
        { requestName: "getHeal", valueType: DashboardValueType.MinValue, name: t("MinHeal") },
        { requestName: "getHeal", valueType: DashboardValueType.ValuePerSecond, name: t("HPS") },
        { requestName: "getHeal", valueType: DashboardValueType.AverageValuePerSecond, name: t("AverageHPS") },
        { requestName: "getHeal", valueType: DashboardValueType.MaxValuePerSecond, name: t("MaxHPS") },
        { requestName: "getHeal", valueType: DashboardValueType.MinValuePerSecond, name: t("MinHPS") },
        { requestName: "getDamageSpells", valueType: 0, name: t("OverallDamageSpells") },
        { requestName: "getHealSpells", valueType: 0, name: t("OverallHealSpells") },
    ];

    const [selectedDashboards, setSelectedDashboards] = useState<DashboardItemModel[]>([]);
    
    const { filtersDOM, bossesValue, combatsValue, creatorsValue, targetsValue, refetch } = useDashboardFilters(combatLogId);

    const addDashboard = (requestName: string, valueType: number, name: string) => {
        setSelectedDashboards(prev => [
            ...prev,
            {
                requestName,
                valueType,
                name
            }
        ]);
    }

    const closeDashboard = (element: DashboardItemModel) => {
        const alreadySelectedDashboards = Array.from(selectedDashboards);
        const actualElementIndex = selectedDashboards.indexOf(element);
        alreadySelectedDashboards.splice(actualElementIndex, 1);

        setSelectedDashboards(alreadySelectedDashboards);
    }

    return (
        <div className="dashboard">
            <h5>{t("OverallByRaid")}</h5>
            {filtersDOM()}
            <div className="dashboard__list">
                <div>{t("AvailableDashboards")}</div>
                <ul className="dashboard-title-list">
                    {dashboards.map((item, index) => (
                        <li key={index} onClick={() => addDashboard(item.requestName, item.valueType, item.name)}>{item.name}</li>
                    ))
                    }
                </ul>
            </div>
            <ul className="dashboard__items">
                {selectedDashboards.map((item, index) => (
                    <li key={index} className="item">
                        <DashboardItem
                            requestName={item.requestName}
                            valueType={item.valueType}
                            combatLogId={combatLogId}
                            bossName={bossesValue}
                            combatId={combatsValue}
                            creatorName={creatorsValue}
                            targetName={targetsValue}
                            name={item.name}
                            setCloseDashboardItem={() => closeDashboard(item)}
                            refetch={refetch}
                        />
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default memo(Dashboard);