import DamageDoneGeneralHelper from '../components/helpers/DamageDoneGeneralHelper';
import DamageTakenGeneralHelper from '../components/helpers/DamageTakenGeneralHelper';
import HealDoneGeneralHelper from '../components/helpers/HealDoneGeneralHelper';
import ResourceRecoveryGeneralHelper from '../components/helpers/ResourceRecoveryGeneralHelper';
import { useLazyGetDamageDoneGeneralByCombatPlayerIdQuery } from '../store/api/combatParser/DamageDone.api';
import { useLazyGetDamageTakenGeneralByCombatPlayerIdQuery } from '../store/api/combatParser/DamageTaken.api';
import { useLazyGetHealDoneGeneralByCombatPlayerIdQuery } from '../store/api/combatParser/HealDone.api';
import { useLazyGetResourceRecoveryGeneralByCombatPlayerIdQuery } from '../store/api/combatParser/ResourcesRecovery.api';

const useCombatGeneralData = (combatPlayerId, detailsType) => {
    const [getDamageDoneGeneralByCombatPlayerIdAsync] = useLazyGetDamageDoneGeneralByCombatPlayerIdQuery();
    const [getDamageTakenGeneralByCombatPlayerIdAsync] = useLazyGetDamageTakenGeneralByCombatPlayerIdQuery();
    const [getHealDoneGeneralByCombatPlayerIdAsync] = useLazyGetHealDoneGeneralByCombatPlayerIdQuery();
    const [getResourceRecoveryGeneralByCombatPlayerIdAsync] = useLazyGetResourceRecoveryGeneralByCombatPlayerIdQuery();

    const getProcentage = (firstValue, secondValue) => {
        const number = firstValue / secondValue;
        const procentage = number * 100;
        const round = procentage.toFixed(2);

        return round;
    }

    const getGeneralListAsync = async () => {
        const data = await getPlayerGeneralDetailsAsync();

        switch (detailsType) {
            case "DamageDone":
                return <DamageDoneGeneralHelper
                    generalData={data}
                    getProcentage={getProcentage}
                />
            case "HealDone":
                return <HealDoneGeneralHelper
                    generalData={data}
                    getProcentage={getProcentage}
                />
            case "DamageTaken":
                return <DamageTakenGeneralHelper
                    generalData={data}
                    getProcentage={getProcentage}
                />
            case "ResourceRecovery":
                return <ResourceRecoveryGeneralHelper
                    generalData={data}
                />
            default:
                return <DamageDoneGeneralHelper
                    generalData={data}
                />
        }
    }

    const getPlayerGeneralDetailsAsync = async () => {
        let detailsResult = null;
        switch (detailsType) {
            case "DamageDone":
                detailsResult = await getDamageDoneGeneralByCombatPlayerIdAsync(combatPlayerId);
                break;
            case "HealDone":
                detailsResult = await getHealDoneGeneralByCombatPlayerIdAsync(combatPlayerId);
                break;
            case "DamageTaken":
                detailsResult = await getDamageTakenGeneralByCombatPlayerIdAsync(combatPlayerId);
                break;
            case "ResourceRecovery":
                detailsResult = await getResourceRecoveryGeneralByCombatPlayerIdAsync(combatPlayerId);
                break;
            default:
                detailsResult = await getDamageDoneGeneralByCombatPlayerIdAsync(combatPlayerId);
                break;
        }

        if (detailsResult.data !== undefined) {
            return detailsResult.data;
        }

        return null;
    }

    return [getGeneralListAsync, getPlayerGeneralDetailsAsync];
}

export default useCombatGeneralData;