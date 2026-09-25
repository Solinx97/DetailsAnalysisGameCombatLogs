import { useContext, useEffect, useState } from 'react';
import { useLazyGetCharacterMythicKeystoneQuery } from '../api/WoWCharacter.api';
import type { MythicKeystoneModel } from '../types/mythicKeystone/MythicKeystoneModel';
import WoWGameDataContext from '@/context/WoWGameDataContext';
import { faDashboard, faKey } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

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
                <div className="btn-shadow">
                    <FontAwesomeIcon
                        icon={faDashboard}
                    />
                    <div>{mythicKeystone.currentMythicRating.rating.toFixed(2)}</div>
                </div>
            </div>
            <ul className="current-period">
                {mythicKeystone.currentPeriod.bestRuns.map((run, index) => (
                    <li key={index} className="run">
                        <div className="level">
                            <div className="btn-shadow">
                                <FontAwesomeIcon
                                    icon={faKey}
                                />
                                <div>{run.level}</div>
                            </div>
                            <div>{run.mythicRating.rating.toFixed(2)}</div>
                        </div>
                        <div>{run.dungeon.name}</div>
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default CharacterMythicKeystone;