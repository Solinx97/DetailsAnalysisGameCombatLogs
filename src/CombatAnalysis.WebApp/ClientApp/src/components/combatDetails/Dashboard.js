import { memo, useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import Loading from "../Loading";
import DashboardDeathItem from "./DashboardDeathItem";
import DashboardItem from "./DashboardItem";

import "../../styles/dashboard.scss";

const Dashboard = ({ players, combatId, combatLogId, combatName, playersDeath }) => {
    const { t } = useTranslation("combatDetails/dashboard");

    const [damageSum, setDamageSum] = useState(0);
    const [healingSum, setHealSum] = useState(0);
    const [damageTakenSum, setDamageTakenSum] = useState(0);
    const [resourcesRecoverySum, setResourcesRecoverySum] = useState(0);
    const [sortedByDamagePlayers, setSortedByDamagePlayers] = useState([]);
    const [sortedByHealPlayers, setSortedByHealPlayers] = useState([]);
    const [sortedByDamageTakenPlayers, setSortedByDamageTakenPlayers] = useState([]);
    const [sortedByResourcesRecoveryPlayers, setSortedByResourcesRecoveryPlayers] = useState([]);
    const [closedItems, setClosedItems] = useState([]);

    const navigate = useNavigate();

    const dashboardDetailsType = {
        0: "damageDone",
        1: "healDone",
        2: "damageTaken",
        3: "energyRecovery"
    };

    useEffect(() => {
        if (players.length === 0) {
            return;
        }

        const unblockedArray = Object.assign([], players);

        setDamageSum(calculateSum(dashboardDetailsType[0]));
        setHealSum(calculateSum(dashboardDetailsType[1]));
        setDamageTakenSum(calculateSum(dashboardDetailsType[2]));
        setResourcesRecoverySum(calculateSum(dashboardDetailsType[3]));

        setSortedByDamagePlayers(sortByKey(unblockedArray, dashboardDetailsType[0]));
        setSortedByHealPlayers(sortByKey(unblockedArray, dashboardDetailsType[1]));
        setSortedByDamageTakenPlayers(sortByKey(unblockedArray, dashboardDetailsType[2]));
        setSortedByResourcesRecoveryPlayers(sortByKey(unblockedArray, dashboardDetailsType[3]));
    }, [players]);

    const calculateSum = (key) => {
        const reducedPlayers = players.reduce((acc, player) => acc + player[key], 0);

        return reducedPlayers;
    }

    const sortByKey = (array, key) => {
        const sortedPlayers = [...array].sort((a, b) => b[key] - a[key]);

        return sortedPlayers;
    }

    const calculation = (player, typeOfResource, resourcesSum) => {
        const typeOfResourceValue = player[typeOfResource];

        if (!typeOfResourceValue) {
            return 0;
        }

        const playerContribution = typeOfResourceValue / resourcesSum;
        const playerContributionFixed = (playerContribution * 100).toFixed(2);

        return playerContributionFixed;
    }

    const goToCombatGeneralDetails = (playerId) => {
        navigate(`/combat-general-details?id=${playerId}&detailsType=DamageDone&combatId=${combatId}&combatLogId=${combatLogId}&name=${combatName}&tab=${0}`);
    }

    const openClosedItems = (closedItem) => {
        const filteredItems = closedItems.filter(item => item.id !== closedItem.id);

        setClosedItems(filteredItems);
        closedItem.setItemClosed(false);
    }

    if (damageSum === 0) {
        return (<Loading />);
    }

    return (
        <div className="dashboard">
            <div className="closed-items">
                {closedItems.length > 0 &&
                    <>
                        <div>{t("ClosedItems")}</div>
                        <ul>
                            {closedItems.map((item, index) =>
                                <li key={index} onClick={() => openClosedItems(item)}>{item.name}</li>
                            )}
                        </ul>
                    </>
                }
            </div>
            <div className="items">
                <DashboardItem
                    name={t("Damage")}
                    array={sortedByDamagePlayers}
                    detailsType={0}
                    calculation={calculation}
                    goToCombatGeneralDetails={goToCombatGeneralDetails}
                    resourcesSum={damageSum}
                    closedItems={closedItems}
                    setClosedItems={setClosedItems}
                />
                <DashboardItem
                    name={t("Healing")}
                    array={sortedByHealPlayers}
                    detailsType={1}
                    calculation={calculation}
                    goToCombatGeneralDetails={goToCombatGeneralDetails}
                    resourcesSum={healingSum}
                    closedItems={closedItems}
                    setClosedItems={setClosedItems}
                />
                <DashboardItem
                    name={t("DamageTaken")}
                    array={sortedByDamageTakenPlayers}
                    detailsType={2}
                    calculation={calculation}
                    goToCombatGeneralDetails={goToCombatGeneralDetails}
                    resourcesSum={damageTakenSum}
                    closedItems={closedItems}
                    setClosedItems={setClosedItems}
                />
                <DashboardItem
                    name={t("ResourcesRecovery")}
                    array={sortedByResourcesRecoveryPlayers}
                    detailsType={3}
                    calculation={calculation}
                    goToCombatGeneralDetails={goToCombatGeneralDetails}
                    resourcesSum={resourcesRecoverySum}
                    closedItems={closedItems}
                    setClosedItems={setClosedItems}
                />
                <DashboardDeathItem
                    playersDeath={playersDeath}
                    players={players}
                />
            </div>
        </div>
    );
}

export default memo(Dashboard);