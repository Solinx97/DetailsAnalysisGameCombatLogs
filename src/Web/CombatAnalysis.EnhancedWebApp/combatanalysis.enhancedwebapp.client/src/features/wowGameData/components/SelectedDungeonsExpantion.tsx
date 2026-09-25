import { faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useCallback, useState } from 'react';
import type { DungeonInstanceModel } from '../types/dungeon/DungeonInstanceModel';
import SelectedDungeon from './SelectedDungeon';
import useFormatting from '@/shared/hooks/useFormatting';

const SelectedDungeonsExpantion: React.FC<{ instances: DungeonInstanceModel[], onlyCurrentWeekCompleted: boolean }> = ({ instances, onlyCurrentWeekCompleted }) => {
    const [selectedInstanceId, setSelectedInstanceId] = useState<number>(0);

    const { getPreviousDayOfWeek } = useFormatting();

    const checkIfAnyCompleted = useCallback(
        (instance: DungeonInstanceModel) => {
            const previousDay = getPreviousDayOfWeek(2);

            return instance.modes.some(mode =>
                onlyCurrentWeekCompleted
                    ? mode.progress.encounters.some(
                        x => previousDay < new Date(x.lastKillTime)
                    )
                    : mode.progress.encounters.length > 0
            );
        },
        [onlyCurrentWeekCompleted]
    );

    const getInstanceDetails = (instance: DungeonInstanceModel, index: number) => {
        const anyExist = checkIfAnyCompleted(instance);

        if ((onlyCurrentWeekCompleted && anyExist) || !onlyCurrentWeekCompleted) {
            return (
                <li key={index}>
                    <div className={`btn-shadow ${selectedInstanceId === instance.instance.id ? 'selected' : ''}`}
                        onClick={() => setSelectedInstanceId(prev => prev === 0 ? instance.instance.id : 0)}>
                        <FontAwesomeIcon
                            icon={selectedInstanceId === instance.instance.id ? faLocationCrosshairs : faPlus}
                        />
                        <div>{instance.instance.name}</div>
                    </div>
                    {selectedInstanceId === instance.instance.id &&
                        <SelectedDungeon
                            modes={instance.modes}
                            onlyCurrentWeekCompleted={onlyCurrentWeekCompleted}
                        />
                    }
                </li>
            );
        }

        return (
            <li key={index}>
                <div className="btn-shadow not-any-completed"
                    onClick={() => { }}>
                    <div>{instance.instance.name}</div>
                </div>
            </li>
        );
    }

    return (
        <div className="expansion-raids">
            <ul className="expansion-raids__item">
                {instances.map((instance, index) => (
                    getInstanceDetails(instance, index)
                ))
                }
            </ul>
        </div>
    );
}

export default SelectedDungeonsExpantion;