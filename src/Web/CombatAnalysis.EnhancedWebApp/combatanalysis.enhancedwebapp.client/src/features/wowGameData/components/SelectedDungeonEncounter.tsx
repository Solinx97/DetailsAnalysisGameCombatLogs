import useFormatting from '@/shared/hooks/useFormatting';
import { faCalendarWeek } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type { DungeonModeEncountModel } from '../types/dungeon/DungeonModeEncountModel';
import { useEffect, useState } from 'react';

const SelectedDungeonEncounter: React.FC<{ encounters: DungeonModeEncountModel[], onlyCurrentWeekCompleted: boolean }> = ({ encounters, onlyCurrentWeekCompleted }) => {
    const { getDate, getPreviousDayOfWeek } = useFormatting();

    const [allEncounters, setAllEncounters] = useState<DungeonModeEncountModel[]>([]);

    useEffect(() => {
        if (onlyCurrentWeekCompleted) {
            setAllEncounters(encounters.filter(x => getPreviousDayOfWeek(2) < new Date(x.lastKillTime)));
        }
        else {
            setAllEncounters(encounters);
        }
    }, [encounters]);

    return (
        <ul className="dungeon-encounters">
            {allEncounters.map((encounter, index) => (
                <li key={index} className="encounter">
                    <div>{encounter.encounter.name}</div>
                    <div className="count">{encounter.completedCount}</div>
                    <div>{getDate(encounter.lastKillTime)}</div>
                    {getPreviousDayOfWeek(2) < new Date(encounter.lastKillTime) &&
                        <FontAwesomeIcon
                            icon={faCalendarWeek}
                        />
                    }
                </li>
            ))
            }
        </ul>
    );
}

export default SelectedDungeonEncounter;