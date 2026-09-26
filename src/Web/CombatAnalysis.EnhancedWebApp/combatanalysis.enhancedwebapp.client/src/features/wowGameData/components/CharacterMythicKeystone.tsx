import { useContext, useEffect, useState } from 'react';
import { useGetCharacterMythicKeystoneQuery } from '../api/WoWCharacter.api';
import WoWGameDataContext from '@/context/WoWGameDataContext';
import { faDashboard, faKey } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import ResponseInformation from '@/shared/components/ResponseInformation';

const CharacterMythicKeystone: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [isSkipRequest, setIsSkipRequest] = useState<boolean>(false);

    const { data: mythicKeystone, isLoading, error } = useGetCharacterMythicKeystoneQuery({ username, serverName, regionName },
            {
                skip: isSkipRequest
            });

    useEffect(() => {
        setIsSkipRequest([username, serverName].filter(x => x.trim().length > 0).length < [username, serverName].length);
    }, [username, serverName]);

    if (!mythicKeystone || isLoading || isSkipRequest || error) {
        return (<ResponseInformation
            error={error}
            watchParams={[username, serverName]}
            isLoading={!mythicKeystone || isLoading}
        />);
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