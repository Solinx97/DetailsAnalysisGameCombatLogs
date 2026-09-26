import { useContext, useEffect, useState } from 'react';
import { useGetCharacterReputationsQuery } from '../api/WoWCharacter.api';
import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';

const CharacterReputations: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [isSkipRequest, setIsSkipRequest] = useState<boolean>(false);

    const { data: reputaions, isLoading, error } = useGetCharacterReputationsQuery({ username, serverName, regionName },
            {
                skip: isSkipRequest
            });

    useEffect(() => {
        setIsSkipRequest([username, serverName].filter(x => x.trim().length > 0).length < [username, serverName].length);
    }, [username, serverName]);

    if (!reputaions || isLoading || isSkipRequest || error) {
        return (<ResponseInformation
            error={error}
            watchParams={[username, serverName]}
            isLoading={!reputaions || isLoading}
        />);
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