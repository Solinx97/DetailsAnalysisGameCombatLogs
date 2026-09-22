import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useNavigate } from 'react-router-dom';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';

interface CombatDetailsHeaderProps {
    details: CombatDetailsModel;
    combatPlayer: CombatPlayerModel;
    t(key: string): string;
}

const CombatDetailsHeader: React.FC<CombatDetailsHeaderProps> = ({ details, combatPlayer, t }) => {
    const navigate = useNavigate();

    return (
        <>
            <div className="player">
                <div className="btn-shadow select-another-player"
                    onClick={() => navigate(`/selected-combat?id=${details.id}&combatLogId=${details.combatLogId}&name=${details.name}&number=${details.number}&isWin=${details.isWin}&duration=${details.duration}&gameVersion=${details.gameVersion}`)}>
                    <FontAwesomeIcon
                        icon={faDeleteLeft}
                    />
                    <div>{t("SelectPlayer")}</div>
                </div>
                <div className="btn-shadow username">
                    <div>{combatPlayer?.player?.username}</div>
                </div>
            </div>
            <div className="boss">
                <div>{details.name}</div>
                <div className={`combat-number ${details.isWin ? 'win' : 'lose'}`}>{details.number}</div>
            </div>
        </>
    );
}

export default CombatDetailsHeader;