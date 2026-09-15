import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type JSX } from 'react';
import { useTranslation } from 'react-i18next';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import type { HealDoneGeneralModel } from '../../types/HealDoneGeneralModel';

interface HealDoneGeneralHelperProps {
    generalData: HealDoneGeneralModel[] | null;
    getProcentage: (firstValue: number, secondValue: number) => string;
    combatPlayer: CombatPlayerModel;
    getValueShortName: (value: number) => string;
    getSpellValueProcentage: (item: HealDoneGeneralModel, targetValue: number) => string;
}

const HealDoneGeneralHelper: React.FC<HealDoneGeneralHelperProps> = ({ generalData, getProcentage, combatPlayer, getValueShortName, getSpellValueProcentage }) => {
    const fixedNumberUntil = 2;

    const { t } = useTranslation("helpers/combatDetailsHelper");

    const [hideColumns, setHideColumns] = useState<string[]>([]);

    const handleAddToHideColumns = (columnName: string): void => {
        const hiddenCollumns = hideColumns;
        hiddenCollumns.push(columnName);

        setHideColumns(Array.from(hiddenCollumns));
    }

    const handleRemoveFromHideColumns = (columnName: string): void => {
        const hiddenCollumns = hideColumns;
        const newArray = hiddenCollumns.filter(item => item !== columnName);

        setHideColumns(Array.from(newArray));
    }

    const tableTitle = (): JSX.Element => {
        return (
            <li className="player-general-data-details__title" key="0">
                <ul>
                    <li>
                        {t("Spell")}
                    </li>
                    <li>
                        {t("Total")}
                    </li>
                    {!hideColumns.includes("Average") &&
                        <li className="allow-hide-column">
                            {t("Average")}
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Hide")}
                                onClick={() => handleAddToHideColumns("Average")}
                            />
                        </li>
                    }
                    <li>
                        {t("HPS")}
                    </li>
                    {!hideColumns.includes("Count") &&
                        <li className="allow-hide-column">
                            {t("Count")}
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Hide")}
                                onClick={() => handleAddToHideColumns("Count")}
                            />
                        </li>
                    }
                    {!hideColumns.includes("Crit") &&
                        <li className="allow-hide-column">
                            {t("Crit")}, %
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Hide")}
                                onClick={() => handleAddToHideColumns("Crit")}
                            />
                        </li>
                    }
                    {!hideColumns.includes("Max") &&
                        <li className="allow-hide-column">
                            {t("Max")}
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Hide")}
                                onClick={() => handleAddToHideColumns("Max")}
                            />
                        </li>
                    }
                    {!hideColumns.includes("Min") &&
                        <li className="allow-hide-column">
                            {t("Min")}
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Hide")}
                                onClick={() => handleAddToHideColumns("Min")}
                            />
                        </li>
                    }
                </ul>
            </li>
        );
    }

    const hiddenColumns = (): JSX.Element => {
        return (
            <ul className="hidden-columns">
                {hideColumns.map((column, index) => (
                    <li key={index} className="allow-hide-column" onClick={() => handleRemoveFromHideColumns(column)}>
                        {t(column)}
                    </li>
                ))}
            </ul>
        );
    }

    return (
        <>
            <li className="player-general-data-details__inherit">
                <div>
                    {t("Total")}: {getValueShortName(combatPlayer.unit.unitInfo.healDone)}
                </div>
                {hideColumns.length > 0 && hiddenColumns()}
            </li>
            {tableTitle()}
            {generalData?.map((item) => (
                    <li className="player-general-data-details__item" key={item.id}>
                        <ul>
                            <li>
                                {item.spell}
                            </li>
                            <li className="amount">
                                <span>{getValueShortName(item.value)}</span>
                                <span className="procentage">{getSpellValueProcentage(item, combatPlayer.unit.unitInfo.healDone)}%</span>
                            </li>
                            {!hideColumns.includes("Average") &&
                                <li>
                                    {getValueShortName(item.averageValue)}
                                </li>
                            }
                            <li>
                                {item.healPerSecond.toFixed(fixedNumberUntil)}
                            </li>
                            {!hideColumns.includes("Count") &&
                                <li>
                                    {item.castNumber}
                                </li>
                            }
                            {!hideColumns.includes("Crit") &&
                                <li>
                                    {getProcentage(item.critNumber, item.castNumber)}%
                                </li>
                            }
                            {!hideColumns.includes("Max") &&
                                <li>
                                    {getValueShortName(item.maxValue)}
                                </li>
                            }
                            {!hideColumns.includes("Min") &&
                                <li>
                                    {getValueShortName(item.minValue)}
                                </li>
                            }
                        </ul>
                    </li>
                ))
            }
        </>
    );
}

export default HealDoneGeneralHelper;