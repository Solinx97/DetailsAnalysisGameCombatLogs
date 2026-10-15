import Loading from '@/shared/components/Loading';
import { faCalendarDay, faSitemap } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useParams, useSearchParams } from 'react-router-dom';
import { useLazyGetCombatPlayerByIdQuery } from '../../api/GameLogs.api';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import CombatGeneralDetails from '../CombatGeneralDetails';
import CombatMoreDetails from '../CombatMoreDetails';
import CombatDetailsHeader from './CombatDetailsHeader';

import './CombatDetails.scss';

const CombatDetails: React.FC = () => {
    const { t } = useTranslation('combatDetails/combatGeneralDetails');
    
	const { combatId } = useParams();

    const [searchParams] = useSearchParams();
    const detailsType = Number(searchParams.get("detailsType"));
    const playerId = Number(searchParams.get("playerId"));

    const [combatPlayer, setCombatPlayer] = useState<CombatPlayerModel | null>(null);
    const [tabIndex, setTabIndex] = useState<number>(0);

    const [getCombatPlayerById] = useLazyGetCombatPlayerByIdQuery();

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
        switch (detailsType) {
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

    if (!combatPlayer) {
        return (<Loading />);
    }

    return (
        <div className="general-details">
            <div className="general-details__container">
                <div className="general-details__navigate">
                    <CombatDetailsHeader
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
                        combatId={parseInt(combatId || "0")}
                        detailsType={detailsType}
                    />
                    : <CombatMoreDetails
                        unitId={combatPlayer.unitId}
                        detailsType={detailsType}
                    />
                }
            </div>
        </div>
    );
}

export default CombatDetails;