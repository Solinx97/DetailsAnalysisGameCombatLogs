import { faBolt, faCircleNodes, faDatabase, faHourglassStart, faKhanda, faPlusCircle, faShieldHalved } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import type { CombatModel } from '../types/CombatModel';

interface GeneralAnalysisItemProps {
    uniqueCombats: CombatModel[];
    combatLogId: number;
    getValueShortName(value: number): string;
}

const getCombatDuration = (duration: string) => duration.substring(3);

const GeneralAnalysisItem: React.FC<GeneralAnalysisItemProps> = ({ uniqueCombats, combatLogId, getValueShortName }) => {
    const { t } = useTranslation("combatDetails/generalAnalysis");

    const navigate = useNavigate();

    const [selectedCombatIndex, setSelectedCombatIndex] = useState<number>(uniqueCombats.length - 1);
    const [selectedCombat, setSelectedCombat] = useState(uniqueCombats[selectedCombatIndex]);

    useEffect(() => {
        setSelectedCombat(uniqueCombats[selectedCombatIndex]);
    }, [selectedCombatIndex]);

    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        const hoursMins = `${date.getUTCHours()}:${date.getUTCMinutes()}:${date.getUTCSeconds()}`;

        return hoursMins;
    }

    if (selectedCombat === null) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="card">
            <ul className="unique-combats__all">
                {uniqueCombats.map((combat, index) => (
                    <li key={combat.id + 2} className={`unique-combats__${combat.isWin ? 'win' : 'lose'}`} onClick={() => setSelectedCombatIndex(index)}>
                        <div className="combat-number">{index + 1}</div>
                        <div className="combat-time">
                            <div className="combat-time__range">
                                <div>
                                    <div>{formatDate(combat.startDate)}</div>
                                </div>
                                <div>-</div>
                                <div>
                                    <div>{formatDate(combat.finishDate)}</div>
                                </div>
                            </div>
                            <div className="combat-time__lasts">
                                <div>{getCombatDuration(combat.duration)}</div>
                                <FontAwesomeIcon
                                    icon={faHourglassStart}
                                    className="list-group-item__duration"
                                    title={t("Duration") || ""}
                                />
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
            <div className="unique-combats__selected">
                <div className="combat-title">
                    <div className={`status combat-title__${selectedCombat.isWin ? 'win' : 'lose'}`}>
                        <div className="combat-number">{selectedCombatIndex + 1}</div>
                        <div className="card-body">
                            <h5 className="card-title">{selectedCombat.boss.name}</h5>
                            <p className="card-text">{selectedCombat.dungeonName}</p>
                        </div>
                    </div>
                </div>
                <div className="combat-time">
                    <div className="combat-time__range">
                        <div className="list-group-item">
                            <div>
                                <div>{formatDate(selectedCombat?.startDate)}</div>
                            </div>
                        </div>
                        <div>-</div>
                        <div className="list-group-item">
                            <div>
                                <div>{formatDate(selectedCombat?.finishDate)}</div>
                            </div>
                        </div>
                    </div>
                    <div className="combat-time__lasts">
                        <div>{getCombatDuration(selectedCombat.duration)}</div>
                        <FontAwesomeIcon
                            icon={faHourglassStart}
                            className="list-group-item__player-statistic-item"
                            title={t("Duration") || ""}
                        />
                    </div>
                </div>
            </div>
            <ul className="information">
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faKhanda}
                        className="list-group-item__player-statistic-item"
                        title={t("Damage") || ""}
                    />
                    <div>{getValueShortName(uniqueCombats[selectedCombatIndex].damageDone)}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faPlusCircle}
                        className="list-group-item__player-statistic-item"
                        title={t("Healing") || ""}
                    />
                    <div>{getValueShortName(uniqueCombats[selectedCombatIndex].healDone)}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faShieldHalved}
                        className="list-group-item__player-statistic-item"
                        title={t("DamageTaken") || ""}
                    />
                    <div>{getValueShortName(uniqueCombats[selectedCombatIndex].damageTaken)}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faBolt}
                        className="list-group-item__player-statistic-item"
                        title={t("ResourcesRecovery") || ""}
                    />
                    <div>{getValueShortName(uniqueCombats[selectedCombatIndex].resourcesRecovery)}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faCircleNodes}
                        className="list-group-item__player-statistic-item"
                        title={t("Buffs") || ""}
                    />
                    <div className="auras-details" onClick={() => navigate(`/general-analysis/auras?combat=${selectedCombat.id}&combatLog=${combatLogId}`)}>More...</div>
                </li>
            </ul>
            <div className="card-body details">
                <div className="btn-shadow" onClick={() => navigate(`/selected-combat?id=${uniqueCombats[selectedCombatIndex].id}&combatLogId=${combatLogId}&name=${uniqueCombats[selectedCombatIndex].boss.name}&number=${selectedCombatIndex + 1}&isWin=${selectedCombat.isWin}`)}>
                    <FontAwesomeIcon
                        icon={faDatabase}
                    />
                    <div>{t("MoreDetails")}</div>
                </div>
            </div>
        </div>
    );
}

export default GeneralAnalysisItem;