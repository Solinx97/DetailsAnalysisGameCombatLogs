import { faDeleteLeft, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayersByCombatIdQuery } from '../../store/api/CombatParserApi';
import PlayerInformation from '../childs/PlayerInformation';
import PersonalTabs from '../common/PersonalTabs';
import Dashboard from './Dashboard';
import GeneralDetailsChart from './GeneralDetailsChart';

import "../../styles/detailsSpecificalCombat.scss";

const DetailsSpecificalCombat = () => {
    const { t } = useTranslation("combatDetails/detailsSpecificalCombat");

    const navigate = useNavigate();

    const [combatId, setCombatId] = useState(0);
    const [combatLogId, setCombatLogId] = useState(0);
    const [combatName, setCombatName] = useState("");
    const [tab, setTab] = useState(0);
    const [combatPlayers, setCombatPlayers] = useState([]);
    const [searchCombatPlayers, setSearchCombatPlayers] = useState([]);
    const [showCommonDetails, setShowCommonDetails] = useState(false);
    const [showSearch, setShowSearch] = useState(false);

    const [getCombatPlayersByCombatIdAsync] = useLazyGetCombatPlayersByCombatIdQuery();

    const filterContent = useRef(null);

    const PlayerInformationMemo = memo(PlayerInformation);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatId(+queryParams.get("id"));
        setCombatLogId(+queryParams.get("combatLogId"));
        setCombatName(queryParams.get("name"));
        setTab(+queryParams.get("tab"));
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

    const handlerSearch = (e) => {
        const filteredPeople = combatPlayers.filter((item) => item.userName.toLowerCase().startsWith(e.target.value.toLowerCase()));
        setSearchCombatPlayers(filteredPeople);
    }

    const getCombatPlayersAsync = async () => {
        const combatPlayers = await getCombatPlayersByCombatIdAsync(combatId);
        if (combatPlayers.data !== undefined) {
            setCombatPlayers(combatPlayers.data);
            setSearchCombatPlayers(combatPlayers.data);
        }
    }

    const cleanSearch = () => {
        filterContent.current.value = "";

        setSearchCombatPlayers(combatPlayers);
    }

    return (
        <div className="details-specifical-combat__container">
            <div className="details-specifical-combat__navigate">
                <div className="btn-shadow select-combat" onClick={() => navigate(`/general-analysis?id=${combatLogId}`)}>
                    <FontAwesomeIcon
                        icon={faDeleteLeft}
                    />
                    <div>{t("SelectCombat")}</div>
                </div>
                <h3>{t("Players")}</h3>
                <div className="btn-shadow search-icon" onClick={() => setShowSearch((item) => !item)}>
                    {showSearch
                        ? <FontAwesomeIcon
                            icon={faMagnifyingGlassMinus}
                        />
                        : <FontAwesomeIcon
                            icon={faMagnifyingGlassPlus}
                        />
                    }
                    <div>{t("Search")}</div>
                </div>
                <div>{combatName}</div>
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
            {combatPlayers.length > 0 &&
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
                tab={tab}
                tabs={[
                    { id: 0, header: "Dashboard", content: <Dashboard combatId={combatId} combatLogId={combatLogId} players={searchCombatPlayers} combatName={combatName} /> },
                    { id: 1, header: "Common", content: <PlayerInformationMemo combatPlayers={searchCombatPlayers} combatId={combatId} combatLogId={combatLogId} combatName={combatName} /> }
                ]}
            />
        </div>
    );
}

export default DetailsSpecificalCombat;