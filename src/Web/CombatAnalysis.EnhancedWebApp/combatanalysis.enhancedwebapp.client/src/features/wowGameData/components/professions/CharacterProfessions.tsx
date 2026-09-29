import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faLocationCrosshairs } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useState } from 'react';
import { useGetProfessionsQuery } from '../../api/WoWCharacter.api';
import CharacterProfessionsTiers from './CharacterProfessionsTiers';

import './Professions.scss';

const CharacterProfessions: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [selectedTierId, setSelectedTierId] = useState<number>(0);

    const { data: professions, isLoading, error } = useGetProfessionsQuery({ username, serverName, regionName });

    if (!professions || isLoading || error) {
        return (<ResponseInformation
            error={error}
            isLoading={!professions || isLoading}
        />);
    }

    return (
        <div className="character-professions">
            <div className="character-professions__title">
                <h6>{t("Professions")}</h6>
            </div>
            <h6>{t("Primaries")}:</h6>
            <ul className="character-professions__professions">
                {professions.primaries.map(profession => (
                    <li key={profession.profession.id}>
                        <div className="profession">
                            <div className="btn-shadow"
                                onClick={() => setSelectedTierId(prev => prev === profession.profession.id ? 0 : profession.profession.id)}>
                                <FontAwesomeIcon
                                    icon={faLocationCrosshairs}
                                />
                                <div>{profession.profession.name}</div>
                            </div>
                        </div>
                        {selectedTierId === profession.profession.id &&
                            <CharacterProfessionsTiers
                                tiers={profession.tiers}
                            />
                        }
                    </li>
                ))
                }
            </ul>
            <h6>{t("Secondaries")}:</h6>
            <ul className="character-professions__professions">
                {professions.secondaries.map(profession => (
                    <li key={profession.profession.id}>
                        <div className="profession">
                            <div className="btn-shadow"
                                onClick={() => setSelectedTierId(prev => prev === profession.profession.id ? 0 : profession.profession.id)}>
                                <FontAwesomeIcon
                                    icon={faLocationCrosshairs}
                                />
                                <div>{profession.profession.name}</div>
                            </div>
                        </div>
                        {selectedTierId === profession.profession.id &&
                            <CharacterProfessionsTiers
                                tiers={profession.tiers}
                            />
                        }
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default CharacterProfessions;