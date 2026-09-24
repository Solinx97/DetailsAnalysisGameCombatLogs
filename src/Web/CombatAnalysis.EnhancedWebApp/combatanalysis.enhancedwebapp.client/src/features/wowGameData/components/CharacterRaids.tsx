import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetCharacterRaidsQuery, useLazyGetCharacterDungeonsQuery } from '../api/BattleNetData.api';
import type { CharacterDungeonModel } from '../types/dungeon/CharacterDungeonModel';

const CharacterRaids: React.FC<{ username: string | undefined, isRaids: boolean }> = ({ username, isRaids }) => {
    const { t } = useTranslation('wowGameData');

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
                const receivedDungeons = await getDungeons({ username, serverName: "howling-fjord", regionName: "eu" }).unwrap();
                setDungeons(receivedDungeons);
            } catch (error) {
                console.error("Failed to fetch character dungeons:", error);
            }
        }

        loadAsync();
    }, []);

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