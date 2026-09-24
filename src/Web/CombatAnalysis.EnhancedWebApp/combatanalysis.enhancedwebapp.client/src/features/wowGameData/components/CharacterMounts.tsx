import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetCharacterMountsQuery } from '../api/BattleNetData.api';
import type { CharacterMountModel } from '../types/CharacterMountModel';

const CharacterMounts: React.FC<{ username: string | undefined }> = ({ username }) => {
    const { t } = useTranslation('wowGameData');

    const [mounts, setMounts] = useState<CharacterMountModel[]>([]);

    const [getMounts] = useLazyGetCharacterMountsQuery();

    useEffect(() => {
        if (!username || username.trim().length === 0) {
            return;
        }

        const loadAsync = async () => {
            try {
                const receivedMounts = await getMounts({ username, serverName: "howling-fjord", regionName: "eu" }).unwrap();
                setMounts(receivedMounts);
            } catch (error) {
                console.error("Failed to fetch character mounts:", error);
            }
        }

        loadAsync();
    }, []);

    if (!username || username.trim().length === 0) {
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