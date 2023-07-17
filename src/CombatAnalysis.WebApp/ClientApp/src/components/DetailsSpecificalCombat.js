import React, { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import DetailsSpecificalCombatService from '../services/DetailsSpecificalCombatService';
import DetailsPieChart from './childs/DetailsPieChart';
import PlayerInformation from './childs/PlayerInformation';

import "../styles/detailsSpecificalCombat.scss";

const DetailsSpecificalCombat = () => {
    const detailsSpecificalCombatService = new DetailsSpecificalCombatService();

    const navigate = useNavigate();
    const { t, i18n } = useTranslation("detailsSpecificalCombat");

    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [combatPlayers, setCombatPlayers] = useState([]);
    const [showGeneralDetails, setShowGeneralDetails] = useState(false);
    const [damageDonePieChart, setDamageDonePieChart] = useState({});
    const [healDonePieChart, setHealDonePieChart] = useState({});
    const [damageTakenPieChart, setDamageTakenPieChart] = useState({});

    const PlayerInformationMemo = memo(PlayerInformation);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatId(+queryParams.get("id"));
    }, []);

    useEffect(() => {
        if (combatId <= 0) {
            return;
        }

        const getCombatPlayers = async () => {
            await getCombatPlayersAsync();
        };

        getCombatPlayers();
    }, [combatId]);

    useEffect(() => {
        if (combatPlayers.length <= 0) {
            return;
        }

        let data = createPieChardData();

        setDamageDonePieChart({
            title: t("Damage"),
            color: "blue",
            data: data.damageDone
        });
        setHealDonePieChart({
            title: t("Healing"),
            color: "green",
            data: data.healDone
        });
        setDamageTakenPieChart({
            title: t("DamageTaken"),
            color: "orange",
            data: data.damageTaken
        });
    }, [combatPlayers]);

    const getCombatPlayersAsync = async () => {
        const combatPlayersData = await detailsSpecificalCombatService.combatPlayersByCombatIdAsync(combatId);
        setCombatPlayers(combatPlayersData);

        await getCombatsAsync();
    }

    const getCombatsAsync = async () => {
        const combat = await detailsSpecificalCombatService.combatByIdAsync(combatId);
        if (combat === null) {
            return;
        }

        setCombatLogId(combat.combatLogId);
    }

    const createPieChardData = () => {
        if (combatPlayers === null) {
            return;
        }

        let healDone = [];
        let damageTaken = [];

        let damageDone = new Array(combatPlayers.length);
        for (let i = 0; i < combatPlayers.length; i++) {
            let realmNameIndex = combatPlayers[i].userName.indexOf('-');
            let userName = combatPlayers[i].userName.substr(0, realmNameIndex);

            damageDone[i] = {};
            damageDone[i].name = userName
            damageDone[i].value = combatPlayers[i].damageDone;

            healDone[i] = {};
            healDone[i].name = userName
            healDone[i].value = combatPlayers[i].healDone;

            damageTaken[i] = {};
            damageTaken[i].name = userName
            damageTaken[i].value = combatPlayers[i].damageTaken;
        }

        return {
            damageDone: damageDone,
            healDone: healDone,
            damageTaken: damageTaken
        };
    }

    const render = () => {
        return (
            <div className="details-specifical-combat__container">
                <div className="details-specifical-combat__container_navigate">
                    <h3>{t("Players")}</h3>
                    <div className="btn-group" role="group">
                        <button type="button" className="btn btn-primary" onClick={() => navigate(`/general-analysis?id=${combatLogId}`)}>{t("SelectCombat")}</button>
                    </div>
                </div>
                <div className="form-check form-switch">
                    <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowGeneralDetails((item) => !item)} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{t("ShowCommonStatistics")}</label>
                </div>
                {showGeneralDetails &&
                    <div className="details-specifical-combat__container_general-details-charts">
                        <DetailsPieChart payload={damageDonePieChart} />
                        <DetailsPieChart payload={healDonePieChart} />
                        <DetailsPieChart payload={damageTakenPieChart} />
                    </div>
                }
                <PlayerInformationMemo combatPlayers={combatPlayers} />
            </div>
        );
    };

    return render();
}

export default DetailsSpecificalCombat;