import { NoneValue } from '@/shared/helpers/ConstHelpers';
import useCombatLogs from '@/shared/hooks/useCombatLogs';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import Select from 'react-select';
import { useGetUniqueCombatsQuery, useGetUniqueUnitsNameQuery } from '../api/GameLogs.api';
import type { CombatModel } from '../types/CombatModel';
import { CombatUnitType } from '@/shared/helpers/EnumHelper';

type Option = {
    value: string;
    label: string;
}

type OptionNumber = {
    value: number;
    label: string;
}

interface DashboardFiltersData {
    filtersDOM: () => React.ReactNode;
    bossesValue: string;
    combatsValue: number;
    creatorsValue: string;
    targetsValue: string;
}

const useDashboardFilters = (combatLogId: number): DashboardFiltersData => {
    const { t } = useTranslation('combatDetails/dashboard');

    const [bossesOptions, setBossesOptions] = useState<Option[]>([]);
    const [bossesValue, setBossesValue] = useState<Option | null>(bossesOptions[0]);

    const [combatsOptions, setCombatsOptions] = useState<OptionNumber[]>([]);
    const [combatsValue, setCombatsValue] = useState<OptionNumber | null>(combatsOptions[0]);

    const [creatorsOptions, setCreatorsOptions] = useState<Option[]>([]);
    const [creatorsValue, setCreatorsValue] = useState<Option | null>(creatorsOptions[0]);

    const [targetsOptions, setTargetsOptions] = useState<Option[]>([]);
    const [targetsValue, setTargetsValue] = useState<Option | null>(targetsOptions[0]);

    const [creatorType, setCreatorType] = useState<number>(0);
    const [targetType, setTargetType] = useState<number>(0);

    const { removeServerName } = useCombatLogs();

    const { data: allUniqueCombats } = useGetUniqueCombatsQuery(combatLogId);
    const { data: creatorUniqueUnitNames } = useGetUniqueUnitsNameQuery({
        combatLogId,
        bossName: !bossesValue ? NoneValue.NONE_VALUE : bossesValue?.value
    });
    const { data: targetUniqueUnitNames } = useGetUniqueUnitsNameQuery({
        combatLogId,
        bossName: !bossesValue ? NoneValue.NONE_VALUE : bossesValue?.value
    });

    useEffect(() => {
        if (!creatorUniqueUnitNames || creatorUniqueUnitNames.length === 0) {
            return;
        }

        const options = creatorUniqueUnitNames.map(
            (item) => ({
                value: item.name,
                label: item.type === CombatUnitType["Player"]
                    ? `[p] ${removeServerName(item.name)}`
                    : item.type === CombatUnitType["Pet"]
                        ? `-p- ${item.name}`
                        : item.name
            })
        )

        options.unshift({ value: NoneValue.NONE_VALUE, label: t("All") });
        setCreatorsOptions(options);
    }, [creatorUniqueUnitNames]);

    useEffect(() => {
        if (!creatorUniqueUnitNames || creatorUniqueUnitNames.length === 0) {
            return;
        }

        let filtered = creatorUniqueUnitNames;
        if (creatorType === 1) {
            filtered = creatorUniqueUnitNames.filter(x => x.type === CombatUnitType["Player"]);
        }
        else if (creatorType === 2) {
            filtered = creatorUniqueUnitNames.filter(x => x.type !== CombatUnitType["Player"] && x.type !== CombatUnitType["PlayerCreature"]);
        }

        const options = filtered.map(
            (item) => ({
                value: item.name,
                label: removeServerName(item.name)
            })
        )

        options.unshift({ value: NoneValue.NONE_VALUE, label: t("All") });
        setCreatorsOptions(options);
    }, [creatorType]);

    useEffect(() => {
        if (!targetUniqueUnitNames || targetUniqueUnitNames.length === 0) {
            return;
        }

        const options = targetUniqueUnitNames.map(
            (item) => ({
                value: item.name,
                label: item.type === CombatUnitType["Player"]
                    ? `[p] ${removeServerName(item.name)}`
                    : removeServerName(item.name)
            })
        )

        options.unshift({ value: NoneValue.NONE_VALUE, label: t("All") });
        setTargetsOptions(options);
    }, [targetUniqueUnitNames]);

    useEffect(() => {
        if (!targetUniqueUnitNames || targetUniqueUnitNames.length === 0) {
            return;
        }

        let filtered = targetUniqueUnitNames;
        if (targetType === 1) {
            filtered = targetUniqueUnitNames.filter(x => x.type === CombatUnitType["Player"]);
        }
        else if (targetType === 2) {
            filtered = targetUniqueUnitNames.filter(x => x.type !== CombatUnitType["Player"] && x.type !== CombatUnitType["PlayerCreature"]);
        }

        const options = filtered.map(
            (item) => ({
                value: item.name,
                label: removeServerName(item.name)
            })
        )

        options.unshift({ value: NoneValue.NONE_VALUE, label: t("All") });
        setTargetsOptions(options);
    }, [targetType]);

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
        setBossesOptions(options);
    }, [allUniqueCombats]);

    useEffect(() => {
        if (!allUniqueCombats || bossesValue === undefined || bossesValue === null) {
            return;
        }

        const allUniqueCombatsMap = new Map(Object.entries(allUniqueCombats));
        if (bossesValue.value === NoneValue.NONE_VALUE) {
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

        const selectedBoss = allUniqueCombatsMap.get(bossesValue.value);
        if (!selectedBoss) {
            return;
        }

        const values = createCombatOptions([], selectedBoss);
        values.unshift({ value: 0, label: t("All") });
        setCombatsOptions(values);
    }, [allUniqueCombats, bossesValue]);

    useEffect(() => {
        if (!creatorUniqueUnitNames || creatorUniqueUnitNames.length === 0) {
            return;
        }

        const options = creatorUniqueUnitNames.map(
            (item) => ({
                value: item.name,
                label: item.name
            })
        )

        options.unshift({ value: NoneValue.NONE_VALUE, label: t("All") });
        setTargetsOptions(options);
    }, [creatorUniqueUnitNames]);

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
        setBossesOptions(options);
    }, [allUniqueCombats]);

    useEffect(() => {
        if (!allUniqueCombats || bossesValue === undefined || bossesValue === null) {
            return;
        }

        const allUniqueCombatsMap = new Map(Object.entries(allUniqueCombats));
        if (bossesValue.value === NoneValue.NONE_VALUE) {
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

        const selectedBoss = allUniqueCombatsMap.get(bossesValue.value);
        if (!selectedBoss) {
            return;
        }

        const values = createCombatOptions([], selectedBoss);
        values.unshift({ value: 0, label: t("All") });
        setCombatsOptions(values);
    }, [allUniqueCombats, bossesValue]);

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

    const filtersDOM = (): React.ReactNode => {
        return (
            <div className="dashboard__filter">
                <div className="filter-item">
                    <div>{t("Boss")}</div>
                    <Select<Option>
                        className="options"
                        options={bossesOptions}
                        value={bossesValue}
                        onChange={(selected) => setBossesValue(selected)}
                    />
                </div>
                <div className="filter-item">
                    <div>{t("Combat")}</div>
                    <Select<OptionNumber>
                        className="options"
                        options={combatsOptions}
                        value={combatsValue}
                        onChange={(selected) => setCombatsValue(selected)}
                    />
                </div>
                <div className="filter-item">
                    <div className="filter-item__extented">
                        <div>{t("Creator")}</div>
                        <div className="actions">
                            <button onClick={() => setCreatorType(0)}>any</button>
                            <button onClick={() => setCreatorType(1)}>ally</button>
                            <button onClick={() => setCreatorType(2)}>enemy</button>
                        </div>
                    </div>
                    <Select<Option>
                        className="options"
                        options={creatorsOptions}
                        value={creatorsValue}
                        onChange={(selected) => setCreatorsValue(selected)}
                    />
                </div>
                <div className="filter-item">
                    <div className="filter-item__extented">
                        <div>{t("Target")}</div>
                        <div className="actions">
                            <button onClick={() => setTargetType(0)}>any</button>
                            <button onClick={() => setTargetType(1)}>ally</button>
                            <button onClick={() => setTargetType(2)}>enemy</button>
                        </div>
                    </div>
                    <Select<Option>
                        className="options"
                        options={targetsOptions}
                        value={targetsValue}
                        onChange={(selected) => setTargetsValue(selected)}
                    />
                </div>
            </div>
        );
    }

    return {
        filtersDOM,
        bossesValue: !bossesValue ? NoneValue.NONE_VALUE : bossesValue.value,
        combatsValue: !combatsValue ? 0 : combatsValue.value,
        creatorsValue: !creatorsValue ? NoneValue.NONE_VALUE : creatorsValue.value,
        targetsValue: !targetsValue ? NoneValue.NONE_VALUE : targetsValue.value,
    };
}

export default useDashboardFilters;