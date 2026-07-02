import { faFlask, faHourglass, faAppleWhole } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState } from 'react';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import type { CombatDetailsModel } from '../../types/CombatDetailsModel';
import DetailsItem from './DetailsItem';
import { useLazyGetCombatAbilitiesQuery } from '../../api/GameLogs.api';
import type { CombatAbilityModel } from '../../types/CombatAbilityModel';
import Select from 'react-select';

interface DetailsProps {
    combatPlayers: CombatPlayerModel[];
    details: CombatDetailsModel;
    getValueShortName(value: number): string;
    t(key: string): string;
}

type Option = {
    value: number;
    label: string;
}

const Details: React.FC<DetailsProps> = ({ combatPlayers, details, getValueShortName, t }) => {
    const [filteredCombatPlayers, setFilteredCombatPlayers] = useState<CombatPlayerModel[]>(combatPlayers);
    const [allAbilities, setAllAbilities] = useState<Map<number, CombatAbilityModel[]>>();
    const [abilityVisisble, setAbilityVisisble] = useState<string>("");

    const [getCombatPlayerAbilities] = useLazyGetCombatAbilitiesQuery();
    const sortOptions: Option[] = [
        { value: -1, label: t("Username") },
        { value: 0, label: t("Damage") },
        { value: 1, label: t("Healing") },
        { value: 2, label: t("DamageTaken") },
        { value: 3, label: t("ResourcesRecovery") },
    ];
    const [sortingValue, setSortingValue] = useState<Option | null>(sortOptions[0]);

    const abilityOptions: Option[] = [
        { value: 1, label: t("Potions") },
        { value: 3, label: t("Protection") },
        { value: 4, label: t("Efficiency") },
        { value: 7, label: t("PartyEfficiency") },
        { value: 9, label: t("Food") },
    ];
    const [abilitiesValues, setAbilitiesValues] = useState<readonly Option[]>([abilityOptions[0]]);

    useEffect(() => {
        filter();
    }, [sortingValue]);

    useEffect(() => {
        const loadAbilities = async () => {
            try {
                const results = await Promise.all(
                    filteredCombatPlayers.map(async player => {
                        const abilities = await getAbilities(player.id);
                        return { playerId: player.id, abilities: abilities ?? [] };
                    })
                );

                const map = new Map<number, CombatAbilityModel[]>();
                results.forEach(r => {
                    map.set(r.playerId, r.abilities);
                });

                setAllAbilities(map);
            } catch (e) {
                console.error(e);
            }
        };

        loadAbilities();
    }, [abilitiesValues]);

    const getAbilities = async (combatPlayerId: number) => {
        try {
            const selectedAbilities = abilitiesValues.map(x => x.value);
            if (selectedAbilities.length === 0) {
                return [];
            }

            const query = selectedAbilities.map(x => `abilityTypes=${x}`).join("&");
            const abilities = await getCombatPlayerAbilities({ combatPlayerId, query }).unwrap();

            return abilities;
        } catch (e) {
            console.error(e);
        }
    }

    const abilitiesContent = (combatPlayerId: number) => {
        const abilities = allAbilities?.get(combatPlayerId) ?? [];

        return (
            <ul className="ability">
                {abilities.map((ability, index) => (
                    <li key={`${combatPlayerId}-${ability.id}-${index}`} className="ability__item" onMouseOver={() => setAbilityVisisble(`${combatPlayerId}-${index}`)} onMouseLeave={() => setAbilityVisisble("")}>
                        {ability.abilityType === 1 &&
                            <FontAwesomeIcon
                                icon={faFlask}
                            />
                        }
                        {ability.abilityType === 7 &&
                            <FontAwesomeIcon
                                icon={faHourglass}
                            />
                        }
                        {ability.abilityType === 9 &&
                            <FontAwesomeIcon
                                icon={faAppleWhole}
                            />
                        }
                        {(abilityVisisble === `${combatPlayerId}-${index}`) &&
                            <div>{ability.name}</div>
                        }
                    </li>
                ))}
            </ul>
        );
    }

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
        let result = new Array<CombatPlayerModel>();
        if (sortingValue === undefined || sortingValue === null) {
            return;
        }

        if (sortingValue.value >= 0) {
            result = [...combatPlayers].sort(compare);
        }
        else {
            result = [...combatPlayers].sort((a: CombatPlayerModel, b: CombatPlayerModel) => a.player.username.localeCompare(b.player.username));
        }

        setFilteredCombatPlayers(result);
    }

    return (
        <div className="details">
            <ul className="details__filter">
                <li>
                    <div>{t("Sorting")}:</div>
                    <Select<Option>
                        className="options"
                        options={sortOptions}
                        value={sortingValue}
                        onChange={(selected) => setSortingValue(selected)}
                    />
                </li>
                <li>
                    <div>{t("Abilities")}:</div>
                    <Select<Option, true>
                        isMulti
                        className="options"
                        options={abilityOptions}
                        value={abilitiesValues}
                        onChange={setAbilitiesValues}
                    />
                </li>
            </ul>
            <ul>
                {filteredCombatPlayers?.map((combatPlayer) => (
                    <li key={combatPlayer.id} className="card">
                        <div className="card-body">
                            <h5 className="card-title">{combatPlayer.player.username}</h5>
                        </div>
                        {abilitiesContent(combatPlayer.id)}
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