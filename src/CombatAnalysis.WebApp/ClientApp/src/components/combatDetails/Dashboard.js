import { useEffect, useState } from "react";
import { useNavigate } from 'react-router-dom';
import DashboardItem from "./DashboardItem";
import DashboardDeathItem from "./DashboardDeathItem";

import "../../styles/dashboard.scss";

const Dashboard = ({ players, combatId, combatLogId, combatName, playersDeath }) => {
    const [damageSum, setDamageSum] = useState(0);
    const [healingSum, setHealSum] = useState(0);
    const [damageTakenSum, setDamageTakenSum] = useState(0);
    const [resourcesRecoverySum, setResourcesRecoverySum] = useState(0);

    const [sortedByDamagePlayers, setSortedByDamagePlayers] = useState([]);
    const [sortedByHealPlayers, setSortedByHealPlayers] = useState([]);
    const [sortedByDamageTakenPlayers, setSortedByDamageTakenPlayers] = useState([]);
    const [sortedByResourcesRecoveryPlayers, setSortedByResourcesRecoveryPlayers] = useState([]);

    const navigate = useNavigate();

    useEffect(() => {
        if (players.length === 0) {
            return;
        }

        getDamageSum();
        getHealingSum();
        getDamageTakenSum();
        getResourcesRecoverySum();

        let unblockedArray = Object.assign([], players);

        let sorteredByDamage = quickSort(unblockedArray, 0);
        setSortedByDamagePlayers(sorteredByDamage);

        let sortedByHeal = quickSort(unblockedArray, 1);
        setSortedByHealPlayers(sortedByHeal);

        let sortedByDamageTaken = quickSort(unblockedArray, 2);
        setSortedByDamageTakenPlayers(sortedByDamageTaken);

        let sortedByResourcesRecovery = quickSort(unblockedArray, 3);
        setSortedByResourcesRecoveryPlayers(sortedByResourcesRecovery);
    }, [players]);

    const quickSort = (array, type) => {
        if (array.length <= 1) {
            return array;
        }

        const pivot = array[array.length - 1];
        const left = [];
        const right = [];

        switch (type) {
            case 0:
                quickSortByDamage(array, left, right, pivot);
                return [...quickSort(left, type), pivot, ...quickSort(right, type)];
            case 1:
                quickSortByHeaing(array, left, right, pivot);
                return [...quickSort(left, type), pivot, ...quickSort(right, type)];
            case 2:
                quickSortByDamageTaken(array, left, right, pivot);
                return [...quickSort(left, type), pivot, ...quickSort(right, type)];
            case 3:
                quickSortByResourcesRecovery(array, left, right, pivot);
                return [...quickSort(left, type), pivot, ...quickSort(right, type)];
            default:
                quickSortByDamage(array, left, right, pivot);
                return [...quickSort(left, type), pivot, ...quickSort(right, type)];
        }
    }

    const quickSortByDamage = (array, left, right, pivot) => {
        for (let i = 0; i < array.length - 1; i++) {
            if (array[i].damageDone > pivot.damageDone) {
                left.push(array[i]);
            }
            else {
                right.push(array[i])
            }
        }
    }

    const quickSortByHeaing = (array, left, right, pivot) => {
        for (let i = 0; i < array.length - 1; i++) {
            if (array[i].healDone > pivot.healDone) {
                left.push(array[i]);
            }
            else {
                right.push(array[i])
            }
        }
    }

    const quickSortByDamageTaken = (array, left, right, pivot) => {
        for (let i = 0; i < array.length - 1; i++) {
            if (array[i].damageTaken > pivot.damageTaken) {
                left.push(array[i]);
            }
            else {
                right.push(array[i])
            }
        }
    }

    const quickSortByResourcesRecovery = (array, left, right, pivot) => {
        for (let i = 0; i < array.length - 1; i++) {
            if (array[i].energyRecovery > pivot.energyRecovery) {
                left.push(array[i]);
            }
            else {
                right.push(array[i])
            }
        }
    }

    const getDamageSum = () => {
        let sum = 0;

        for (let i = 0; i < players.length; i++) {
            sum += players[i].damageDone;
        }

        setDamageSum(sum);
    }

    const getHealingSum = () => {
        let sum = 0;

        for (let i = 0; i < players.length; i++) {
            sum += players[i].healDone;
        }

        setHealSum(sum);
    }

    const getDamageTakenSum = () => {
        let sum = 0;

        for (let i = 0; i < players.length; i++) {
            sum += players[i].damageTaken;
        }

        setDamageTakenSum(sum);
    }

    const getResourcesRecoverySum = () => {
        let sum = 0;

        for (let i = 0; i < players.length; i++) {
            sum += players[i].energyRecovery;
        }

        setResourcesRecoverySum(sum);
    }

    const calculation = (player, typeOfResource, resourcesSum) => {
        if (player.damageTaken === 0) {
            return 0;
        }

        let typeOfResourceValue = 0;
        switch (typeOfResource) {
            case 0:
                typeOfResourceValue = player.damageDone;
                break;
            case 1:
                typeOfResourceValue = player.healDone;
                break;
            case 2:
                typeOfResourceValue = player.damageTaken;
                break;
            case 3:
                typeOfResourceValue = player.energyRecovery;
                break;
            default:
        }

        const playerContribution = typeOfResourceValue / resourcesSum;
        const precentOfContribution = playerContribution * 100;
        const sumRound = precentOfContribution.toFixed(2);

        return sumRound;
    }

    const goToCombatGeneralDetails = (playerId) => {
        navigate(`/combat-general-details?id=${playerId}&detailsType=DamageDone&combatId=${combatId}&combatLogId=${combatLogId}&name=${combatName}&tab=${0}`);
    }

    if (damageSum === 0) {
        return <div className="dashboard__loading">Loading...</div>;
    }

    return (
        <div className="dashboard">
            <div className="container">
                <DashboardItem
                    name="Damage"
                    array={sortedByDamagePlayers}
                    detailsType={0}
                    calculation={calculation}
                    goToCombatGeneralDetails={goToCombatGeneralDetails}
                    resourcesSum={damageSum}
                />
                <DashboardItem
                    name="Healing"
                    array={sortedByHealPlayers}
                    detailsType={1}
                    calculation={calculation}
                    goToCombatGeneralDetails={goToCombatGeneralDetails}
                    resourcesSum={healingSum}
                />
                <DashboardItem
                    name="Damage taken"
                    array={sortedByDamageTakenPlayers}
                    detailsType={2}
                    calculation={calculation}
                    goToCombatGeneralDetails={goToCombatGeneralDetails}
                    resourcesSum={damageTakenSum}
                />
                <DashboardItem
                    name="Resources"
                    array={sortedByResourcesRecoveryPlayers}
                    detailsType={3}
                    calculation={calculation}
                    goToCombatGeneralDetails={goToCombatGeneralDetails}
                    resourcesSum={resourcesRecoverySum}
                />
                <DashboardDeathItem
                    playersDeath={playersDeath}
                    players={players}
                />
            </div>
        </div>
    );
}

export default Dashboard;