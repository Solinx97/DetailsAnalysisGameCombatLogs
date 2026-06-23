import { type Dispatch, type SetStateAction } from 'react';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';

interface CombatReplyItemProps {
    combatPlayer: CombatPlayerModel;
    selectedPlayerId: number;
    setSelectedPlayerId: Dispatch<SetStateAction<number>>;
}

const CombatReplyItem: React.FC<CombatReplyItemProps> = ({ combatPlayer, selectedPlayerId, setSelectedPlayerId }) => {
    return (
        <div className={`${selectedPlayerId === combatPlayer.id ? "selected" : ""}`} onClick={() => setSelectedPlayerId(combatPlayer.id)}>{combatPlayer.player.username}</div>
    );
}

export default CombatReplyItem;