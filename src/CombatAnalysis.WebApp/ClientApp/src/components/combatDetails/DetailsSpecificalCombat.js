import { faDeleteLeft, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayersByCombatIdQuery, useLazyGetPlayersDeathByPlayerIdQuery } from '../../store/api/core/CombatParser.api';
import PlayerInformation from '../childs/PlayerInformation';
import PersonalTabs from '../common/PersonalTabs';
import Dashboard from './Dashboard';
import GeneralDetailsChart from './GeneralDetailsChart';

import "../../styles/detailsSpecificalCombat.scss";

const DetailsSpecificalCombat = () => {
    const { t } = useTranslation("combatDetails/detailsSpecificalCombat");

    const navigate = useNavigate();

    const [combatDetails, setCombatDetails] = useState({ combatId: 0, combatLogId: 0, combatName: "", tab: 0 });
    const [combatPlayers, setCombatPlayers] = useState([]);
    const [playersDeath, setPlayersDeath] = useState([]);
    const [searchCombatPlayers, setSearchCombatPlayers] = useState([]);
    const [showCommonDetails, setShowCommonDetails] = useState(false);
    const [showSearch, setShowSearch] = useState(false);

    const maxWidth = 425;
    const screenSize = {
        width: window.innerWidth,
        height: window.innerHeight
    };

    const [getCombatPlayersByCombatIdAsync] = useLazyGetCombatPlayersByCombatIdQuery();
    const [getPlayersDeathByCombatIdAsync] = useLazyGetPlayersDeathByPlayerIdQuery();

    const filterContent = useRef(null);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatDetails({
            combatId: +queryParams.get("id"),
            combatLogId: +queryParams.get("combatLogId"),
            combatName: queryParams.get("name"),
            tab: +queryParams.get("tab")
        });
    }, []);

    useEffect(() => {
        if (combatDetails.combatId <= 0) {
            return;
        }

        const fetchData = async () => {
            const combatPlayersData = await getCombatPlayersAsync();
            await getPlayersDeathAsync(combatPlayersData);
        };

        fetchData();
    }, [combatDetails.combatId])

    const handlerSearch = (e) => {
        const filteredPeople = combatPlayers.filter((item) => item.username.toLowerCase().startsWith(e.target.value.toLowerCase()));
        setSearchCombatPlayers(filteredPeople);
    }

    const getCombatPlayersAsync = async () => {
        const combatPlayersResult = await getCombatPlayersByCombatIdAsync(combatDetails.combatId);
        if (combatPlayersResult.data !== undefined) {
            setCombatPlayers(combatPlayersResult.data);
            setSearchCombatPlayers(combatPlayersResult.data);

            return combatPlayersResult.data;
        }

        return [];
    }

    const getPlayersDeathAsync = async (players) => {
        const deathsPromises = players.map(player => getPlayersDeathByCombatIdAsync(player.id));
        const deathsResults = await Promise.all(deathsPromises);
        const deaths = deathsResults.filter(result => result.data && result.data.length > 0).map(result => result.data[0]);
        setPlayersDeath(deaths);
    };

    const cleanSearch = () => {
        filterContent.current.value = "";

        setSearchCombatPlayers(combatPlayers);
    }

    return (
        <div className="details-specifical-combat__container">
            <div className="details-specifical-combat__navigate">
                <div className="btn-shadow select-combat" onClick={() => navigate(`/general-analysis?id=${combatDetails.combatLogId}`)}>
                    <FontAwesomeIcon
                        icon={faDeleteLeft}
                    />
                    <div>{t("SelectCombat")}</div>
                </div>
                <h5>{t("Players")}</h5>
                <div className="btn-shadow search-icon" onClick={() => setShowSearch((item) => !item)}>
                    <FontAwesomeIcon
                        icon={showSearch ? faMagnifyingGlassMinus : faMagnifyingGlassPlus}
                    />
                    <div>{t("Search")}</div>
                </div>
                <div>{combatDetails.combatName}</div>
            </div>
            {showSearch &&
                <div className="mb-3 search-people">
                    <label htmlFor="inputUsername" className="form-label">{t("SearchPlayer")}</label>
                    <div className="add-new-people__search-input">
                        <input type="text" className="form-control" placeholder={t("TypeUsername")} id="inputUsername"
                            ref={filterContent} onChange={handlerSearch} />
                        <FontAwesomeIcon
                            icon={faXmark}
                            title={t("Clean")}
                            onClick={cleanSearch}
                        />
                    </div>
                </div>
            }
            {(combatPlayers.length > 0 && screenSize.width > maxWidth) &&
                <div className="form-check form-switch">
                    <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowCommonDetails((item) => !item)} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{t("ShowCommonStatistics")}</label>
                </div>
            }
            {showCommonDetails &&
                <GeneralDetailsChart
                    combatPlayers={searchCombatPlayers}
                />
            }
            <PersonalTabs
                tab={combatDetails.tab}
                tabs={[
                    {
                        id: 0,
                        header: t("Dashboard"),
                        content: <Dashboard combatId={combatDetails.combatId} combatLogId={combatDetails.combatLogId} players={searchCombatPlayers} combatName={combatDetails.combatName} playersDeath={playersDeath} />
                    },
                    {
                        id: 1,
                        header: t("Details"),
                        content: <PlayerInformation combatPlayers={searchCombatPlayers} combatId={combatDetails.combatId} combatLogId={combatDetails.combatLogId} combatName={combatDetails.combatName} />
                    }
                ]}
            />
        </div>
    );
}

export default DetailsSpecificalCombat;