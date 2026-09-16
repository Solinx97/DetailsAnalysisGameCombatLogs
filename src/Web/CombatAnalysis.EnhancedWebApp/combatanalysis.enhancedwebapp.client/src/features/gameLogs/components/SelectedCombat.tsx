import { faDeleteLeft, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState, type ChangeEvent } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useGetGenericChartDamageDoneQuery } from '../api/DamageDone.api';
import { useLazyGetCombatPlayersByCombatIdQuery } from '../api/GameLogs.api';
import { useGetGenericChartHealDoneQuery } from '../api/HealDone.api';
import type { CombatDetailsModel } from '../types/CombatDetailsModel';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import SelectedCombatChart from './charts/SelectedCombatChart';
import Details from './details/Details';
import PersonalTabs from './PersonalTabs';

import './SelectedCombat.scss';

const SelectedCombat: React.FC = () => {
    const fixedNumberUntil = 2;

    const { t } = useTranslation('combatDetails/selectedCombat');

    const navigate = useNavigate();

    const [gameVersion, setgGameVersion] = useState<number>(-1);
    const [details, setDetails] = useState<CombatDetailsModel>({
        id: 0,
        detailsType: 0,
        combatLogId: 0,
        name: '',
        number: 0,
        isWin: false,
        duration: 0
    });
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
        const queryParams = new URLSearchParams(window.location.search);

        const id: number = parseInt(queryParams.get("id") || '0');
        const combatLogId: number = parseInt(queryParams.get("combatLogId") || '0');
        const name: string = queryParams.get("name") || '';
        const number: number = parseInt(queryParams.get("number") || '0');
        const isWin: boolean = queryParams.get("isWin") === 'true';
        const duration: number = parseInt(queryParams.get("duration") || "1");

        setDetails({
            id,
            detailsType: 0,
            combatLogId,
            name,
            number,
            isWin,
            duration,
        });

        const version: number = parseInt(queryParams.get("gameVersion") || '-1');
        setgGameVersion(version);
    }, []);

    useEffect(() => {
        if (details.id <= 0) {
            return;
        }

        const fetchData = async () => {
            await getCombatPlayersAsync();
        }

        fetchData();
    }, [details.id]);

    const handlerSearch = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        const foundPlayers = combatPlayers.filter((item) => item.player.username.toLowerCase().startsWith(e?.target.value.toLowerCase() || ""));
        setSelectedPlayers(foundPlayers);
    }

    const getCombatPlayersAsync = async () => {
        try {
            const combatPlayersResult = await getCombatPlayersByCombatIdAsync(details.id).unwrap();
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
        <div className="selected-combat__container">
            <div className="selected-combat__navigate">
                <div className="btn-shadow select-combat" onClick={() => navigate(`/general-analysis?id=${details.combatLogId}&gameVersion=${gameVersion}`)}>
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
                    <div>{details.name}</div>
                    <div className={`combat-number ${details.isWin ? 'win' : 'lose'}`}>{details.number}</div>
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
                                combatId={details.id}
                                colors={getRandomColors(selectedPlayers.length)}
                                useGetGenericChartQuery={useGetGenericChartDamageDoneQuery}
                            />
                        },
                        {
                            id: 1,
                            header: t("Healing"),
                            content: <SelectedCombatChart
                                combatPlayers={selectedPlayers}
                                combatId={details.id}
                                colors={getRandomColors(selectedPlayers.length)}
                                useGetGenericChartQuery={useGetGenericChartHealDoneQuery}
                            />
                        }
                    ]}
                    tabsClassName={"charts"}
                />
            }
            <Details
                details={details}
                combatPlayers={selectedPlayers}
                getValueShortName={getValueShortName}
                gameVersion={gameVersion}
                t={t}
            />
        </div>
    );
}

export default SelectedCombat;