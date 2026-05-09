import { faCalendarDay, faDeleteLeft, faSitemap } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayerByIdQuery } from '../api/GameLogs.api';
import type { CombatDetailsModel } from '../types/CombatDetailsModel';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import CombatGeneralDetails from './CombatGeneralDetails';
import CombatMoreDetails from './CombatMoreDetails';

import './CombatGeneralDetails.scss';

const CombatDetails: React.FC = () => {
    const { t } = useTranslation("combatDetails/combatGeneralDetails");

    const navigate = useNavigate();

    const [combatPlayer, setCombatPlayer] = useState<CombatPlayerModel | null>(null);
    const [tabIndex, setTabIndex] = useState<number>(0);
    const [playerId, setPlayerId] = useState<number>(0);
    const [details, setDetails] = useState<CombatDetailsModel>({
        id: 0,
        detailsType: 'DamageDone',
        combatLogId: 0,
        name: '',
        number: 0,
        isWin: false
    });

    const [getCombatPlayerById] = useLazyGetCombatPlayerByIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);

        const id: number = parseInt(queryParams.get("id") || '0');
        const detailsType: string = queryParams.get("detailsType") ?? 'DamageDone';
        const combatLogId: number = parseInt(queryParams.get("combatLogId") || '0');
        const name: string = queryParams.get("name") || '';
        const number: number = parseInt(queryParams.get("number") || '0');
        const isWin: boolean = queryParams.get("isWin") === 'true';

        const playerId: number = parseInt(queryParams.get("playerId") || '0');
        setPlayerId(playerId);

        setDetails({
            id,
            detailsType,
            combatLogId,
            name,
            number,
            isWin,
        });
    }, []);

    useEffect(() => {
        if (playerId <= 0) {
            return;
        }

        const getGeneralDetails = async () => {
            await getCombatPlayerByIdAsync(playerId);
        }

        getGeneralDetails();
    }, [playerId]);

    const getCombatPlayerByIdAsync = async (id: number): Promise<void> => {
        try {
            const combatPlayer = await getCombatPlayerById(id).unwrap();
            setCombatPlayer(combatPlayer);
        } catch (e) {
            console.error(e);
        }
    }

    const getDetailsTypeName = (): string => {
        switch (+details.detailsType) {
            case 0:
                return t("Damage");
            case 1:
                return t("Healing");
            case 2:
                return t("DamageTaken");
            case 3:
                return t("ResourcesRecovery");
            default:
                return "";
        }
    }

    if (details.id <= 0 || !combatPlayer) {
        return <div>Loading...</div>;
    }

    return (
        <div className="general-details__container">
            <div className="general-details__navigate">
                <div className="player">
                    <div className="btn-shadow select-another-player"
                        onClick={() => navigate(`/selected-combat?id=${details.id}&combatLogId=${details.combatLogId}&name=${details.name}&number=${details.number}&isWin=${details.isWin}`)}>
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
                <div className="details-type">{getDetailsTypeName()}</div>
                <ul className="types">
                    <li className="nav-item">
                        <div className={`btn-shadow ${tabIndex === 0 ? "active" : ""}`} onClick={() => setTabIndex(0)}>
                            <FontAwesomeIcon
                                icon={faSitemap}
                            />
                            <div>{t("CommonInform")}</div>
                        </div>
                    </li>
                    <li className="nav-item">
                        <div className={`btn-shadow ${tabIndex === 1 ? "active" : ""}`} onClick={() => setTabIndex(1)}>
                            <FontAwesomeIcon
                                icon={faCalendarDay}
                            />
                            <div>{t("DetailsInform")}</div>
                        </div>
                    </li>
                </ul>
            </div>
            {tabIndex === 0
                ? <CombatGeneralDetails
                    combatPlayer={combatPlayer}
                    detailsType={details.detailsType}
                />
                : <CombatMoreDetails
                    combatPlayerId={combatPlayer.id}
                    detailsType={details.detailsType}
                />
            }
        </div>
    );
}

export default CombatDetails;