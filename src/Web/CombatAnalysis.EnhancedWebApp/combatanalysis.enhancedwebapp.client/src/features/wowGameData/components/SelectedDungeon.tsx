import WoWGameDataContext from '@/context/WoWGameDataContext';
import { DungeonModeType } from '@/shared/helpers/EnumHelper';
import useFormatting from '@/shared/hooks/useFormatting';
import { faChartSimple, faFlagCheckered, faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useState } from 'react';
import type { DungeonModeModel } from '../types/dungeon/DungeonModeModel';
import SelectedDungeonEncounter from './SelectedDungeonEncounter';

const SelectedDungeon: React.FC<{ 
    modes: DungeonModeModel[];
    onlyCurrentWeekCompleted: boolean;
}> = ({ modes, onlyCurrentWeekCompleted }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t } = context;
    const { getPreviousDayOfWeek } = useFormatting();

    const [selectedInstanceId, setSelectedInstanceId] = useState<number>(-1);

    const getEncounters = (index: number, mode: DungeonModeModel) => {
        const isAnyEixst = onlyCurrentWeekCompleted
            ? mode.progress.encounters.filter(x => getPreviousDayOfWeek(2) < new Date(x.lastKillTime)).length > 0
            : mode.progress.encounters.length > 0;
        if (!isAnyEixst) {
            return (<></>);
        }

        return (
            <li key={index} className="dungeon-details">
                <div className="dungeon-details__information">
                    <div>{mode.difficulty.name}</div>
                    <FontAwesomeIcon
                        icon={mode.status.type === DungeonModeType[0]
                            ? faFlagCheckered
                            : mode.status.type === DungeonModeType[1]
                                ? faChartSimple
                                : faPlus}
                    />
                    <div className="count">{mode.progress.completedCount}</div>
                </div>
                <div className={`btn-shadow ${selectedInstanceId === index ? 'selected' : ''}`}
                    onClick={() => setSelectedInstanceId(prev => prev === -1 ? index : -1)}>
                    <FontAwesomeIcon
                        icon={selectedInstanceId === index ? faLocationCrosshairs : faPlus}
                    />
                    <div>{t("Encounters")} [{mode.progress.encounters.length}]</div>
                </div>
                {selectedInstanceId === index &&
                    <SelectedDungeonEncounter
                        encounters={mode.progress.encounters}
                        onlyCurrentWeekCompleted={onlyCurrentWeekCompleted}
                    />
                }
            </li>
        );
    }

    return (
        <div className="dungeon">
            <ul className="dungeon__item">
                {
                    modes.map((mode, index) => (getEncounters(index, mode)))
                }
            </ul>
        </div>
    );
}

export default SelectedDungeon;