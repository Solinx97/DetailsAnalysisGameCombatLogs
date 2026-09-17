import Loading from '@/shared/components/Loading';
import { faCalendarDay, faSitemap } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetCombatPlayerByIdQuery } from '../../api/GameLogs.api';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import CombatGeneralDetails from '../CombatGeneralDetails';
import CombatMoreDetails from '../CombatMoreDetails';
import CombatDetailsHeader from './CombatDetailsHeader';

import './CombatDetails.scss';

const CombatDetails: React.FC = () => {
    const { t } = useTranslation('combatDetails/combatGeneralDetails');

    const [playerId, setPlayerId] = useState<number>(0);
    const [combatPlayer, setCombatPlayer] = useState<CombatPlayerModel | null>(null);
    const [tabIndex, setTabIndex] = useState<number>(0);
    const [details, setDetails] = useState<CombatDetailsModel>({
        id: 0,
        detailsType: 0,
        combatLogId: 0,
        name: '',
        number: 0,
        isWin: false,
        duration: 0,
        gameVersion: -1
    });

    const [getCombatPlayerById] = useLazyGetCombatPlayerByIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);

        const id: number = parseInt(queryParams.get("id") || '0');
        const detailsType: number = +(queryParams.get("detailsType") ?? 0);
        const combatLogId: number = parseInt(queryParams.get("combatLogId") || '0');
        const name: string = queryParams.get("name") || '';
        const number: number = parseInt(queryParams.get("number") || '0');
        const isWin: boolean = queryParams.get("isWin") === 'true';
        const duration: number = parseInt(queryParams.get("duration") || "1");
        const gameVersion: number = parseInt(queryParams.get("gameVersion") || "-1");

        const playerId: number = parseInt(queryParams.get("playerId") || '0');
        setPlayerId(playerId);

        setDetails({
            id,
            detailsType,
            combatLogId,
            name,
            number,
            isWin,
            duration,
            gameVersion
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
        return (<Loading />);
    }

    return (
        <div className="general-details__container">
            <div className="general-details__navigate">
                <CombatDetailsHeader
                    details={details}
                    combatPlayer={combatPlayer}
                    t={t}
                />
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
                    combatId={details.id}
                    detailsType={details.detailsType}
                />
                : <CombatMoreDetails
                    unitId={combatPlayer.unitId}
                    detailsType={details.detailsType}
                />
            }
        </div>
    );
}

export default CombatDetails;