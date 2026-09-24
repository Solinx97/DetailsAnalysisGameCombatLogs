import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetCharacterMythicKeystoneQuery } from '../api/BattleNetData.api';
import type { MythicKeystoneModel } from '../types/mythicKeystone/MythicKeystoneModel';

const CharacterMythicKeystone: React.FC<{ username: string | undefined }> = ({ username }) => {
    const { t } = useTranslation('wowGameData');

    const [mythicKeystone, setMythicKeystone] = useState<MythicKeystoneModel | null>(null);

    const [getMythicKeystone] = useLazyGetCharacterMythicKeystoneQuery();

    useEffect(() => {
        if (!username || username.trim().length === 0) {
            return;
        }

        const loadAsync = async () => {
            try {
                const receivedKeyStone = await getMythicKeystone({ username, serverName: "howling-fjord", regionName: "eu" }).unwrap();
                setMythicKeystone(receivedKeyStone);
            } catch (error) {
                console.error("Failed to fetch character mythic stone:", error);
            }
        }

        loadAsync();
    }, []);

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