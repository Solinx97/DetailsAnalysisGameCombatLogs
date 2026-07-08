import { memo, useEffect, useState } from 'react';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import DetailsItem from './DetailsItem';
import Select from 'react-select';
import CombatPreAuraItem from '../auras/CombatPreAuraItem';

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

    const compare = (playerA: CombatPlayerModel, playerB: CombatPlayerModel) => {
        const keys: (keyof CombatPlayerModel)[] = ['damageDone', 'healDone', 'damageTaken', 'resourcesRecovery'];
        const key = keys[sortingValue === null ? 0 : sortingValue.value];

        if (playerA[key] > playerB[key]) {
            return -1;
        }
        if (playerA[key] < playerB[key]) {
            return 1;
        }

        return 0;
    }

    const filter = () => {
        if (sortingValue === undefined || sortingValue === null) {
            return;
        }

        setFilteredCombatPlayers([...combatPlayers].sort(compare));
    }

    if (filteredCombatPlayers.length === 0) {
        return (<div>Loading...</div>);
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
                        <div className="card-body">
                            <h5 className="card-title">{combatPlayer.player.username}</h5>
                        </div>
                        <CombatPreAuraItem
                            combatPlayerId={combatPlayer.id}
                            combatId={combatPlayer.combatId}
                        />
                        <DetailsItem
                            player={combatPlayer}
                            details={details}
                            getValueShortName={getValueShortName}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default memo(Details);