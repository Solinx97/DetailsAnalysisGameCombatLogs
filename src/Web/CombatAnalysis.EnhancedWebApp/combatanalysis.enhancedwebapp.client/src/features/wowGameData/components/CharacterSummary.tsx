import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetCharacterSummaryQuery } from '../api/BattleNetData.api';
import type { WoWCharacterModel } from '../types/character/WoWCharacterModel';

const CharacterSummary: React.FC<{ username: string | undefined }> = ({ username }) => {
    const { t } = useTranslation('wowGameData');

    const [characterSummary, setCharacterSummary] = useState<WoWCharacterModel | null>(null);

    const [getSummary] = useLazyGetCharacterSummaryQuery();

    useEffect(() => {
        if (!username || username.trim().length === 0) {
            return;
        }

        const loadAsync = async () => {
            try {
                const receivedSummary = await getSummary({ username, serverName: "howling-fjord", regionName: "eu" }).unwrap();
                setCharacterSummary(receivedSummary);
            } catch (error) {
                console.error("Failed to fetch character summary:", error);
            }
        }

        loadAsync();
    }, []);

    if (!username || username.trim().length === 0) {
        return (<div>No data</div>);
    }

    if (!characterSummary) {
        return (<div>Loading...</div>);
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
                    <div className="item">{characterSummary.activeTitle.name}</div>
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