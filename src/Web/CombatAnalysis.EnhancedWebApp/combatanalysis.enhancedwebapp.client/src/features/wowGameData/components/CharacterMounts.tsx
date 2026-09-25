import { useContext, useEffect, useState } from 'react';
import { useLazyGetCharacterMountsQuery } from '../api/WoWCharacter.api';
import type { CharacterMountModel } from '../types/CharacterMountModel';
import WoWGameDataContext from '@/context/WoWGameDataContext';

const CharacterMounts: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [mounts, setMounts] = useState<CharacterMountModel[]>([]);
    const [hasError, setHasError] = useState<boolean>(false);

    const [getMounts] = useLazyGetCharacterMountsQuery();

    useEffect(() => {
        if (!username || username.trim().length === 0) {
            return;
        }

        const loadAsync = async () => {
            try {
                const receivedMounts = await getMounts({ username, serverName, regionName }).unwrap();
                setMounts(receivedMounts);
            } catch (error) {
                console.error("Failed to fetch character mounts:", error);
                setHasError(true);
            }
        }

        loadAsync();
    }, []);

    if (!username || username.trim().length === 0 || hasError) {
        return (<div>No data</div>);
    }

    if (!mounts || mounts.length === 0) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="mounts">
            <div className="mounts__title">
                <h6>{t("Mounts")}:</h6>
                <h6 className="count">{mounts.length}</h6>
            </div>
            <ul className="mounts__container">
                {mounts.map((mount, index) => (
                    <li key={index} className="mounts__item">
                        <div className="item">{mount.mount.name}</div>
                        {mount.isUsable &&
                            <div className="item">Usable</div>
                        }
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default CharacterMounts;