import { faDeleteLeft, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState, type ChangeEvent } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
import { useGetGenericChartDamageDoneQuery } from '../api/DamageDone.api';
import { useLazyGetCombatPlayersByCombatIdQuery } from '../api/GameLogs.api';
import { useGetGenericChartHealDoneQuery } from '../api/HealDone.api';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import SelectedCombatChart from './charts/SelectedCombatChart';
import Details from './details/Details';
import PersonalTabs from './PersonalTabs';

import './SelectedCombat.scss';

const SelectedCombat: React.FC = () => {
    const fixedNumberUntil = 2;

    const { t } = useTranslation('combatDetails/selectedCombat');

    const navigate = useNavigate();
    
	const { combatId } = useParams();

    const [searchParams] = useSearchParams();
    const combatLogId = searchParams.get("combatLogId");
    const name = searchParams.get("name");
    const number = Number(searchParams.get("number"));
    const isWin = searchParams.get("isWin") === "true";
    const gameVersion = Number(searchParams.get("gameVersion"));
    
    const [combatPlayers, setCombatPlayers] = useState<CombatPlayerModel[]>([]);
    const [selectedPlayers, setSelectedPlayers] = useState<CombatPlayerModel[]>([]);
    const [showCommonStatistics, setShowCommonStatistics] = useState(false);
    const [showSearch, setShowSearch] = useState(false);

    const maxWidth = 425;
    const screenSize = {
        width: window.innerWidth,
        height: window.innerHeight
    };

    const [getCombatPlayersByCombatIdAsync] = useLazyGetCombatPlayersByCombatIdQuery();

    const filterContent = useRef<HTMLInputElement>(null);

    useEffect(() => {
        const fetchData = async () => {
            await getCombatPlayersAsync();
        }

        fetchData();
    }, []);

    const handlerSearch = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        const foundPlayers = combatPlayers.filter((item) => item.player.username.toLowerCase().startsWith(e?.target.value.toLowerCase() || ""));
        setSelectedPlayers(foundPlayers);
    }

    const getCombatPlayersAsync = async () => {
        try {
            const combatPlayersResult = await getCombatPlayersByCombatIdAsync(parseInt(combatId || "0")).unwrap();
            setCombatPlayers(combatPlayersResult);
            setSelectedPlayers(combatPlayersResult);

            return combatPlayersResult;
        } catch (error) {
            console.error("Errror to load Combat players");

            return [];
        }
    }

    const cleanSearch = () => {
        if (filterContent.current) {
            filterContent.current.value = "";
        }

        setSelectedPlayers(combatPlayers);
    }

    const getValueShortName = (value: number): string => {
        const thousands = value / 1000;
        const millions = value / 1000000;

        if (millions >= 1) {
            return `${millions.toFixed(fixedNumberUntil)}M`;
        }
        else if (thousands >= 1) {
            return `${thousands.toFixed(fixedNumberUntil)}K`;
        }

        return value.toString();
    }

    const getRandomColors = (raidSize: number) => {
        const colors = new Array<string>();

        for (let index = 0; index < raidSize; index++) {
            colors.push(`hsl(${Math.floor(Math.random() * 360)}, 70%, 50%)`);
        }

        return colors;
    }

    return (
        <div className="selected-combat">
            <div className="selected-combat__container">
                <div className="selected-combat__navigate">
                    <div className="btn-shadow select-combat" onClick={() => navigate(`/general-analysis/${combatLogId}?gameVersion=${gameVersion}`)}>
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
                    <div className="boss">
                        <div>{name}</div>
                        <div className={`combat-number ${isWin ? 'win' : 'lose'}`}>{number}</div>
                    </div>
                </div>
                {showSearch &&
                    <div className="mb-3 search-people">
                        <label htmlFor="inputUsername" className="form-label">{t("SearchPlayer")}</label>
                        <div className="add-new-people__search-input">
                            <input type="text" className="form-control" placeholder={t("TypeUsername") || ""} id="inputUsername"
                                ref={filterContent} onChange={handlerSearch} />
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Clean") || ""}
                                onClick={cleanSearch}
                            />
                        </div>
                    </div>
                }
                {(combatPlayers.length > 0 && screenSize.width > maxWidth) &&
                    <div className="form-check form-switch">
                        <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowCommonStatistics((item) => !item)} />
                        <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{showCommonStatistics ? t("HideCommonStatistics") : t("ShowCommonStatistics")}</label>
                    </div>
                }
                {showCommonStatistics &&
                    <PersonalTabs
                        tab={0}
                        tabs={[
                            {
                                id: 0,
                                header: t("Damage"),
                                content: <SelectedCombatChart
                                    combatPlayers={selectedPlayers}
                                    combatId={parseInt(combatId || "0")}
                                    colors={getRandomColors(selectedPlayers.length)}
                                    useGetGenericChartQuery={useGetGenericChartDamageDoneQuery}
                                />
                            },
                            {
                                id: 1,
                                header: t("Healing"),
                                content: <SelectedCombatChart
                                    combatPlayers={selectedPlayers}
                                    combatId={parseInt(combatId || "0")}
                                    colors={getRandomColors(selectedPlayers.length)}
                                    useGetGenericChartQuery={useGetGenericChartHealDoneQuery}
                                />
                            }
                        ]}
                        tabsClassName={"charts"}
                    />
                }
                <Details
                    combatPlayers={selectedPlayers}
                    getValueShortName={getValueShortName}
                    t={t}
                />
            </div>
        </div>
    );
}

export default SelectedCombat;