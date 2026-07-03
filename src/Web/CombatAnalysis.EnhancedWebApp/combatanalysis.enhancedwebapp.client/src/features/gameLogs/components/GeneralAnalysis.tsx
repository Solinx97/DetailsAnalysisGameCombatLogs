import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import Loading from '../../../shared/components/Loading';
import { useLazyGetCombatsByCombatLogIdQuery } from '../api/GameLogs.api';
import type { CombatModel } from '../types/CombatModel';
import GeneralAnalysisItem from './GeneralAnalysisItem';

import './GeneralAnalysis.scss';

const GeneralAnalysis: React.FC = () => {
    const fixedNumberUntil = 2;
    
    const { t } = useTranslation("combatDetails/generalAnalysis");
    const navigate = useNavigate();

    const [combatLogId, setCombatLogId] = useState<number>(0);
    const [allUniqueCombats, setUniqueCombats] = useState<Array<CombatModel[]>>([]);

    const [getCombatsByCombatLogId] = useLazyGetCombatsByCombatLogIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const id: number = parseInt(queryParams.get("id") || '0');
        setCombatLogId(id);

        const getCombats = async () => {
            await getCombatsAsync(id);
        }

        if (id > 0) {
            getCombats();
        }
    }, []);

    const getCombatsAsync = async (id: number) => {
        try {
            const combats = await getCombatsByCombatLogId(id);
            if (combats.data !== undefined) {
                createListOfSimilarCombats(combats.data);
            }
        } catch (error) {
            console.error("Failed to fetch combats:", error);
        }
    }

    const createListOfSimilarCombats = (combats: CombatModel[]) => {
        const uniqueCombatList: Array<CombatModel[]> = [];
        const uniqueNames = new Set();

        const umblockedCombatsArray = Object.assign([], combats);
        const sortedCombats: CombatModel[] = umblockedCombatsArray.sort((a: CombatModel, b: CombatModel) => a.startDate.localeCompare(b.startDate));

        sortedCombats.forEach((combat: CombatModel) => {
            if (!uniqueNames.has(combat.boss.name)) {
                uniqueNames.add(combat.boss.name);
                const foundCombats: CombatModel[] = sortedCombats.filter(x => x.boss.name === combat.boss.name);
                uniqueCombatList.push(foundCombats);
            }
        });

        setUniqueCombats(uniqueCombatList);
    }

    const getValueShortName = (value: number): string => {
        const thousands = value / 1000;
        const millions = value / 1000000;

        if (millions >= 1) {
            return `${millions.toFixed(fixedNumberUntil)} M`;
        }
        else if (thousands >= 1) {
            return `${thousands.toFixed(fixedNumberUntil)} K`;
        }

        return `${value}`;
    }

    if (combatLogId === 0) {
        return (<Loading />);
    }

    return (
        <div className="general-analysis__container">
            <div className="general-analysis__navigate">
                <div className="btn-shadow select-logs" onClick={() => navigate("/game-combat-logs")}>
                    <FontAwesomeIcon
                        icon={faDeleteLeft}
                    />
                    <div>{t("Logs")}</div>
                </div>
                <h5>{t("Combats")}</h5>
            </div>
            <ul className="cards">
                {allUniqueCombats.map((uniqueCombats, index) => (
                        <li key={index}>
                            <GeneralAnalysisItem
                                uniqueCombats={uniqueCombats}
                                combatLogId={combatLogId}
                                getValueShortName={getValueShortName}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default GeneralAnalysis;