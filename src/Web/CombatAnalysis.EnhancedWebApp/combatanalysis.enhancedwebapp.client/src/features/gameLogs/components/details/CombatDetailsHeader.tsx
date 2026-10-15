import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';

interface CombatDetailsHeaderProps {
    combatPlayer: CombatPlayerModel;
    t(key: string): string;
}

const CombatDetailsHeader: React.FC<CombatDetailsHeaderProps> = ({ combatPlayer, t }) => {
    const navigate = useNavigate();
    
	const { combatId } = useParams();

    const [searchParams] = useSearchParams();
    const combatLogId = searchParams.get("combatLogId");
    const name = searchParams.get("name");
    const number = searchParams.get("number");
    const isWin = searchParams.get("isWin") === "true";
    const duration = searchParams.get("duration");
    const gameVersion = searchParams.get("gameVersion");

    return (
        <>
            <div className="player">
                <div className="btn-shadow select-another-player"
                    onClick={() => navigate(`/selected-combat/${combatId}?combatLogId=${combatLogId}&name=${name}&number=${number}&isWin=${isWin}&duration=${duration}&gameVersion=${gameVersion}`)}>
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
                <div>{name}</div>
                <div className={`combat-number ${isWin ? 'win' : 'lose'}`}>{number}</div>
            </div>
        </>
    );
}

export default CombatDetailsHeader;