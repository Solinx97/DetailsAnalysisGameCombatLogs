import type { CombatModel } from '../types/CombatModel';
import GeneralAnalysisItem from './GeneralAnalysisItem';

interface GeneralAnalysisItemsProps {
    allUniqueCombats: Map<string, CombatModel[]>;
    combatLogId: number;
}

const GeneralAnalysisItems: React.FC<GeneralAnalysisItemsProps> = ({ allUniqueCombats, combatLogId }) => {
    const fixedNumberUntil = 2;

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

    return (
        <ul className="cards">
            {Array.from(allUniqueCombats.entries()).map(([key, uniqueCombats]) => (
                <li key={key}>
                    <GeneralAnalysisItem
                        uniqueCombats={uniqueCombats}
                        combatLogId={combatLogId}
                        getValueShortName={getValueShortName}
                    />
                </li>
            ))
            }
        </ul>
    );
}

export default GeneralAnalysisItems;