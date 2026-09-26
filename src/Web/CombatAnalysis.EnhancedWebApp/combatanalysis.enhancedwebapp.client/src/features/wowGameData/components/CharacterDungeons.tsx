import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { faLocationCrosshairs, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useState } from 'react';
import { useGetCharacterDungeonsQuery, useGetCharacterRaidsQuery } from '../api/WoWCharacter.api';
import SelectedDungeonsExpantion from './SelectedDungeonsExpantion';

const CharacterDungeons: React.FC<{ isRaids: boolean }> = ({ isRaids }) => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [selectedExpansionId, setSelectedExpantionId] = useState<number>(0);
    const [onlyCurrentWeekCompleted, setCurrentWeekCompleted] = useState<boolean>(false);
    const [isSkipRequest, setIsSkipRequest] = useState<boolean>(false);

    const { data: dungeons, isLoading, error } = isRaids
        ? useGetCharacterRaidsQuery({ username, serverName, regionName },
            {
                skip: isSkipRequest
            }
        )
        : useGetCharacterDungeonsQuery({ username, serverName, regionName },
            {
                skip: isSkipRequest
            }
        );

    useEffect(() => {
        setIsSkipRequest([username, serverName].filter(x => x.trim().length > 0).length < [username, serverName].length);
    }, [username, serverName]);

    if (!dungeons || isLoading || isSkipRequest || error) {
        return (<ResponseInformation
            error={error}
            watchParams={[username, serverName]}
            isLoading={!dungeons || isLoading}
        />);
    }

    return (
        <div className="raids">
            <div className="raids__title">
                <h6>{isRaids ? t("Raids") : t("Dungeons")}</h6>
            </div>
            <div className="form-check">
                <input className="form-check-input" type="checkbox" value="" id="checkIndeterminate"
                    defaultChecked={onlyCurrentWeekCompleted}
                    onChange={() => setCurrentWeekCompleted(prev => !prev)} />
                <label className="form-check-label" htmlFor="checkIndeterminate">{t("OnlyCurrentWeekCompleted")}</label>
            </div>
            <ul className="raids__container">
                {dungeons.expansions.map((expansion, index) => (
                    <li key={index} className="raids__item">
                        <div className={`btn-shadow ${selectedExpansionId === expansion.expansion.id ? 'selected' : ''}`}
                            onClick={() => setSelectedExpantionId(prev => prev === 0 ? expansion.expansion.id : 0)}>
                            <FontAwesomeIcon
                                icon={selectedExpansionId === expansion.expansion.id ? faLocationCrosshairs : faPlus}
                            />
                            <div>{expansion.expansion.name}</div>
                        </div>
                        {selectedExpansionId === expansion.expansion.id &&
                            <SelectedDungeonsExpantion
                                instances={expansion.instances}
                                onlyCurrentWeekCompleted={onlyCurrentWeekCompleted}
                            />
                        }
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default CharacterDungeons;