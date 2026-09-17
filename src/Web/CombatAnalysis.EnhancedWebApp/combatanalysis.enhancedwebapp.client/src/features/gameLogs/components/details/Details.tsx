import Loading from '@/shared/components/Loading';
import { faArrowsToEye } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState } from 'react';
import Select from 'react-select';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import type { UnitInfoModel } from '../../types/UnitInfoModel';
import CombatPreAuraItem from '../auras/CombatPreAuraItem';
import DetailsItem from './DetailsItem';
import PlayerParams from './PlayerParams';

import '../auras/CombatAuras.scss';

interface DetailsProps {
    details: CombatDetailsModel;
    combatPlayers: CombatPlayerModel[];
    getValueShortName(value: number): string;
    t(key: string): string;
}

type Option = {
    value: number;
    label: string;
}

const Details: React.FC<DetailsProps> = ({ details, combatPlayers, getValueShortName, t }) => {
    const [filteredCombatPlayers, setFilteredCombatPlayers] = useState<CombatPlayerModel[]>(combatPlayers);
    const [playerStatsCombatPlayerId, setPlayerStatsCombatPlayerId] = useState(0);

    const sortOptions: Option[] = [
        { value: 0, label: t("Damage") },
        { value: 1, label: t("Healing") },
        { value: 2, label: t("DamageTaken") },
        { value: 3, label: t("ResourcesRecovery") },
    ];
    const [sortingValue, setSortingValue] = useState<Option | null>(sortOptions[0]);

    useEffect(() => {
        filter();
    }, [sortingValue, combatPlayers]);

    const compare = (unitA: UnitInfoModel, unitB: UnitInfoModel) => {
        const keys: (keyof UnitInfoModel)[] = ['damageDone', 'healDone', 'damageTaken', 'resourcesRecovery'];
        const key = keys[sortingValue === null ? 0 : sortingValue.value];

        if (unitA[key] > unitB[key]) {
            return -1;
        }
        else if (unitA[key] < unitB[key]) {
            return 1;
        }

        return 0;
    }

    const filter = () => {
        if (sortingValue === undefined || sortingValue === null) {
            return;
        }

        setFilteredCombatPlayers(
            [...combatPlayers]
                .sort((a, b) => compare(a.unit?.unitInfo, b.unit?.unitInfo))
        );
    }

    if (filteredCombatPlayers.length === 0) {
        return (<Loading />);
    }

    return (
        <div className="details">
            <div className="details__filter">
                <div>{t("Sorting")}</div>
                <Select<Option>
                    className="options"
                    options={sortOptions}
                    value={sortingValue}
                    onChange={(selected) => setSortingValue(selected)}
                />
            </div>
            <ul className="details__content">
                {filteredCombatPlayers?.map((combatPlayer) => (
                    <li key={combatPlayer.id} className="card">
                        <div className="card-body card-title">
                            <h5>{combatPlayer.player.username}</h5>
                            <div className="btn-shadow"
                                onClick={() => setPlayerStatsCombatPlayerId(combatPlayer.id)}>
                                <FontAwesomeIcon
                                    icon={faArrowsToEye}
                                />
                                <div>{t("Stats")}</div>
                            </div>
                        </div>
                        <CombatPreAuraItem
                            unitId={combatPlayer.unitId}
                            combatId={details.id}
                        />
                        <DetailsItem
                            avgilvl={combatPlayer.averageItemLevel}
                            playerId={combatPlayer.id}
                            unitInfo={combatPlayer.unit.unitInfo}
                            details={details}
                            getValueShortName={getValueShortName}
                            deathCount={combatPlayer.deathCount}
                        />
                        {playerStatsCombatPlayerId === combatPlayer.id &&
                            <PlayerParams
                                t={t}
                                combatPlayerId={combatPlayer.id}
                                gameVersion={details.gameVersion}
                                setPlayerStatsCombatPlayerId={setPlayerStatsCombatPlayerId}
                            />
                        }
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default memo(Details);