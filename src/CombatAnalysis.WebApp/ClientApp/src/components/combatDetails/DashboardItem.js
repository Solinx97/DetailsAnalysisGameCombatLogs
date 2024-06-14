import { useState } from "react";
import DashboardMinDetails from "./DashboardMinDetails";

const minCount = 4;

const timeout = 1000;

const DashboardItem = ({ name, array, detailsType, calculation, goToCombatGeneralDetails, resourcesSum }) => {
    const [itemCount, setItemCount] = useState(minCount);
    const [selectedPlayerIndex, setSelectedPlayerIndex] = useState(0);
    const [minDetails, setMinDetails] = useState(<></>);
    const [showDetailsTimeout, setShowDetailsTimeout] = useState(null);

    const fillMinDetailsHandle = (e, index, detailsType, combatPlayerId) => {
        const timeoutHandler = setTimeout(() => {
            setSelectedPlayerIndex(index);
            setMinDetails(<DashboardMinDetails combatPlayerId={combatPlayerId} closeHandle={closeMinDetailsHandle} detailsType={detailsType} />);
        }, timeout);

        setShowDetailsTimeout(timeoutHandler);
    }

    const closeMinDetailsHandle = (e) => {
        setSelectedPlayerIndex(0);
        setMinDetails(<></>);
    }

    const clearTimeoutHandle = (e) => {
        clearTimeout(showDetailsTimeout);
    }

    const getDetailsValue = (player) => {
        let value = 0;
        switch (detailsType) {
            case 0:
                value = player.damageDone;
                break;
            case 1:
                value = player.healDone;
                break;
            case 2:
                value = player.damageTaken;
                break;
            case 3:
                value = player.energyRecovery;
                break;
            default:
        }

        return value;
    }

    return (
        <div className="dashboard__statistics">
            <div>{name}</div>
            <ul className="players-progress">
                {array?.slice(0, itemCount).map((player, index) => (
                    <li key={player.id}>
                        {(selectedPlayerIndex === index) &&
                            minDetails
                        }
                        <div className="title">
                            <div className="username">{player.userName}</div>
                            <div className="value">{getDetailsValue(player)}</div>
                        </div>
                        <div className="player-statistics">
                            <div className="progress" onMouseOver={(e) => fillMinDetailsHandle(e, index, detailsType, player.id)} onMouseOut={clearTimeoutHandle}
                                onClick={() => goToCombatGeneralDetails(player.id)}>
                                <div className="progress-bar" role="progressbar" style={{ width: calculation(player, detailsType, resourcesSum) + '%' }}
                                    aria-valuenow={calculation(player, detailsType, resourcesSum)} aria-valuemin="0" aria-valuemax="0"></div>
                            </div>
                            <div className="player-contribution">{calculation(player, detailsType, resourcesSum)} %</div>
                        </div>
                    </li>
                ))}
            </ul>
            {itemCount === minCount
                ? <div className="extend" onClick={() => setItemCount(array.length)}>More</div>
                : <div className="extend" onClick={() => setItemCount(minCount)}>Less</div>
            }
        </div>
    );
}

export default DashboardItem;