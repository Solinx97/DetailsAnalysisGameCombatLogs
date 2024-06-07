import React from "react";
import { useTranslation } from 'react-i18next';
import CommonPlayerInform from './CommonPlayerInform';

const PlayerInformation = ({ combatPlayers, combatId, combatLogId }) => {
    const { t } = useTranslation("childs/playerInformation");

    return (
        <ul>
            {combatPlayers?.map((player) => (
                    <li key={player.id} className="card">
                        <div className="card-body">
                            <h5 className="card-title">{player.userName}</h5>
                        </div>
                        <CommonPlayerInform
                            player={player}
                            combatId={combatId}
                            combatLogId={combatLogId}
                        />
                    </li>
                ))}
        </ul>
    );
}

export default PlayerInformation;