import { faBolt, faCircleNodes, faDatabase, faHourglassStart, faKhanda, faPlusCircle, faShieldHalved, faLocationCrosshairs } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import type { CombatModel } from '../types/CombatModel';
import GeneralAnalysisCombats from './GeneralAnalysisCombats';

interface GeneralAnalysisItemProps {
    uniqueCombats: CombatModel[];
    combatLogId: number;
    getValueShortName(value: number): string;
}

const GeneralAnalysisItem: React.FC<GeneralAnalysisItemProps> = ({ uniqueCombats, combatLogId, getValueShortName }) => {
    const { t } = useTranslation("combatDetails/generalAnalysis");

    const navigate = useNavigate();

    const [selectedCombatIndex, setSelectedCombatIndex] = useState<number>(uniqueCombats.length - 1);
    const [selectedCombat, setSelectedCombat] = useState<CombatModel>(uniqueCombats[selectedCombatIndex]);

    useEffect(() => {
        setSelectedCombat(uniqueCombats[selectedCombatIndex]);
    }, [selectedCombatIndex]);

    const getCombatDuration = (duration: string) => duration.substring(3);

    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        const hoursMins = `${date.getUTCHours()}:${date.getUTCMinutes()}:${date.getUTCSeconds()}`;

        return hoursMins;
    }

    const getTotalSeconds = (duration: string) => {
        const [hours, minutes, seconds] = duration.split(":").map(Number);

        const totalSeconds = hours * 3600 + minutes * 60 + seconds;

        return totalSeconds;
    }

    if (selectedCombat === null) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="card">
            <GeneralAnalysisCombats
                uniqueCombats={uniqueCombats}
                setSelectedCombatIndex={setSelectedCombatIndex}
                t={t}
            />
            <div className="unique-combats__selected">
                <div className="combat-title">
                    <div className={`status combat-title__${selectedCombat.isWin ? 'win' : 'lose'}`}>
                        <div className="combat-number">{selectedCombatIndex + 1}</div>
                        <div className="card-body">
                            <h5 className="card-title">{selectedCombat.boss.name}</h5>
                            <p className="card-text">{selectedCombat.dungeonName}</p>
                        </div>
                    </div>
                    <div className="see-reply btn-shadow" onClick={() => navigate(`/general-analysis/reply?combat=${selectedCombat.id}&combatLog=${combatLogId}`)}>
                        <FontAwesomeIcon
                            icon={faLocationCrosshairs}
                        />
                        <div>{t("Reply")}</div>
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
                    <div>{t("DPS")}</div>
                    <FontAwesomeIcon
                        icon={faKhanda}
                        className="list-group-item__player-statistic-item"
                        title={t("Damage") || ""}
                    />
                    <div>{getValueShortName(selectedCombat.damageDone)}</div>
                </li>
                <li className="list-group-item">
                    <div>{t("HPS")}</div>
                    <FontAwesomeIcon
                        icon={faPlusCircle}
                        className="list-group-item__player-statistic-item"
                        title={t("Healing") || ""}
                    />
                    <div>{getValueShortName(selectedCombat.healDone)}</div>
                </li>
                <li className="list-group-item">
                    <div>{t("DamageTaken")}</div>
                    <FontAwesomeIcon
                        icon={faShieldHalved}
                        className="list-group-item__player-statistic-item"
                        title={t("DamageTaken") || ""}
                    />
                    <div>{getValueShortName(selectedCombat.damageTaken)}</div>
                </li>
                <li className="list-group-item">
                    <div>{t("ResourcesRecovery")}</div>
                    <FontAwesomeIcon
                        icon={faBolt}
                        className="list-group-item__player-statistic-item"
                        title={t("ResourcesRecovery") || ""}
                    />
                    <div>{getValueShortName(selectedCombat.resourcesRecovery)}</div>
                </li>
                <li className="list-group-item buffs"
                    onClick={() => navigate(`/general-analysis/auras?combat=${selectedCombat.id}&combatLog=${combatLogId}`)}>
                    <FontAwesomeIcon
                        icon={faCircleNodes}
                        className="list-group-item__player-statistic-item"
                        title={t("Buffs") || ""}
                    />
                    <div>{t("Buffs")}</div>
                </li>
            </ul>
            <div className="card-body details">
                <div className="btn-shadow"
                    onClick={() => navigate(`/selected-combat?id=${selectedCombat.id}&combatLogId=${combatLogId}&name=${selectedCombat.boss.name}&number=${selectedCombatIndex + 1}&isWin=${selectedCombat.isWin}&duration=${getTotalSeconds(selectedCombat.duration)}`)}>
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