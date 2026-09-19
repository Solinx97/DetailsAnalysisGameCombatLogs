import { faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import Loading from '../../../shared/components/Loading';
import { useLazyGetUniqueCombatsByCombatLogIdQuery } from '../api/GameLogs.api';
import type { CombatModel } from '../types/CombatModel';
import PersonalTabs from './PersonalTabs';
import GeneralAnalysisItems from './GeneralAnalysisItems';
import Dashboard from './dashboard/Dashboard';

import './GeneralAnalysis.scss';

const GeneralAnalysis: React.FC = () => {
    const { t } = useTranslation('combatDetails/generalAnalysis');

    const navigate = useNavigate();

    const [combatLogId, setCombatLogId] = useState<number>(0);
    const [allUniqueCombats, setAllUniqueCombats] = useState<Map<string, CombatModel[]>>(new Map());

    const [getUniqueCombatsByCombatLogId] = useLazyGetUniqueCombatsByCombatLogIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        const id: number = parseInt(queryParams.get("id") || '0');
        setCombatLogId(id);

        const loadAsync = async () => {
            try {
                const uniqueCombats = await getUniqueCombatsByCombatLogId(id).unwrap();
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
            <PersonalTabs
                tab={1}
                tabs={[
                    {
                        id: 0,
                        header: t("Dashboard"),
                        content: <Dashboard
                            combatLogId={combatLogId}
                        />
                    },
                    {
                        id: 1,
                        header: t("Informations"),
                        content: <GeneralAnalysisItems
                            allUniqueCombats={allUniqueCombats}
                            combatLogId={combatLogId}
                        />
                    }
                ]}
                tabsClassName={"charts"}
            />
        </div>
    );
}

export default GeneralAnalysis;