import { CombatLogStatus } from '@/shared/helpers/EnumHelper';
import { faArrowDown, faArrowUp, faCircleXmark, faMagnifyingGlassChart, faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { format } from 'date-fns';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import type { CombatLogModel } from '../types/CombatLogModel';
import CombatLogItemActions from './CombatLogItemActions';
import CombatLogItemDiscussion from './CombatLogItemDiscussion';

interface CombatLogItemProps {
    t: (key: string) => string;
    appUserId: string;
    combatLog: CombatLogModel;
    gameVersion: number;
    isAuth: boolean;
}

const GameCombatLogItem: React.FC<CombatLogItemProps> = ({ t, appUserId, combatLog, gameVersion, isAuth }) => {
    const navigate = useNavigate();

    const [showChats, setShowChats] = useState(false);
    const [showGroupChats, setShowGroupChats] = useState(true);
    const [showPersonalChats, setShowPersonalChats] = useState(true);

    const getStatus = () => {
        const lastStatus = combatLog.statuses.at(-1);

        if (lastStatus === undefined) {
            return (<></>);
        }

        switch (lastStatus.status) {
            case CombatLogStatus["Created"]:
                return (<div className="combat-log-status">Created</div>);
            case CombatLogStatus["Creating"]:
                return (<div className="combat-log-status">Creating...</div>);
            case CombatLogStatus["Deleting"]:
                return (<div className="combat-log-status">Deleting...</div>);
            case CombatLogStatus["Deleted"]:
                return (<div className="combat-log-status">Deleted</div>);
            default:
                return (<></>);
        }
    }

    return (
        <div className="card">
            <ul className="list-group list-group-flush">
                <li className="list-group-item title">
                    <div className="title__main">
                        <div>{combatLog.name}</div>
                        {getStatus()}
                    </div>
                    <div className="actions">
                        <div className="actions__communication">
                            {!isAuth &&
                                <FontAwesomeIcon
                                    icon={faTriangleExclamation}
                                    className="authorization"
                                    title={t("ShouldAuthorize")}
                                />
                            }
                            <CombatLogItemDiscussion
                                t={t}
                            />
                        </div>
                        {(appUserId === combatLog.appUserId) &&
                            <CombatLogItemActions
                                t={t}
                                combatLog={combatLog}
                            />
                        }
                    </div>
                </li>
                <li className="list-group-item">{format(new Date(combatLog.date), 'MM/dd/yyyy HH:mm')}</li>
            </ul>
            {(combatLog.statuses.at(-1)?.status === CombatLogStatus["Creating"]
                || combatLog.statuses.at(-1)?.status === CombatLogStatus["Created"]) &&
                <div className="card-body">
                    <div className="btn-shadow" onClick={() => navigate(`/general-analysis?id=${combatLog.id}&gameVersion=${gameVersion}`)}>
                        <FontAwesomeIcon
                            icon={faMagnifyingGlassChart}
                        />
                        <div>{t("Analyzing")}</div>
                    </div>
                </div>
            }
            {showChats &&
                <div className="chat-list">
                    <div className="chat-list__close">
                        <FontAwesomeIcon
                            icon={faCircleXmark}
                            onClick={() => setShowChats(false)}
                            title={t("Close")}
                        />
                    </div>
                    <div>{t("Chats")}</div>
                    <div className="chat-list__chats">
                        <div className="title">
                            <div className="name">{t("GroupChats")}</div>
                            <FontAwesomeIcon
                                icon={showGroupChats ? faArrowUp : faArrowDown}
                                onClick={() => setShowGroupChats(!showGroupChats)}
                                title={showGroupChats ? t("HideChats") : t("ShowChats")}
                            />
                        </div>
                        <div className="title">
                            <div className="name">{t("PersonalChats")}</div>
                            <FontAwesomeIcon
                                icon={showPersonalChats ? faArrowUp : faArrowDown}
                                onClick={() => setShowPersonalChats(!showPersonalChats)}
                                title={showPersonalChats ? t("HideChats") : t("ShowChats")}
                            />
                        </div>
                    </div>
                    <input type="button" value={t("Close")} className="btn btn-light" onClick={() => setShowChats(false)} />
                </div>
            }
        </div>
    );
}

export default GameCombatLogItem;