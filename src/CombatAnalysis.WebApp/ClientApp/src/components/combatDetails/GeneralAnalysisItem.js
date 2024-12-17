import { faBolt, faCheck, faCircleNodes, faClock, faDatabase, faGraduationCap, faHourglassStart, faKhanda, faPlusCircle, faShieldHalved, faSkull } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { format } from 'date-fns';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

const formatDate = (dateString) => format(new Date(dateString), 'HH:mm');
const getCombatDuration = (duration) => duration.substring(3);

const GeneralAnalysisItem = ({ uniqueCombats, combatLogId }) => {
    const { t } = useTranslation("combatDetails/generalAnalysis");

    const navigate = useNavigate();

    const [selectedCombatIndex, setSelectedCombatIndex] = useState(0);
    const [selectedCombat, setSelectedCombat] = useState(uniqueCombats[selectedCombatIndex]);

    useEffect(() => {
        setSelectedCombat(uniqueCombats[selectedCombatIndex]);
    }, [selectedCombatIndex]);

    if (selectedCombat === null) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="card">
            <ul className="unique-combats__all">
                {uniqueCombats?.map((combat, index) => (
                    <li key={combat.id + 2} className={`unique-combats__${combat.isWin ? 'win' : 'lose'}`} onClick={() => setSelectedCombatIndex(index)}>
                        <div className="combat-number">{index + 1}</div>
                        <div className="combat-time">
                            <div className="combat-time__range">
                                <div>
                                    <div>
                                        <div>{formatDate(new Date(combat.startDate), 'HH:mm')}</div>
                                    </div>
                                </div>
                                <div>-</div>
                                <div>
                                    <div>
                                        <div>{formatDate(new Date(combat.finishDate), 'HH:mm')}</div>
                                    </div>
                                </div>
                            </div>
                            <div className="combat-time__lasts">
                                <div>{getCombatDuration(combat.duration)}</div>
                                <FontAwesomeIcon
                                    icon={faHourglassStart}
                                    className="list-group-item__duration"
                                    title={t("Duration")}
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
                            <h5 className="card-title">{selectedCombat.name}</h5>
                            <p className="card-text">{selectedCombat.dungeonName}</p>
                        </div>
                        {selectedCombat?.isWin
                            ? <FontAwesomeIcon
                                icon={faGraduationCap}
                                className="win"
                                title={t("Win")}
                            />
                            : <FontAwesomeIcon
                                icon={faSkull}
                                className="lose"
                                title={t("Lose")}
                            />
                        }
                    </div>
                    {selectedCombat.isReady
                        ? <FontAwesomeIcon
                            icon={faCheck}
                            className="list-group-item__player-statistic-item"
                            title={t("Ready")}
                        />
                        : <FontAwesomeIcon
                            icon={faClock}
                            className="list-group-item__player-statistic-item"
                            title={t("NotReady")}
                        />
                    }
                    {/*<FontAwesomeIcon*/}
                    {/*    icon={faGlobe}*/}
                    {/*    title={t("PlayerMovements")}*/}
                    {/*    onClick={() => navigate(`/player-movements?combatId=${selectedCombat.id}`)}*/}
                    {/*/>*/}
                </div>
                <div className="combat-time">
                    <div className="combat-time__range">
                        <div className="list-group-item">
                            <div>
                                <div>{format(new Date(selectedCombat?.startDate), 'HH:mm')}</div>
                            </div>
                        </div>
                        <div>-</div>
                        <div className="list-group-item">
                            <div>
                                <div>{format(new Date(selectedCombat?.finishDate), 'HH:mm')}</div>
                            </div>
                        </div>
                    </div>
                    <div className="combat-time__lasts">
                        <div>{getCombatDuration(selectedCombat.duration)}</div>
                        <FontAwesomeIcon
                            icon={faHourglassStart}
                            className="list-group-item__player-statistic-item"
                            title={t("Duration")}
                        />
                    </div>
                </div>
            </div>
            <ul className="information">
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faKhanda}
                        className="list-group-item__player-statistic-item"
                        title={t("Damage")}
                    />
                    <div>{uniqueCombats[selectedCombatIndex].damageDone}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faPlusCircle}
                        className="list-group-item__player-statistic-item"
                        title={t("Healing")}
                    />
                    <div>{uniqueCombats[selectedCombatIndex].healDone}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faShieldHalved}
                        className="list-group-item__player-statistic-item"
                        title={t("DamageTaken")}
                    />
                    <div>{uniqueCombats[selectedCombatIndex].damageTaken}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faBolt}
                        className="list-group-item__player-statistic-item"
                        title={t("EnergyRecovery")}
                    />
                    <div>{uniqueCombats[selectedCombatIndex].energyRecovery}</div>
                </li>
                <li className="list-group-item">
                    <FontAwesomeIcon
                        icon={faCircleNodes}
                        className="list-group-item__player-statistic-item"
                        title={t("Buffs")}
                    />
                    <div className="auras-details" onClick={() => navigate(`/general-analysis/auras?combat=${selectedCombat.id}&combatLog=${combatLogId}`)}>More...</div>
                </li>
            </ul>
            <div className="card-body details">
                {uniqueCombats[selectedCombatIndex].isReady
                    ? <div className="btn-shadow" onClick={() => navigate(`/details-specifical-combat?id=${uniqueCombats[selectedCombatIndex].id}&combatLogId=${combatLogId}&name=${uniqueCombats[selectedCombatIndex].name}`)}>
                        <FontAwesomeIcon
                            icon={faDatabase}
                        />
                        <div>{t("MoreDetails")}</div>
                    </div>
                    : <div>{t("NeedWait")}</div>
                }
            </div>
        </div>
    );
}

export default GeneralAnalysisItem;