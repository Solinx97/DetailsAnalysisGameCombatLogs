import useTime from '@/shared/hooks/useTime';
import { faHourglassStart } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type Dispatch, type SetStateAction } from 'react';
import type { CombatModel } from "../types/CombatModel";

interface GeneralAnalysisCombatsProps {
    uniqueCombats: CombatModel[];
    setSelectedCombatIndex: Dispatch<SetStateAction<number>>;
    t: (key: string) => string;
}

const GeneralAnalysisCombats: React.FC<GeneralAnalysisCombatsProps> = ({ uniqueCombats, setSelectedCombatIndex, t }) => {
    const lastCombat = uniqueCombats[uniqueCombats.length - 1];
    const [showAll, setShowAll] = useState<boolean>(false);

    const { formatDate } = useTime();

    const getCombatDuration = (duration: string) => duration.substring(3);

    return (
        <ul className="card__combats">
            <li key={0} className="last-combat">
                <div className="last-combat__last">last combat</div>
                <div className={`last-combat__content card__${lastCombat.isWin ? 'win' : 'lose'}`} onClick={() => setSelectedCombatIndex(uniqueCombats.length - 1)}>
                    <div className="combat-number">{uniqueCombats.length}</div>
                    <div className="combat-time">
                        <div className="combat-time__range">
                            <div>
                                <div>{formatDate(lastCombat.startDate)}</div>
                            </div>
                            <div>-</div>
                            <div>
                                <div>{formatDate(lastCombat.finishDate)}</div>
                            </div>
                        </div>
                        <div className="combat-time__lasts">
                            <div>{getCombatDuration(lastCombat.duration)}</div>
                            <FontAwesomeIcon
                                icon={faHourglassStart}
                                className="list-group-item__duration"
                                title={t("Duration") || ""}
                            />
                        </div>
                    </div>
                </div>
            </li>
            {uniqueCombats.length > 1 &&
                <>
                    {showAll
                        ? <li className="hide-all-combats" onClick={() => setShowAll(false)}>
                            <div>-</div>
                            <div className="show-all-combats__count">{uniqueCombats.length}</div>
                        </li>
                        : <li className="show-all-combats" onClick={() => setShowAll(true)}>
                            <div>+</div>
                            <div className="show-all-combats__count">{uniqueCombats.length}</div>
                        </li>
                    }
                </>
            }
            {(uniqueCombats.length > 1 && showAll) &&
                uniqueCombats.map((combat, index) => (
                    <li key={combat.id} className={`card__${combat.isWin ? 'win' : 'lose'}`} onClick={() => setSelectedCombatIndex(index)}>
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
                ))
            }
        </ul>
    );
}

export default GeneralAnalysisCombats;