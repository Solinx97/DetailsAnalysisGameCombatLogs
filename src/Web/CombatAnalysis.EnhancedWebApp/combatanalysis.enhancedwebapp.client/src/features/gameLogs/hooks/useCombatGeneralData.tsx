import type { JSX } from 'react';
import { useLazyGetDamageDoneGeneralByUnitIdQuery, useLazyGetDamageTakenGeneralByUnitIdQuery } from '../api/DamageDone.api';
import { useLazyGetHealDoneGeneralByUnitIdQuery } from '../api/HealDone.api';
import { useLazyGetResourceRecoveryGeneralByUnitIdQuery } from '../api/ResourcesRecovery.api';
import DamageDoneGeneralHelper from '../components/helpers/DamageDoneGeneralHelper';
import DamageTakenGeneralHelper from '../components/helpers/DamageTakenGeneralHelper';
import HealDoneGeneralHelper from '../components/helpers/HealDoneGeneralHelper';
import ResourceRecoveryGeneralHelper from '../components/helpers/ResourceRecoveryGeneralHelper';
import type { CombatPlayerModel } from '../types/CombatPlayerModel';
import type { DamageDoneGeneralModel } from '../types/DamageDoneGeneralModel';
import type { HealDoneGeneralModel } from '../types/HealDoneGeneralModel';
import type { ResourceRecoveryGeneralModel } from '../types/ResourceRecoveryGeneralModel';

type CombatGeneralData = readonly [
    () => Promise<JSX.Element>,
    () => Promise<DamageDoneGeneralModel[] | ResourceRecoveryGeneralModel[] | HealDoneGeneralModel[] | null>
]

const useCombatGeneralData = (combatPlayer: CombatPlayerModel, detailsType: number): CombatGeneralData => {
    const fixedNumberUntil = 2;

    const [getDamageDoneGeneralByUnitIdAsync] = useLazyGetDamageDoneGeneralByUnitIdQuery();
    const [getDamageTakenGeneralByUnitIdAsync] = useLazyGetDamageTakenGeneralByUnitIdQuery();
    const [getHealDoneGeneralByUnutIdAsync] = useLazyGetHealDoneGeneralByUnitIdQuery();
    const [getResourceRecoveryGeneralByUnitIdAsync] = useLazyGetResourceRecoveryGeneralByUnitIdQuery();

    const getProcentage = (firstValue: number, secondValue: number): string => {
        const number = firstValue / secondValue;
        const procentage = number * 100;
        const round = procentage.toFixed(2);

        return round;
    }

    const getSpellValueProcentage = (item: DamageDoneGeneralModel | ResourceRecoveryGeneralModel | HealDoneGeneralModel, targetValue: number): string => {
        const procentage = (item.value / targetValue) * 100;

        return procentage.toFixed(fixedNumberUntil);
    }

    const getValueShortName = (value: number): string => {
        const thousands = value / 1000;
        const millions = value / 1000000;

        if (millions >= 1) {
            return `${millions.toFixed(fixedNumberUntil)} M`;
        }
        else if (thousands >= 1) {
            return `${thousands.toFixed(fixedNumberUntil)} K`;
        }

        return `${value}`;
    }

    const getGeneralListAsync = async (): Promise<JSX.Element> => {
        let data = null;

        switch (detailsType) {
            case 0:
                data = await getDamageDoneGeneralByUnitIdAsync(combatPlayer.unitId).unwrap();
                return <DamageDoneGeneralHelper
                    generalData={data}
                    getProcentage={getProcentage}
                    combatPlayer={combatPlayer}
                    getValueShortName={getValueShortName}
                    getSpellValueProcentage={getSpellValueProcentage}
                />
            case 1:
                data = await getHealDoneGeneralByUnutIdAsync(combatPlayer.unitId).unwrap();
                return <HealDoneGeneralHelper
                    generalData={data}
                    getProcentage={getProcentage}
                    combatPlayer={combatPlayer}
                    getValueShortName={getValueShortName}
                    getSpellValueProcentage={getSpellValueProcentage}
                />
            case 2:
                data = await getDamageTakenGeneralByUnitIdAsync(combatPlayer.unitId).unwrap();
                return <DamageTakenGeneralHelper
                    generalData={data}
                    getProcentage={getProcentage}
                    combatPlayer={combatPlayer}
                    getValueShortName={getValueShortName}
                    getSpellValueProcentage={getSpellValueProcentage}
                />
            case 3:
                data = await getResourceRecoveryGeneralByUnitIdAsync(combatPlayer.unitId).unwrap();
                return <ResourceRecoveryGeneralHelper
                    generalData={data}
                    combatPlayer={combatPlayer}
                    getValueShortName={getValueShortName}
                    getSpellValueProcentage={getSpellValueProcentage}
                />
            default:
                data = await getDamageDoneGeneralByUnitIdAsync(combatPlayer.unitId).unwrap();
                return <DamageDoneGeneralHelper
                    generalData={data}
                    getProcentage={getProcentage}
                    combatPlayer={combatPlayer}
                    getValueShortName={getValueShortName}
                    getSpellValueProcentage={getSpellValueProcentage}
                />
        }
    }

    const getPlayerGeneralDetailsAsync = async (): Promise<DamageDoneGeneralModel[] | ResourceRecoveryGeneralModel[] | HealDoneGeneralModel[] | null> => {
        try {
            let detailsResult: DamageDoneGeneralModel[] | ResourceRecoveryGeneralModel[] | HealDoneGeneralModel[] | null = null;
            switch (detailsType) {
                case 0:
                    detailsResult = await getDamageDoneGeneralByUnitIdAsync(combatPlayer.unitId).unwrap();
                    break;
                case 1:
                    detailsResult = await getHealDoneGeneralByUnutIdAsync(combatPlayer.unitId).unwrap();;
                    break;
                case 2:
                    detailsResult = await getDamageTakenGeneralByUnitIdAsync(combatPlayer.unitId).unwrap();;
                    break;
                case 3:
                    detailsResult = await getResourceRecoveryGeneralByUnitIdAsync(combatPlayer.unitId).unwrap();;
                    break;
                default:
                    detailsResult = await getDamageDoneGeneralByUnitIdAsync(combatPlayer.unitId).unwrap();
                    break;
            }

            return detailsResult;
        } catch (e) {
            console.error(e);

            return null;
        }
    }

    return [getGeneralListAsync, getPlayerGeneralDetailsAsync] as const;
}

export default useCombatGeneralData;