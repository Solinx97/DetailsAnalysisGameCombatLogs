import WoWGameDataContext from '@/context/WoWGameDataContext';
import { faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useState } from 'react';
import { useLazyGetCharacterDungeonsQuery, useLazyGetCharacterRaidsQuery } from '../api/WoWCharacter.api';
import type { CharacterDungeonModel } from '../types/dungeon/CharacterDungeonModel';
import SelectedDungeonsExpantion from './SelectedDungeonsExpantion';

const CharacterDungeons: React.FC<{ isRaids: boolean }> = ({ isRaids }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [selectedExpansionId, setSelectedExpantionId] = useState<number>(0);
    const [dungeons, setDungeons] = useState<CharacterDungeonModel | null>(null);
    const [onlyCurrentWeekCompleted, setCurrentWeekCompleted] = useState<boolean>(false);
    
    const [getDungeons] = isRaids
        ? useLazyGetCharacterRaidsQuery()
        : useLazyGetCharacterDungeonsQuery();

    useEffect(() => {
        if (!username || username.trim().length === 0) {
            return;
        }

        const loadAsync = async () => {
            try {
                const receivedDungeons = await getDungeons({ username, serverName, regionName }).unwrap();
                setDungeons(receivedDungeons);
            } catch (error) {
                console.error("Failed to fetch character dungeons:", error);
            }
        }

        loadAsync();
    }, []);

    if (!username || username.trim().length === 0) {
        return (<div>No data</div>);
    }

    if (!dungeons) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="raids">
            <div className="raids__title">
                <h6>{isRaids ? t("Raids") : t("Dungeons")}</h6>
            </div>
            <div className="form-check">
                <input className="form-check-input" type="checkbox" value="" id="checkIndeterminate"
                    defaultChecked={onlyCurrentWeekCompleted}
                    onChange={() => setCurrentWeekCompleted(prev => !prev)} />
                <label className="form-check-label" htmlFor="checkIndeterminate">
                    {t("OnlyCurrentWeekCompleted")}
                </label>
            </div>
            <ul className="raids__container">
                {dungeons.expansions.map((expansion, index) => (
                    <li key={index} className="raids__item">
                        <div className={`btn-shadow ${selectedExpansionId === expansion.expansion.id ? 'selected' : ''}`}
                            onClick={() => setSelectedExpantionId(prev => prev === 0 ? expansion.expansion.id : 0)}>
                            <FontAwesomeIcon
                                icon={selectedExpansionId === expansion.expansion.id ? faLocationCrosshairs : faPlus}
                            />
                            <div>{expansion.expansion.name}</div>
                        </div>
                        {selectedExpansionId === expansion.expansion.id &&
                            <SelectedDungeonsExpantion
                                instances={expansion.instances}
                                onlyCurrentWeekCompleted={onlyCurrentWeekCompleted}
                            />
                        }
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default CharacterDungeons;