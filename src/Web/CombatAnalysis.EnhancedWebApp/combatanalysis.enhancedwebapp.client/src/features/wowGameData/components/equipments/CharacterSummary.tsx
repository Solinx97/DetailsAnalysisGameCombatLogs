import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { useContext, useEffect, useState } from 'react';
import { useGetCharacterSummaryQuery } from '../../api/WoWCharacter.api';

const CharacterSummary: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [isSkipRequest, setIsSkipRequest] = useState<boolean>(false);;

    const { data: characterSummary, isLoading, error } = useGetCharacterSummaryQuery({ username, serverName, regionName },
        {
            skip: isSkipRequest
        });

    useEffect(() => {
        setIsSkipRequest([username, serverName].filter(x => x.trim().length > 0).length < [username, serverName].length);
    }, [username, serverName]);

    if (!characterSummary || isLoading || isSkipRequest || error) {
        return (<ResponseInformation
            error={error}
            watchParams={[username, serverName]}
            isLoading={!characterSummary || isLoading}
        />);
    }

    return (
        <div className="summary">
            <ul className="summary__container">
                <li className="summary__item special">
                    <div className="item">{characterSummary.gender.name}</div>
                </li>
                <li className="summary__item special">
                    <div className="item">{characterSummary.race.name}</div>
                </li>
                <li className="summary__item special">
                    <div className="item">{characterSummary.class.name}</div>
                </li>
                <li className="summary__item special">
                    <div className="item">{characterSummary.activeSpec.name}</div>
                </li>
            </ul>
            {characterSummary.guild &&
                <ul className="summary__container">
                    <li className="summary__item special category">
                        <div className="item">{t("Guild")}</div>
                    </li>
                    <li className="summary__item special">
                        <div className="item">{characterSummary.guild.name}</div>
                    </li>
                    <li className="summary__item special">
                        <div className="item">{characterSummary.guild.faction.name}</div>
                    </li>
                    <li className="summary__item special">
                        <div className="item">{characterSummary.guild.realm.name}</div>
                    </li>
                </ul>
            }
            <ul className="summary__container">
                <li className="summary__item special category">
                    <div className="item">{t("AchievementPoints")}</div>
                </li>
                <li className="summary__item special">
                    <div className="item">{characterSummary.achievementPoints}</div>
                </li>
                <li className="summary__item special category">
                    <div className="item">{t("EquippedItemLevel")}</div>
                </li>
                <li className="summary__item special">
                    <div className="item">{characterSummary.equippedItemLevel}</div>
                </li>
            </ul>
            <ul className="summary__container">
                <li className="summary__item username">{characterSummary.name}{characterSummary.activeTitle ? ` (${characterSummary.activeTitle?.name})` : ''}</li>
                <li className="summary__item special">{characterSummary.realm.name} </li>
                <li className="summary__item special">{characterSummary.level}</li>
            </ul>
        </div>
    );
}

export default CharacterSummary;