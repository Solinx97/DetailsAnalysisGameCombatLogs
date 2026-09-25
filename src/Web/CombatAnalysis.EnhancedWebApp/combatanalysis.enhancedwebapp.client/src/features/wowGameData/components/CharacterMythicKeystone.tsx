import { useContext, useEffect, useState } from 'react';
import { useLazyGetCharacterMythicKeystoneQuery } from '../api/WoWCharacter.api';
import type { MythicKeystoneModel } from '../types/mythicKeystone/MythicKeystoneModel';
import WoWGameDataContext from '@/context/WoWGameDataContext';

const CharacterMythicKeystone: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [mythicKeystone, setMythicKeystone] = useState<MythicKeystoneModel | null>(null);

    const [getMythicKeystone] = useLazyGetCharacterMythicKeystoneQuery();

    useEffect(() => {
        if (!username || username.trim().length === 0) {
            return;
        }

        const loadAsync = async () => {
            try {
                const receivedKeyStone = await getMythicKeystone({ username, serverName, regionName }).unwrap();
                setMythicKeystone(receivedKeyStone);
            } catch (error) {
                console.error("Failed to fetch character mythic stone:", error);
            }
        }

        loadAsync();
    }, []);

    if (!username || username.trim().length === 0) {
        return (<div>No data</div>);
    }
    
    if (!mythicKeystone) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="mythic-keystone">
            <div className="mythic-keystone__title">
                <h6>{t("MythicKeystone")}</h6>
            </div>
            <ul className="mythic-keystone__container">
                <li className="mythic-keystone__item">
                    <div className="item">{t("CurrentPeriod")}</div>
                    <ul className="current-period">
                        {mythicKeystone.currentPeriod.bestRuns.map((run, index) => (
                            <li key={index}>
                                <div>{run.level}</div>
                                <div>{run.dungeon.name}</div>
                                <div>{run.mythicRating.rating}</div>
                            </li>
                        ))
                        }
                    </ul>
                </li>
                <li className="mythic-keystone__item">
                    <div className="item">{t("CurrentRating")}</div>
                    <div className="item">{mythicKeystone.currentMythicRating.rating}</div>
                </li>
            </ul>
        </div>
    );
}

export default CharacterMythicKeystone;