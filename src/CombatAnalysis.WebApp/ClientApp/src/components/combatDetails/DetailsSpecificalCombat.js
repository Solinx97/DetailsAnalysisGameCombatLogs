import React, { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayersByCombatIdQuery } from '../../store/api/CombatParserApi';
import PlayerInformation from '../childs/PlayerInformation';
import GeneralDetailsChart from './GeneralDetailsChart';

import "../../styles/detailsSpecificalCombat.scss";

const DetailsSpecificalCombat = () => {
    const navigate = useNavigate();
    const { t, i18n } = useTranslation("detailsSpecificalCombat");

    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [combatPlayers, setCombatPlayers] = useState([]);
    const [showGeneralDetails, setShowGeneralDetails] = useState(false);

    const [getCombatPlayersByCombatIdAsync] = useLazyGetCombatPlayersByCombatIdQuery();

    const PlayerInformationMemo = memo(PlayerInformation);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatId(+queryParams.get("id"));
        setCombatLogId(+queryParams.get("combatLogId"));
    }, [])

    useEffect(() => {
        if (combatId <= 0) {
            return;
        }

        const getCombatPlayers = async () => {
            await getCombatPlayersAsync();
        };

        getCombatPlayers();
    }, [combatId])

    const getCombatPlayersAsync = async () => {
        const combatPlayers = await getCombatPlayersByCombatIdAsync(combatId);
        if (combatPlayers.data !== undefined) {
            setCombatPlayers(combatPlayers.data);
        }
    }

    return (
        <div className="details-specifical-combat__container">
            <div className="details-specifical-combat__container_navigate">
                <h3>{t("Players")}</h3>
                <div className="btn-group" role="group">
                    <button type="button" className="btn btn-primary" onClick={() => navigate(`/general-analysis?id=${combatLogId}`)}>{t("SelectCombat")}</button>
                </div>
            </div>
            {combatPlayers.length > 0 &&
                <div className="form-check form-switch">
                    <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralDetails((item) => !item)} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{t("ShowCommonStatistics")}</label>
                </div>
            }
            {showGeneralDetails &&
                <GeneralDetailsChart
                    combatPlayers={combatPlayers}
                />
            }
            <PlayerInformationMemo
                combatPlayers={combatPlayers}
                combatId={combatId}
                combatLogId={combatLogId}
            />
        </div>
    );
}

export default DetailsSpecificalCombat;