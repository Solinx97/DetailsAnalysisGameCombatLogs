import WoWGameDataContext from '@/context/WoWGameDataContext';
import ResponseInformation from '@/shared/components/ResponseInformation';
import { useContext, useEffect, useState } from 'react';
import { useGetCharacterStatsQuery } from '../../api/WoWCharacter.api';

import './CharacterEquipments.scss';

const CharacterStats: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t, username, serverName, regionName } = context;

    const [isSkipRequest, setIsSkipRequest] = useState<boolean>(false);

    const { data: characterStats, isLoading, error } = useGetCharacterStatsQuery({ username, serverName, regionName },
        {
            skip: isSkipRequest
        }
    );

    useEffect(() => {
        setIsSkipRequest([username, serverName].filter(x => x.trim().length > 0).length < [username, serverName].length);
    }, [username, serverName]);

    if (!characterStats || isLoading || isSkipRequest || error) {
        return (<ResponseInformation
            error={error}
            watchParams={[username, serverName]}
            isLoading={!characterStats || isLoading}
        />);
    }

    return (
        <div className="character-stats">
            <div>{t("MainStats")}</div>
            <ul className="character-stats__container">
                <li className="character-stats__power">
                    <div className="special">{t("Strength")}</div>
                    <div>{characterStats.strength.effective}</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Intellect")}</div>
                    <div>{characterStats.intellect.effective}</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Agility")}</div>
                    <div>{characterStats.agility.effective}</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Stamina")}</div>
                    <div>{characterStats.stamina.effective}</div>
                    <div>({characterStats.health})</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Power")}</div>
                    <div>{characterStats.power}</div>
                    <div className="power-type">({characterStats.powerType.name})</div>
                </li>
            </ul>
            <div>{t("Additional")}</div>
            <ul className="character-stats__container">
                {characterStats.attackPower > characterStats.spellPower
                    ? <li className="character-stats__power">
                        <div className="stat-name special">{t("AttackPower")}</div>
                        <div>{characterStats.attackPower}</div>
                    </li>
                    : <li className="character-stats__power">
                        <div className="stat-name special">{t("SpellPower")}</div>
                        <div>{characterStats.spellPower}</div>
                    </li>
                }
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Crit")}</div>
                    <div>{characterStats.spellCrit.value?.toFixed(2)}%</div>
                    <div>({characterStats.spellCrit.ratingNormalized})</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Haste")}</div>
                    <div>{characterStats.spellHaste.value?.toFixed(2)}%</div>
                    <div>({characterStats.spellHaste.ratingNormalized})</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Mastery")}</div>
                    <div>{characterStats.mastery.value?.toFixed(2)}%</div>
                    <div>({characterStats.mastery.ratingNormalized})</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Versatility")}</div>
                    <div>{characterStats.versatilityDamageDoneBonus.toFixed(2)}%/{characterStats.versatilityDamageTakenBonus.toFixed(2)}%</div>
                    <div>({characterStats.versatility})</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Lifesteal")}</div>
                    <div>{characterStats.lifesteal.ratingBonus.toFixed(2)}%</div>
                    <div>({characterStats.lifesteal.ratingNormalized})</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Avoidance")}</div>
                    <div>{characterStats.avoidance.ratingBonus.toFixed(2)}%</div>
                    <div>({characterStats.avoidance.ratingNormalized})</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Movement")}</div>
                    <div>{characterStats.speed.ratingBonus.toFixed(2)}%</div>
                    <div>({characterStats.speed.ratingNormalized})</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name">{t("ManaRegen")}</div>
                    <div>{characterStats.manaRegen}/{characterStats.manaRegenCombat}</div>
                </li>
            </ul>
            <div>{t("Deffensive")}</div>
            <ul className="character-stats__container">
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Armor")}</div>
                    <div>{characterStats.armor.effective}</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Dodge")}</div>
                    <div>{characterStats.dodge.value?.toFixed(2)}%</div>
                    <div>({characterStats.dodge.ratingNormalized})</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Parry")}</div>
                    <div>{characterStats.parry.value?.toFixed(2)}%</div>
                    <div>({characterStats.parry.ratingNormalized})</div>
                </li>
                <li className="character-stats__power">
                    <div className="stat-name special">{t("Block")}</div>
                    <div>{characterStats.block.value?.toFixed(2)}%</div>
                    <div>({characterStats.block.ratingNormalized})</div>
                </li>
            </ul>
        </div>
    );
}

export default CharacterStats;