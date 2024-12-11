import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import DashboardMinDetails from "./DashboardMinDetails";
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const minCount = 4;
const timeout = 1000;

const DashboardItem = ({ name, array, detailsType, calculation, goToCombatGeneralDetails, resourcesSum, closedItems, setClosedItems }) => {
    const { t } = useTranslation("combatDetails/dashboard");

    const [itemCount, setItemCount] = useState(minCount);
    const [selectedPlayerIndex, setSelectedPlayerIndex] = useState(-1);
    const [showDetailsTimeout, setShowDetailsTimeout] = useState(null);
    const [itemClosed, setItemClosed] = useState(false);

    const dashboardDetailsType = {
        0: "damageDone",
        1: "healDone",
        2: "damageTaken",
        3: "resourcesRecovery"
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
        const detailsMapping = ["damageDone", "healDone", "damageTaken", "resourcesRecovery"];

        return player[detailsMapping[detailsType]] || 0;
    }

    const closeItem = () => {
        const allClosedItesm = Object.assign([], closedItems);
        const itemClosedObject = {
            id: allClosedItesm.length,
            name: name,
            setItemClosed: setItemClosed
        };

        allClosedItesm.push(itemClosedObject);

        setClosedItems(allClosedItesm);
        setItemClosed(true);
    }

    if (itemClosed) {
        return (<></>);
    }

    return (
        <div className="dashboard__statistics">
            <div className="title">
                <div>{name}</div>
                <FontAwesomeIcon
                    icon={faXmark}
                    onClick={closeItem}
                />
            </div>
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
                            <div className="username">{player.username}</div>
                            <div className="value">{getDetailsValue(player)}</div>
                        </div>
                        <div className="player-statistics">
                            <div className="progress" onMouseOver={(e) => fillMinDetailsHandle(e, index)} onMouseOut={clearTimeoutHandle}
                                onClick={() => goToCombatGeneralDetails(player.id)}>
                                <div className="progress-bar" role="progressbar" style={{ width: calculation(player, dashboardDetailsType[detailsType], resourcesSum) + '%' }}
                                    aria-valuenow={calculation(player, detailsType, resourcesSum)} aria-valuemin="0" aria-valuemax="0"></div>
                            </div>
                            <div className="player-contribution">{calculation(player, dashboardDetailsType[detailsType], resourcesSum)}%</div>
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