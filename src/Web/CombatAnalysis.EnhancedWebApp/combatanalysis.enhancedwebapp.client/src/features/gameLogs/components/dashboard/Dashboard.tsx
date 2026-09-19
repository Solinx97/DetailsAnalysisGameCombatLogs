import Loading from '@/shared/components/Loading';
import { NoneValue } from '@/shared/helpers/ConstHelpers';
import { DashboardValueType } from '@/shared/helpers/EnumHelper';
import React, { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import Select from 'react-select';
import { useGetUniqueCombatPlayerNamesQuery, useGetUniqueCombatsByCombatLogIdQuery } from '../../api/GameLogs.api';
import type { CombatModel } from '../../types/CombatModel';
import DashboardItem from './DashboardItem';

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

type Option = {
    value: string;
    label: string;
}

type OptionNumber = {
    value: number;
    label: string;
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
        { requestName: "getDamageTaken", valueType: DashboardValueType.Value, name: t("DamageTaken") },
        { requestName: "getDamageTaken", valueType: DashboardValueType.AverageValue, name: t("AverageDamageTaken") },
        { requestName: "getDamageTaken", valueType: DashboardValueType.MaxValue, name: t("MaxDamageTaken") },
        { requestName: "getDamageTaken", valueType: DashboardValueType.MinValue, name: t("MinDamageTaken") },
        { requestName: "getDamageTaken", valueType: DashboardValueType.ValuePerSecond, name: t("DTPS") },
        { requestName: "getDamageTaken", valueType: DashboardValueType.AverageValuePerSecond, name: t("AverageDTPS") },
        { requestName: "getDamageTaken", valueType: DashboardValueType.MaxValuePerSecond, name: t("MaxDTPS") },
        { requestName: "getDamageTaken", valueType: DashboardValueType.MinValuePerSecond, name: t("MinDTPS") },
        { requestName: "getDamageSpells", valueType: 0, name: t("OverallDamageSpells") },
        { requestName: "getHealSpells", valueType: 0, name: t("OverallHealSpells") },
    ];

    const [uniqueBossesOptions, setUniqueBossesOptions] = useState<Option[]>([]);
    const [uniqueBossesValue, setUniqueBossesValue] = useState<Option | null>(uniqueBossesOptions[0]);

    const [combatsOptions, setCombatsOptions] = useState<OptionNumber[]>([]);
    const [combatsValue, setCombatsValue] = useState<OptionNumber | null>(combatsOptions[0]);

    const [combatPlayerNamesOptions, setCombatPlayerNamesOptions] = useState<Option[]>([]);
    const [combatPlayerNamesValue, setCombatPlayerNamesValue] = useState<Option | null>(combatPlayerNamesOptions[0]);

    const [selectedCombatId, setSelectedCombatId] = useState<number>(0);
    const [selectedUnitName, setSelectedUnitName] = useState<string>(NoneValue.NONE_VALUE);

    const [selectedDashboards, setSelectedDashboards] = useState<DashboardItemModel[]>([]);

    const { data: allUniqueCombats, isLoading } = useGetUniqueCombatsByCombatLogIdQuery(combatLogId);
    const { data: allUniquePlayers, isLoading: playersIsLoading } = useGetUniqueCombatPlayerNamesQuery(combatLogId);

    useEffect(() => {
        if (!allUniqueCombats) {
            return;
        }

        const allUniqueCombatsMap = new Map(Object.entries(allUniqueCombats));
        const options = Array.from(allUniqueCombatsMap.entries()).map(
            ([key, _]) => ({
                value: key,
                label: key
            })
        )

        options.unshift({ value: NoneValue.NONE_VALUE, label: t("All") });
        setUniqueBossesOptions(options);
    }, [allUniqueCombats]);

    useEffect(() => {
        if (!allUniquePlayers || allUniquePlayers.length === 0) {
            return;
        }

        const options = allUniquePlayers.map(
            (item) => ({
                value: item,
                label: item
            })
        )

        options.unshift({ value: NoneValue.NONE_VALUE, label: t("All") });
        setCombatPlayerNamesOptions(options);
    }, [allUniquePlayers]);

    useEffect(() => {
        if (!allUniqueCombats || uniqueBossesValue === undefined || uniqueBossesValue === null) {
            return;
        }

        const allUniqueCombatsMap = new Map(Object.entries(allUniqueCombats));
        if (uniqueBossesValue.value === NoneValue.NONE_VALUE) {
            let allValues: OptionNumber[] = [];
            Array.from(allUniqueCombatsMap.entries()).map(
                ([_, value]) => {
                    const values = createCombatOptions(allValues, value);
                    allValues = values;
                }
            )

            allValues.unshift({ value: 0, label: t("All") });
            setCombatsOptions(allValues);
            return;
        }

        const selectedBoss = allUniqueCombatsMap.get(uniqueBossesValue.value);
        if (!selectedBoss) {
            return;
        }

        const values = createCombatOptions([], selectedBoss);
        values.unshift({ value: 0, label: t("All") });
        setCombatsOptions(values);
    }, [allUniqueCombats, uniqueBossesValue]);

    useEffect(() => {
        if (!combatsValue) {
            return;
        }

        setSelectedCombatId(combatsValue.value);
    }, [combatsValue]);

    useEffect(() => {
        if (!combatPlayerNamesValue) {
            return;
        }

        setSelectedUnitName(combatPlayerNamesValue.value);
    }, [combatPlayerNamesValue]);

    const createCombatOptions = (array: OptionNumber[], selectedBoss: CombatModel[]) => {
        const options = selectedBoss.map(
            (item, index) => ({
                value: item.id,
                label: `[${index + 1}] ${item.boss.name}`
            })
        )

        const values = array.concat(options);
        return values;
    }

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

    if (!allUniqueCombats || isLoading || !allUniquePlayers || playersIsLoading) {
        return (<Loading />);
    }

    return (
        <div className="dashboard">
            <h5>{t("OverallByRaid")}</h5>
            <div className="dashboard__filter">
                <div>
                    <div>{t("Boss")}</div>
                    <Select<Option>
                        className="options"
                        options={uniqueBossesOptions}
                        value={uniqueBossesValue}
                        onChange={(selected) => setUniqueBossesValue(selected)}
                    />
                </div>
                <div>
                    <div>{t("Combat")}</div>
                    <Select<OptionNumber>
                        className="options"
                        options={combatsOptions}
                        value={combatsValue}
                        onChange={(selected) => setCombatsValue(selected)}
                    />
                </div>
                <div>
                    <div>{t("Player")}</div>
                    <Select<Option>
                        className="options"
                        options={combatPlayerNamesOptions}
                        value={combatPlayerNamesValue}
                        onChange={(selected) => setCombatPlayerNamesValue(selected)}
                    />
                </div>
            </div>
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
                            setCloseDashboardItem={() => closeDashboard(item)}
                            requestName={item.requestName}
                            valueType={item.valueType}
                            combatLogId={combatLogId}
                            combatId={selectedCombatId}
                            unitName={selectedUnitName}
                            name={item.name}
                        />
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default memo(Dashboard);