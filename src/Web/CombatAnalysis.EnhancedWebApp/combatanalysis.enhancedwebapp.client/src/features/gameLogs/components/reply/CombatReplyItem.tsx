import { type Dispatch, type SetStateAction } from 'react';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';

interface CombatReplyItemProps {
    combatPlayer: CombatPlayerModel;
    selectedPlayerId: number;
    setSelectedPlayerId: Dispatch<SetStateAction<number>>;
    color: string;
}

const CombatReplyItem: React.FC<CombatReplyItemProps> = ({ combatPlayer, selectedPlayerId, setSelectedPlayerId, color }) => {
    const handleSelectPlayer = () => {
        setSelectedPlayerId(0);
        setSelectedPlayerId(combatPlayer.id);
    }

    return (
        <div className={`${selectedPlayerId === combatPlayer.id ? "selected" : ""}`} style={{ color: color }} onClick={handleSelectPlayer}>{combatPlayer.player.username}</div>
    );
}

export default CombatReplyItem;