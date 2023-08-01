import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayerIdQuery } from '../../store/api/CombatParserApi';
import CombatDetails from './CombatDetails';
import CombatGeneralDetailsItem from './CombatGeneralDetailsItem';

import "../../styles/combatGeneralDetails.scss";

const CombatGeneralDetails = () => {
    const navigate = useNavigate();
    const { t, i18n } = useTranslation("combatDetails/combatGeneralDetails");

    const [combatPlayerId, setCombatPlayerId] = useState(0);
    const [combatPlayer, setCombatPlayer] = useState(null);
    const [detailsType, setDetailsType] = useState("");
    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [tabIndex, setTabIndex] = useState(0);

    const [getCombatPlayerIdAsyncMut] = useLazyGetCombatPlayerIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatPlayerId(+queryParams.get("id"));
        setDetailsType(queryParams.get("detailsType"));
        setCombatId(+queryParams.get("combatId"));
        setCombatLogId(+queryParams.get("combatLogId"));
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
        const combatPlayer = await getCombatPlayerIdAsyncMut(combatPlayerId);
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
        return <></>;
    }

    return (
        <div className="general-details__container">
            <div className="general-details__container_navigate">
                <div className="btn-group" role="group">
                    <button type="button" className="btn btn-primary" onClick={() => navigate(`/details-specifical-combat?id=${combatId}&combatLogId=${combatLogId}`)}>{t("SelectPlayer")}</button>
                </div>
                <ul className="nav nav-tabs">
                    <li className="nav-item">
                        <a className={tabIndex == 0 ? "nav-link active" : "nav-link"} aria-current="page" onClick={() => setTabIndex(0)}>{t("CommonInform")}</a>
                    </li>
                    <li className="nav-item">
                        <a className={tabIndex == 1 ? "nav-link active" : "nav-link"} onClick={() => setTabIndex(1)}>{t("DetailsInform")}</a>
                    </li>
                </ul>
            </div>
            {tabIndex === 0
                ? <CombatGeneralDetailsItem
                    combatPlayerUsername={combatPlayer?.userName}
                    combatPlayerId={combatPlayerId}
                    detailsType={detailsType}
                    detailsTypeName={getDetailsTypeName()}
                />
                : <CombatDetails
                    combatPlayerId={combatPlayerId}
                    detailsType={detailsType}
                    detailsTypeName={getDetailsTypeName()}
                    username={combatPlayer?.userName}
                />
            }
        </div>
    );
}

export default CombatGeneralDetails;