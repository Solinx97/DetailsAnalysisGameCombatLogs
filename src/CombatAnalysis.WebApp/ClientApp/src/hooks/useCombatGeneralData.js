import DamageDoneGeneralHelper from '../components/helpers/DamageDoneGeneralHelper';
import DamageTakenGeneralHelper from '../components/helpers/DamageTakenGeneralHelper';
import HealDoneGeneralHelper from '../components/helpers/HealDoneGeneralHelper';
import ResourceRecoveryGeneralHelper from '../components/helpers/ResourceRecoveryGeneralHelper';
import {
    useLazyGetResourceRecoveryGeneralyByPlayerIdQuery
} from '../store/api/combatParser/ResourcesRecovery.api';
import {
    useLazyGetDamageDoneGeneralyByPlayerIdQuery,
} from '../store/api/combatParser/DamageDone.api';
import {
    useLazyGetDamageTakenGeneralyByPlayerIdQuery,
} from '../store/api/combatParser/DamageTaken.api';
import {
    useLazyGetHealDoneGeneralyByPlayerIdQuery,
} from '../store/api/combatParser/HealDone.api';

const useCombatGeneralData = (combatPlayerId, detailsType) => {
    const [getDamageDoneGeneralyByPlayerIdAsync] = useLazyGetDamageDoneGeneralyByPlayerIdQuery();
    const [getHealDoneGeneralyByPlayerIdAsync] = useLazyGetHealDoneGeneralyByPlayerIdQuery();
    const [getDamageTakenGeneralyByPlayerIdAsync] = useLazyGetDamageTakenGeneralyByPlayerIdQuery();
    const [getResourceRecoveryGeneralyByPlayerIdAsync] = useLazyGetResourceRecoveryGeneralyByPlayerIdQuery();

    const getGeneralListAsync = async () => {
        const data = await getPlayerGeneralDetailsAsync();

        switch (detailsType) {
            case "DamageDone":
                return <DamageDoneGeneralHelper
                    generalData={data}
                />
            case "HealDone":
                return <HealDoneGeneralHelper
                    generalData={data}
                />
            case "DamageTaken":
                return <DamageTakenGeneralHelper
                    generalData={data}
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
                detailsResult = await getDamageDoneGeneralyByPlayerIdAsync(combatPlayerId);
                break;
            case "HealDone":
                detailsResult = await getHealDoneGeneralyByPlayerIdAsync(combatPlayerId);
                break;
            case "DamageTaken":
                detailsResult = await getDamageTakenGeneralyByPlayerIdAsync(combatPlayerId);
                break;
            case "ResourceRecovery":
                detailsResult = await getResourceRecoveryGeneralyByPlayerIdAsync(combatPlayerId);
                break;
            default:
                detailsResult = await getDamageDoneGeneralyByPlayerIdAsync(combatPlayerId);
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