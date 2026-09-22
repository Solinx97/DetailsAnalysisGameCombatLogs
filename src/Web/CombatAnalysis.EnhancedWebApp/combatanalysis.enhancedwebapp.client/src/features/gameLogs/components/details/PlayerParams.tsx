import { faClose } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, type Dispatch, type SetStateAction } from 'react';
import { useGetPlayerStatsByCombatPlayerIdQuery } from '../../api/GameLogs.api';
import PlayerInfoItem from './PlayerInfoItem';

interface PlayerParamsProps {
    t(key: string): string;
    combatPlayerId: number;
    gameVersion: number;
    setPlayerStatsCombatPlayerId: Dispatch<SetStateAction<number>>;
}

const PlayerParams: React.FC<PlayerParamsProps> = ({ t, combatPlayerId, gameVersion, setPlayerStatsCombatPlayerId }) => {
    const { data: playerStats, isLoading } = useGetPlayerStatsByCombatPlayerIdQuery({ combatPlayerId, gameVersion });

    if (!playerStats || isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="player-params">
            <PlayerInfoItem
                stats={playerStats}
            />
            <div className="btn-shadow"
                onClick={() => setPlayerStatsCombatPlayerId(0)}>
                <FontAwesomeIcon
                    icon={faClose}
                />
                <div>{t("Close")}</div>
            </div>
        </div>
    );
}

export default memo(PlayerParams);