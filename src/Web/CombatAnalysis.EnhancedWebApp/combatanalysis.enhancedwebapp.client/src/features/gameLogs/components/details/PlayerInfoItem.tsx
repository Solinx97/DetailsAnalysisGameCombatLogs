import { memo } from 'react';
import { useTranslation } from 'react-i18next';
import type { WoWMoPClassicPlayerStatsModel } from '../../types/wowMoPClassic/WoWMoPClassicPlayerStatsModel';
import type { WoWMidnightPlayerStatsModel } from '../../types/woWMidnight/WoWMidnightPlayerStatsModel';

const PlayerInfoItem: React.FC<{ stats: WoWMoPClassicPlayerStatsModel | WoWMidnightPlayerStatsModel }> = ({ stats: stats }) => {
    const { t } = useTranslation('childs/playerInformation');

    const getWoWMoPClasicStats = (stats: WoWMoPClassicPlayerStatsModel) => {
        return (
            <>
                <li className="list-group-item">
                    <div>{t("Spirit")}</div>
                    <div>{stats.spirit}</div>
                </li>
                <li className="list-group-item">
                    <div>{t("Hit")}</div>
                    <div>{stats.hit}</div>
                </li>
                <li className="list-group-item">
                    <div>{t("Expertise")}</div>
                    <div>{stats.expertise}</div>
                </li>
            </>
        );
    }

    const getWoWMidnightStats = (stats: WoWMidnightPlayerStatsModel) => {
        return (
            <>
                <li className="list-group-item">
                    <div>{t("Mastery")}</div>
                    <div>{stats.mastery}</div>
                </li>
                <li className="list-group-item">
                    <div>{t("Versality")}</div>
                    <div>{stats.versality}</div>
                </li>
                <li className="list-group-item">
                    <div>{t("Lifesteal")}</div>
                    <div>{stats.lifesteal}</div>
                </li>
                <li className="list-group-item">
                    <div>{t("Avoidance")}</div>
                    <div>{stats.avoidance}</div>
                </li>
                <li className="list-group-item">
                    <div>{t("Movement")}</div>
                    <div>{stats.movement}</div>
                </li>
            </>
        );
    }

    return (
        <ul className="player-info__stats">
            <li className="category">
                <div className="title">{t("MainStats")}</div>
                <ul className="content">
                    <li className="list-group-item">
                        <div>{t("Strength")}</div>
                        <div>{stats.strength}</div>
                    </li>
                    <li className="list-group-item">
                        <div>{t("Agility")}</div>
                        <div>{stats.agility}</div>
                    </li>
                    <li className="list-group-item">
                        <div>{t("Intelligence")}</div>
                        <div>{stats.intelligence}</div>
                    </li>
                    <li className="list-group-item">
                        <div>{t("Stamina")}</div>
                        <div>{stats.stamina}</div>
                    </li>
                </ul>
            </li>
            <li className="category">
                <div className="title">{t("SecondStats")}</div>
                <ul className="content">
                    <li className="list-group-item">
                        <div>{t("Crit")}</div>
                        <div>{stats.crit}</div>
                    </li>
                    <li className="list-group-item">
                        <div>{t("Haste")}</div>
                        <div>{stats.haste}</div>
                    </li>
                    <li className="list-group-item">
                        <div>{t("Armor")}</div>
                        <div>{stats.armor}</div>
                    </li>
                    <li className="list-group-item">
                        <div>{t("Dodge")}</div>
                        <div>{stats.dodge}</div>
                    </li>
                    <li className="list-group-item">
                        <div>{t("Parry")}</div>
                        <div>{stats.parry}</div>
                    </li>
                    <li className="list-group-item">
                        <div>{t("Block")}</div>
                        <div>{stats.block}</div>
                    </li>
                </ul>
            </li>
            <li className="category">
                <div className="title">{t("OtherStats")}</div>
                <ul className="content">
                    {'spirit' in stats
                        ? getWoWMoPClasicStats(stats)
                        : getWoWMidnightStats(stats)
                    }
                </ul>
            </li>
            <li className="category">
                <div className="title">{t("More")}</div>
                <ul className="content">
                    <li className="list-group-item">
                        <div>{t("Talents")}</div>
                        <div>{stats.talents}</div>
                    </li>
                </ul>
            </li>

        </ul>
    );
}

export default memo(PlayerInfoItem);