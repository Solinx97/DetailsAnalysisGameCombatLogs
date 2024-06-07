import { useEffect, useState } from "react";

const minDamage = 15000;
const minHeal = 15000;
const minDamageTaken = 15000;

const Dashboard = ({ players }) => {
    const [damageSum, setDamageSum] = useState(0);
    const [healSum, setHealSum] = useState(0);
    const [damageTakenSum, setDamageTakenSum] = useState(0);
    const [sortedByDamagePlayers, setSortedByDamagePlayers] = useState([]);
    const [sortedByHealPlayers, setSortedByHealPlayers] = useState([]);
    const [sortedByDamageTakenPlayers, setSortedByDamageTakenPlayers] = useState([]);

    useEffect(() => {
        if (players.length === 0) {
            return;
        }

        getDamageSum();
        getHealSum();
        getDamageTakenSum();

        let unblockedArray = Object.assign([], players);

        const sorteredByDamage = quickSort(unblockedArray, 0);
        setSortedByDamagePlayers(sorteredByDamage);

        const sortedByHeal = quickSort(unblockedArray, 1);
        setSortedByHealPlayers(sortedByHeal);

        const sortedByDamageTaken = quickSort(unblockedArray, 2);
        setSortedByDamageTakenPlayers(sortedByDamageTaken);
    }, [players]);

    const quickSort = (array, type) => {
        if (array.length <= 1) {
            return array;
        }

        const pivot = array[array.length - 1];
        const left = [];
        const right = [];

        if (type === 0) {
            quickSortByDamage(array, left, right, pivot)
        }
        else if (type === 1) {
            quickSortByHeal(array, left, right, pivot)
        }
        else {
            quickSortByDamageTaken(array, left, right, pivot);
        }

        return [...quickSort(left, type), pivot, ...quickSort(right, type)];
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

    const quickSortByHeal = (array, left, right, pivot) => {
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

    const getDamageSum = () => {
        let sum = 0;

        for (let i = 0; i < players.length; i++) {
            sum += players[i].damageDone;
        }

        setDamageSum(sum);
    }

    const getHealSum = () => {
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

    const damageCalculation = (player) => {
        if (player.damageDone === 0) {
            return 0;
        }

        const playerContribution = player.damageDone / damageSum;
        const precentOfContribution = playerContribution * 100;
        const sumRound = precentOfContribution.toFixed(2);

        return sumRound;
    }

    const healCalculation = (player) => {
        if (player.healDone === 0) {
            return 0;
        }

        const playerContribution = player.healDone / healSum;
        const precentOfContribution = playerContribution * 100;
        const sumRound = precentOfContribution.toFixed(2);

        return sumRound;
    }

    const damageTakenCalculation = (player) => {
        if (player.damageTaken === 0) {
            return 0;
        }

        const playerContribution = player.damageTaken / damageTakenSum;
        const precentOfContribution = playerContribution * 100;
        const sumRound = precentOfContribution.toFixed(2);

        return sumRound;
    }

    if (damageSum === 0) {
        return <div>Loading...</div>;
    }

    return (
        <div className="dashboard">
            <ul className="damage-progress">
                {sortedByDamagePlayers?.filter(player => player.damageDone >= minDamage).map((player) => (
                    <li key={player.id}>
                        <div className="title">
                            <div className="username">{player.userName}</div>
                            <div className="value">{player.damageDone}</div>
                        </div>
                        <div className="player-statistics">
                            <div className="progress">
                                <div className="progress-bar" role="progressbar" style={{ width: damageCalculation(player) + '%' }}
                                    aria-valuenow={damageCalculation(player)} aria-valuemin="0" aria-valuemax="0"></div>
                            </div>
                            <div className="player-contribution">{damageCalculation(player)}</div>
                        </div>
                    </li>
                ))}
            </ul>
            <ul className="heal-progress">
                {sortedByHealPlayers?.filter(player => player.healDone > minHeal).map((player) => (
                    <li key={player.id}>
                        <div className="title">
                            <div className="username">{player.userName}</div>
                            <div className="value">{player.healDone}</div>
                        </div>
                        <div className="player-statistics">
                            <div className="progress">
                                <div className="progress-bar" role="progressbar" style={{ width: healCalculation(player) + '%' }}
                                    aria-valuenow={healCalculation(player)} aria-valuemin="0" aria-valuemax="0"></div>
                            </div>
                            <div className="player-contribution">{healCalculation(player)}</div>
                        </div>
                    </li>
                ))}
            </ul>
            <ul className="heal-progress">
                {sortedByDamageTakenPlayers?.filter(player => player.damageTaken > minDamageTaken).map((player) => (
                    <li key={player.id}>
                        <div className="title">
                            <div className="username">{player.userName}</div>
                            <div className="value">{player.damageTaken}</div>
                        </div>
                        <div className="player-statistics">
                            <div className="progress">
                                <div className="progress-bar" role="progressbar" style={{ width: damageTakenCalculation(player) + '%' }}
                                    aria-valuenow={damageTakenCalculation(player)} aria-valuemin="0" aria-valuemax="0"></div>
                            </div>
                            <div className="player-contribution">{damageTakenCalculation(player)}</div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default Dashboard;