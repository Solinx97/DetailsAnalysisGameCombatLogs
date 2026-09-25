import { useContext, useEffect, useState } from 'react';
import { useLazyGetCharacterReputationsQuery } from '../api/WoWCharacter.api';
import type { CharacterReputationModel } from '../types/CharacterReputationModel';
import WoWGameDataContext from '@/context/WoWGameDataContext';

const CharacterReputations: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [reputaions, setReputaions] = useState<CharacterReputationModel[]>([]);

    const [getReputations] = useLazyGetCharacterReputationsQuery();

    useEffect(() => {
        if (!username || username.trim().length === 0) {
            return;
        }

        const loadAsync = async () => {
            try {
                const receivedReputations = await getReputations({ username, serverName, regionName }).unwrap();
                setReputaions(receivedReputations);
            } catch (error) {
                console.error("Failed to fetch character reputations:", error);
            }
        }

        loadAsync();
    }, []);

    if (!username || username.trim().length === 0) {
        return (<div>No data</div>);
    }

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
                    <li key={index} className="reputations__item">
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