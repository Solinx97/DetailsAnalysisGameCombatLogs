import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useLazyGetGeneralAnalysisByIdQuery } from '../../store/api/CombatParserApi';
import Loading from '../Loading';
import GeneralAnalysisItem from './GeneralAnalysisItem';

import "../../styles/generalAnalysis.scss";

const GeneralAnalysis = () => {
    const { t } = useTranslation("combatDetails/generalAnalysis");
    const navigate = useNavigate();

    const [combatLogId, setCombatLogId] = useState(0);
    const [allUniqueCombats, setUniqueCombats] = useState([]);

    const [generalAnalysisAsync] = useLazyGetGeneralAnalysisByIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const id = +queryParams.get("id");
        setCombatLogId(id);

        const getCombats = async () => {
            await getCombatsAsync(id);
        }

        if (id > 0) {
            getCombats();
        }
    }, []);

    const getCombatsAsync = async (id) => {
        try {
            const combats = await generalAnalysisAsync(id);
            if (combats.data !== undefined) {
                createListOfSimilarCombats(combats.data);
            }
        } catch (error) {
            console.error("Failed to fetch combats:", error);
        }
    }

    const createListOfSimilarCombats = (combats) => {
        const uniqueCombatList = [];
        const uniqueNames = new Set();

        const umblockedCombatsArray = Object.assign([], combats);
        const sortedCombats = umblockedCombatsArray.sort((a, b) => a.startDate.localeCompare(b.startDate));

        sortedCombats.forEach(combat => {
            if (!uniqueNames.has(combat.name)) {
                uniqueNames.add(combat.name);
                const foundCombats = sortedCombats.filter(x => x.name === combat.name);
                uniqueCombatList.push(foundCombats);
            }
        });

        setUniqueCombats(uniqueCombatList);
    }

    if (combatLogId === 0) {
        return (<Loading />);
    }

    return (
        <div className="general-analysis__container">
            <div className="general-analysis__navigate">
                <div className="btn-shadow select-logs" onClick={() => navigate("/main-information")}>
                    <FontAwesomeIcon
                        icon={faDeleteLeft}
                    />
                    <div>{t("Logs")}</div>
                </div>
                <div className="title">{t("Combats")}</div>
            </div>
            <ul className="combats__container">
                {allUniqueCombats?.map((uniqueCombats, index) => (
                        <li key={index}>
                            <GeneralAnalysisItem
                                uniqueCombats={uniqueCombats}
                                combatLogId={combatLogId}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default GeneralAnalysis;