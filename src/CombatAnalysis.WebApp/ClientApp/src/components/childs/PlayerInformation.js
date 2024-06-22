import React, { memo } from "react";
import CommonPlayerInform from './CommonPlayerInform';

const PlayerInformation = ({ combatPlayers, combatId, combatLogId, combatName }) => {
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
                            combatName={combatName}
                        />
                    </li>
                ))}
        </ul>
    );
}

export default memo(PlayerInformation);