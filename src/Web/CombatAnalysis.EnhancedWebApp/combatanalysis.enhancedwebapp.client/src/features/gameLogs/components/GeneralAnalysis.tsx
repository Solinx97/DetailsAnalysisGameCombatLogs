import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
import Loading from '../../../shared/components/Loading';
import { useLazyGetUniqueCombatsQuery } from '../api/GameLogs.api';
import type { CombatModel } from '../types/CombatModel';
import GeneralAnalysisItems from './GeneralAnalysisItems';
import PersonalTabs from './PersonalTabs';
import Dashboard from './dashboard/Dashboard';

import './GeneralAnalysis.scss';

const GeneralAnalysis: React.FC = () => {
    const { t } = useTranslation('combatDetails/generalAnalysis');

    const navigate = useNavigate();
    
	const { combatLogId } = useParams();
    
    const [searchParams] = useSearchParams();
    const gameVersion = Number(searchParams.get("gameVersion"));

    const [allUniqueCombats, setAllUniqueCombats] = useState<Map<string, CombatModel[]>>(new Map());

    const [getUniqueCombatsByCombatLogId] = useLazyGetUniqueCombatsQuery();

    useEffect(() => {
        const loadAsync = async () => {
            try {
                const uniqueCombats = await getUniqueCombatsByCombatLogId(parseInt(combatLogId || "0")).unwrap();
                setAllUniqueCombats(new Map(Object.entries(uniqueCombats)));
            } catch (error) {
                console.error("Failed to fetch combats:", error);
            }
        }

        loadAsync();
    }, []);

    if (allUniqueCombats.size === 0) {
        return (<Loading />);
    }

    return (
        <div className="general-analysis">
            <div className="general-analysis__container">
                <div className="general-analysis__navigate">
                    <div className="btn-shadow select-logs" onClick={() => navigate(`/game-combat-logs/${gameVersion}`)}>
                        <FontAwesomeIcon
                            icon={faDeleteLeft}
                        />
                        <div>{t("Logs")}</div>
                    </div>
                    <h5>{t("Combats")}</h5>
                </div>
                <PersonalTabs
                    tab={1}
                    tabs={[
                        {
                            id: 0,
                            header: t("Explorer"),
                            content: <Dashboard
                                combatLogId={parseInt(combatLogId || "0")}
                            />
                        },
                        {
                            id: 1,
                            header: t("Informations"),
                            content: <GeneralAnalysisItems
                                allUniqueCombats={allUniqueCombats}
                                combatLogId={parseInt(combatLogId || "0")}
                            />
                        }
                    ]}
                    tabsClassName={"charts"}
                />
            </div>
        </div>
    );
}

export default GeneralAnalysis;