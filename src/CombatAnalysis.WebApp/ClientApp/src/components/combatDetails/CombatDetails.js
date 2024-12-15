import { faCalendarDay, faDeleteLeft, faSitemap } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayerByIdQuery } from '../../store/api/core/CombatParser.api';
import CombatGeneralDetails from './CombatGeneralDetails';
import CombatMoreDetails from './CombatMoreDetails';

import "../../styles/combatGeneralDetails.scss";

const CombatDetails = () => {
    const { t } = useTranslation("combatDetails/combatGeneralDetails");

    const navigate = useNavigate();

    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [combatPlayer, setCombatPlayer] = useState(null);
    const [detailsType, setDetailsType] = useState("");
    const [combatName, setCombatName] = useState("");
    const [tab, setTab] = useState(0);
    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [tabIndex, setTabIndex] = useState(0);

    const [getCombatPlayerById] = useLazyGetCombatPlayerByIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const combatId = +queryParams.get("combatId");

        setCombatPlayerId(+queryParams.get("id"));
        setDetailsType(queryParams.get("detailsType"));
        setCombatId(combatId);
        setCombatLogId(+queryParams.get("combatLogId"));
        setCombatName(queryParams.get("name"));
        setTab(+queryParams.get("tab"));
    }, [])

    useEffect(() => {
        if (combatPlayerId <= 0) {
            return;
        }

        const getGeneralDetails = async () => {
            await getCombatPlayerByIdAsync(combatPlayerId);
        }

        getGeneralDetails();
    }, [combatPlayerId])

    const getCombatPlayerByIdAsync = async (combatPlayerId) => {
        const combatPlayer = await getCombatPlayerById(combatPlayerId);
        if (combatPlayer.data !== undefined) {
            setCombatPlayer(combatPlayer.data);
        }
    }

    const getDetailsTypeName = () => {
        switch (detailsType) {
            case "DamageDone":
                return t("Damage");
            case "HealDone":
                return t("Healing");
            case "DamageTaken":
                return t("DamageTaken");
            case "ResourceRecovery":
                return t("ResourcesRecovery");
            default:
                return "";
        }
    }

    if (combatPlayerId <= 0) {
        return <div>Loading...</div>;
    }

    return (
        <div className="general-details__container">
            <div className="general-details__navigate">
                <div className="player">
                    <div className="btn-shadow select-another-player"
                        onClick={() => navigate(`/details-specifical-combat?id=${combatId}&combatLogId=${combatLogId}&name=${combatName}&tab=${tab}`)}>
                        <FontAwesomeIcon
                            icon={faDeleteLeft}
                        />
                        <div>{t("SelectPlayer")}</div>
                    </div>
                    <div className="btn-shadow username">
                        <div>{combatPlayer?.username}</div>
                    </div>
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
                    combatPlayerId={combatPlayerId}
                    detailsType={detailsType}
                />
                : <CombatMoreDetails
                    combatPlayerId={combatPlayerId}
                    detailsType={detailsType}
                />
            }
        </div>
    );
}

export default CombatDetails;