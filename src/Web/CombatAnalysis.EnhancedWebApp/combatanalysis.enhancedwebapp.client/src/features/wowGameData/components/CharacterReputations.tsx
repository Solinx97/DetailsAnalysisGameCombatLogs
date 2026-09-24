import { NoneValue } from '@/shared/helpers/ConstHelpers';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetCharacterReputationsQuery } from '../api/BattleNetData.api';
import type { CharacterReputationModel } from '../types/CharacterReputationModel';

import './CharacterReputations.scss';

const CharacterReputations: React.FC<{ username: string | undefined }> = ({ username }) => {
    const { t } = useTranslation('wowGameData');

    const [reputaions, setReputaions] = useState<CharacterReputationModel[]>([]);

    const [getReputations] = useLazyGetCharacterReputationsQuery();

    useEffect(() => {
        const getReputationsAsync = async () => {
            try {
                const receivedReputations = await getReputations({ username: username ? username : NoneValue.NONE_VALUE, serverName: "howling-fjord", regionName: "eu" }).unwrap();
                setReputaions(receivedReputations);
            } catch (error) {
                console.error("Failed to fetch character reputations:", error);
            }
        }

        getReputationsAsync();
    }, []);

    if (reputaions.length === 0) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="reputations">
            <div className="reputations__title">
                <h6>{t("Reputations")}:</h6>
                <h6 className="count">{reputaions.length}</h6>
            </div>
            <ul className="reputations__container">
                {reputaions.map((rep, index) => (
                    <li key={index} className="reputations__reputation">
                        <div className="item">{rep.faction.name}</div>
                        <div className="item">
                            <div className="reputation-value">
                                <div>{rep.standing.value}</div>
                                <div>/</div>
                                <div>{rep.standing.max}</div>
                            </div>
                        </div>
                        <div className="item">{rep.standing.name}</div>
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default CharacterReputations;