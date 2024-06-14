import { useEffect, useState } from "react";
import { useLazyGetDamageTakenByPlayerIdQuery } from '../../store/api/CombatParserApi';

const minCount = 4;

const DashboardDeathItem = ({ playersDeath, players }) => {
    const [getDamageTakenByPlayerIdAsync] = useLazyGetDamageTakenByPlayerIdQuery();

    const [itemCount, setItemCount] = useState(minCount);
    const [extentedPlayersDeath, setExtentedPlayersDeath] = useState([]);

    useEffect(() => {
        if (playersDeath.length === 0) {
            return;
        }

        const getDetaAsync = async () => {
            const unblockedArray = [];
            for (let i = 0; i < playersDeath.length; i++) {
                const player = players.filter(x => x.id === playersDeath[i].combatPlayerId)[0];
                unblockedArray.push(Object.assign({}, playersDeath[i]));

                const detailsResult = await getDamageTakenByPlayerIdAsync(player.id);
                const lastHit = detailsResult.data[detailsResult.data.length - 1];

                unblockedArray[i].username = player.userName;
                unblockedArray[i].skill = lastHit.spellOrItem;
                unblockedArray[i].value = lastHit.value;
            }

            setExtentedPlayersDeath(unblockedArray);
        }

        getDetaAsync();
    }, [playersDeath]);

    return (
        <div className="dashboard__statistics">
            <div>Players death</div>
            <ul className="death-info">
                {extentedPlayersDeath?.slice(0, itemCount).map((death, index) => (
                    <li key={index} className="death-info__details">
                        <div>{death?.username}</div>
                        <div>{death?.skill}</div>
                        <div>{death?.value}</div>
                    </li>
                ))}
            </ul>
            {itemCount === minCount
                ? <div className="extend" onClick={() => setItemCount(extentedPlayersDeath.length)}>More</div>
                : <div className="extend" onClick={() => setItemCount(minCount)}>Less</div>
            }
        </div>
    );
}

export default DashboardDeathItem;