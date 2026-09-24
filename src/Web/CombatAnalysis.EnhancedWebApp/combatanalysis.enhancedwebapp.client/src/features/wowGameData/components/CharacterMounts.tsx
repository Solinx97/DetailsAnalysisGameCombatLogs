import { NoneValue } from '@/shared/helpers/ConstHelpers';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetUserMountsQuery } from '../api/BattleNetData.api';
import type { CharacterMountModel } from '../types/CharacterMountModel';

import './CharacterReputations.scss';

const CharacterMounts: React.FC<{ username: string | undefined }> = ({ username }) => {
    const { t } = useTranslation('wowGameData');

    const [mounts, setMounts] = useState<CharacterMountModel[]>([]);

    // const [getMounts] = useLazyGetCharacterMountsQuery();
    const [getMounts] = useLazyGetUserMountsQuery();

    useEffect(() => {
        const getReputationsAsync = async () => {
            try {
                const receivedMounts = await getMounts({ regionName: "eu" }).unwrap();
                setMounts(receivedMounts);
            } catch (error) {
                console.error("Failed to fetch character mounts:", error);
            }
        }

        getReputationsAsync();
    }, []);

    if (!mounts || mounts.length === 0) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="reputations">
            <div className="reputations__title">
                <h6>{t("Mounts")}:</h6>
                <h6 className="count">{mounts.length}</h6>
            </div>
            <ul className="reputations__container">
                {mounts.map((mount, index) => (
                    <li key={index} className="reputations__reputation">
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