import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useLazyGetGeneralAnalysisByIdQuery } from '../../store/api/CombatParserApi';
import GeneralAnalysisItem from './GeneralAnalysisItem';

import "../../styles/generalAnalysis.scss";

const GeneralAnalysis = () => {
    const navigate = useNavigate();
    const { t } = useTranslation("combatDetails/generalAnalysis");

    const [combatLogId, setCombatLogId] = useState(0);
    const [combats, setCombats] = useState([]);
    const [uniqueCombats, setUniqueCombats] = useState([]);

    const [generalAnalysisAsync] = useLazyGetGeneralAnalysisByIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCombatLogId(+queryParams.get("id"));
    }, []);

    useEffect(() => {
        if (combatLogId <= 0) {
            return;
        }

        const getCombats = async () => {
            await getCombatsAsync();
        };

        getCombats();
    }, [combatLogId]);

    useEffect(() => {
        if (combats.length === 0) {
            return;
        }

        createListOfSimilarCombats();
    }, [combats]);

    const getCombatsAsync = async () => {
        const combats = await generalAnalysisAsync(combatLogId);
        if (combats.data !== undefined) {
            setCombats(combats.data);
        }
    }

    const createListOfSimilarCombats = () => {
        const uniqueCombatList = [];
        const uniqueNames = [];

        const sortedCombats = quickSortByFinishDate(combats);

        for (let i = 0; i < sortedCombats.length; i++) {
            if (uniqueNames.indexOf(sortedCombats[i].name) > -1) {
                continue;
            }

            const foundCombats = sortedCombats.filter(x => x.name === sortedCombats[i].name);
            uniqueCombatList.push(foundCombats);

            uniqueNames.push(sortedCombats[i].name);
        }

        setUniqueCombats(uniqueCombatList);
    }

    const quickSortByFinishDate = (array) => {
        if (array.length <= 1) {
            return array;
        }

        const pivot = array[array.length - 1];
        const left = [];
        const right = [];

        for (let i = 0; i < array.length - 1; i++) {
            if (array[i].startDate < pivot.startDate) {
                left.push(array[i]);
            }
            else {
                right.push(array[i])
            }
        }

        return [...quickSortByFinishDate(left), pivot, ...quickSortByFinishDate(right)];
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
                <h3 className="title">{t("Combats")}</h3>
            </div>
            <ul className="combats__container">
                {uniqueCombats?.map((combats, index) => (
                        <li key={index}>
                            <GeneralAnalysisItem
                                uniqueCombats={combats}
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