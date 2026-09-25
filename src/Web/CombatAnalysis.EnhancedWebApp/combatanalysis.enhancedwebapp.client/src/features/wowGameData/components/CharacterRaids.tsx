import { useContext, useEffect, useState } from 'react';
import { useLazyGetCharacterRaidsQuery, useLazyGetCharacterDungeonsQuery } from '../api/WoWCharacter.api';
import type { CharacterDungeonModel } from '../types/dungeon/CharacterDungeonModel';
import WoWGameDataContext from '@/context/WoWGameDataContext';

const CharacterRaids: React.FC<{ isRaids: boolean }> = ({ isRaids }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [dungeons, setDungeons] = useState<CharacterDungeonModel | null>(null);

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
                <h6>{t("Raids")}</h6>
            </div>
            <ul className="raids__container">
                {dungeons.expansions.map((expansion, index) => (
                    <li key={index} className="raids__item">
                        <div>{expansion.expansion.name}</div>
                        <ul className="current-period">
                            {expansion.instances.map((instance, index1) => (
                                <li key={`${index}-${index1}`}>
                                    <div>{instance.instance.name}</div>
                                    <ul className="current-period">
                                        {instance.modes.map((mode, index2) => (
                                            <li key={`${index}-${index1}-${index2}`}>
                                                <div>{mode.difficulty.name}</div>
                                                <div>{mode.status.name}</div>
                                                <div>{mode.progress.completedCount}</div>
                                                <div>{mode.progress.encounters[0].lastKillTime}</div>
                                            </li>
                                        ))
                                        }
                                    </ul>
                                </li>
                            ))
                            }
                        </ul>
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default CharacterRaids;