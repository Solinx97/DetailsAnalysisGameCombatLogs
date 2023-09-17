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

    const getCombatsAsync = async () => {
        const combats = await generalAnalysisAsync(combatLogId);
        if (combats.data !== undefined) {
            setCombats(combats.data);
        }
    }

    return (
        <div className="general-analysis__container">
            <div className="general-analysis__navigate">
                <h3 className="title">{t("Combats")}</h3>
                <button type="button" className="btn btn-light" onClick={() => navigate("/main-information")}>{t("Logs")}</button>
            </div>
            <ul className="combats__container">
                {combats?.map((item) => (
                        <li key={item.id}>
                            <GeneralAnalysisItem
                                combat={item}
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