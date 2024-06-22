import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import DashboardMinDetails from "./DashboardMinDetails";

const minCount = 4;
const timeout = 1000;

const DashboardItem = ({ name, array, detailsType, calculation, goToCombatGeneralDetails, resourcesSum }) => {
    const { t } = useTranslation("combatDetails/dashboard");

    const [itemCount, setItemCount] = useState(minCount);
    const [selectedPlayerIndex, setSelectedPlayerIndex] = useState(-1);
    const [showDetailsTimeout, setShowDetailsTimeout] = useState(null);

    const dashboardDetailsType = {
        0: "damageDone",
        1: "healDone",
        2: "damageTaken",
        3: "energyRecovery"
    };

    useEffect(() => {
        return () => {
            if (showDetailsTimeout) {
                clearTimeout(showDetailsTimeout);
            }
        }
    }, [showDetailsTimeout]);

    const fillMinDetailsHandle = (e, index) => {
        const timeoutHandler = setTimeout(() => {
            setSelectedPlayerIndex(index);
        }, timeout);

        setShowDetailsTimeout(timeoutHandler);
    }

    const closeMinDetailsHandle = (e) => {
        setSelectedPlayerIndex(-1);
    }

    const clearTimeoutHandle = (e) => {
        clearTimeout(showDetailsTimeout);
    }

    const getDetailsValue = (player) => {
        const detailsMapping = ['damageDone', 'healDone', 'damageTaken', 'energyRecovery'];

        return player[detailsMapping[detailsType]] || 0;
    }

    return (
        <div className="dashboard__statistics">
            <div>{name}</div>
            <ul className="players-progress">
                {array?.slice(0, itemCount).map((player, index) => (
                    <li key={player.id}>
                        {(selectedPlayerIndex === index) &&
                            <DashboardMinDetails
                                combatPlayerId={player.id}
                                closeHandle={closeMinDetailsHandle}
                                detailsType={detailsType}
                            />
                        }
                        <div className="title">
                            <div className="username">{player.userName}</div>
                            <div className="value">{getDetailsValue(player)}</div>
                        </div>
                        <div className="player-statistics">
                            <div className="progress" onMouseOver={(e) => fillMinDetailsHandle(e, index)} onMouseOut={clearTimeoutHandle}
                                onClick={() => goToCombatGeneralDetails(player.id)}>
                                <div className="progress-bar" role="progressbar" style={{ width: calculation(player, dashboardDetailsType[detailsType], resourcesSum) + '%' }}
                                    aria-valuenow={calculation(player, detailsType, resourcesSum)} aria-valuemin="0" aria-valuemax="0"></div>
                            </div>
                            <div className="player-contribution">{calculation(player, dashboardDetailsType[detailsType], resourcesSum)} %</div>
                        </div>
                    </li>
                ))}
            </ul>
            {itemCount === minCount
                ? <div className="extend" onClick={() => setItemCount(array.length)}>{t("More")}</div>
                : <div className="extend" onClick={() => setItemCount(minCount)}>{t("Less")}</div>
            }
        </div>
    );
}

export default DashboardItem;