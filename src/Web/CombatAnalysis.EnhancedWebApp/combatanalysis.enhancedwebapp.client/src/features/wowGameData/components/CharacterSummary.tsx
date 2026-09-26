import WoWGameDataContext from '@/context/WoWGameDataContext';
import { useContext, useEffect, useState } from 'react';
import { useGetCharacterSummaryQuery } from '../api/WoWCharacter.api';
import ResponseInformation from '@/shared/components/ResponseInformation';

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
            <div className="summary__title">
                <h6>{t("Summary")}</h6>
            </div>
            <ul className="summary__container">
                <li className="summary__item">
                    <div className="item">{t("Username")}</div>
                    <div className="item">{characterSummary.name}</div>
                    <div className="item">{characterSummary.activeTitle?.name}</div>
                </li>
                <li className="summary__item">
                    <div className="item">{t("Gender")}</div>
                    <div className="item">{characterSummary.gender.name}</div>
                </li>
                <li className="summary__item">
                    <div className="item">{t("Race")}</div>
                    <div className="item">{characterSummary.race.name}</div>
                </li>
                <li className="summary__item">
                    <div className="item">{t("Class")}</div>
                    <div className="item">{characterSummary.class.name}</div>
                </li>
                <li className="summary__item">
                    <div className="item">{t("ActiveSpec")}</div>
                    <div className="item">{characterSummary.activeSpec.name}</div>
                </li>
                <li className="summary__item">
                    <div className="item">{t("Realm")}</div>
                    <div className="item">{characterSummary.realm.name}</div>
                </li>
                <li className="summary__item">
                    <div className="item">{t("Guild")}</div>
                    <div className="item">{characterSummary.guild?.name}</div>
                </li>
                <li className="summary__item">
                    <div className="item">{t("Level")}</div>
                    <div className="item">{characterSummary.level}</div>
                </li>
                <li className="summary__item">
                    <div className="item">{t("AchievementPoints")}</div>
                    <div className="item">{characterSummary.achievementPoints}</div>
                </li>
                <li className="summary__item">
                    <div className="item">{t("EquippedItemLevel")}</div>
                    <div className="item">{characterSummary.equippedItemLevel}</div>
                </li>
            </ul>
        </div>
    );
}

export default CharacterSummary;