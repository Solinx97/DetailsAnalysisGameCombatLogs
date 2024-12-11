import DamageDoneHelper from '../components/helpers/DamageDoneHelper';
import DamageTakenHelper from '../components/helpers/DamageTakenHelper';
import HealDoneHelper from '../components/helpers/HealDoneHelper';
import ResourceRecoveryHelper from '../components/helpers/ResourceRecoveryHelper';
import {
    useLazyGetDamageDoneByPlayerIdQuery,
    useLazyGetDamageDoneCountByPlayerIdQuery,
    useLazyGetDamageTakenByPlayerIdQuery,
    useLazyGetDamageTakenCountByPlayerIdQuery,
    useLazyGetHealDoneByPlayerIdQuery,
    useLazyGetHealDoneCountByPlayerIdQuery,
    useLazyGetResourceRecoveryByPlayerIdQuery,
    useLazyGetResourceRecoveryCountByPlayerIdQuery,
} from '../store/api/CombatParserApi';

const useCombatDetailsData = (combatPlayerId, detailsType, combatStartDate) => {
    const [getDamageDoneByPlayerIdAsync] = useLazyGetDamageDoneByPlayerIdQuery();
    const [getHealDoneByPlayerIdAsync] = useLazyGetHealDoneByPlayerIdQuery();
    const [getDamageTakenByPlayerIdAsync] = useLazyGetDamageTakenByPlayerIdQuery();
    const [getResourceRecoveryByPlayerIdAsync] = useLazyGetResourceRecoveryByPlayerIdQuery();

    const [getDamageDoneCountByPlayerIdAsync] = useLazyGetDamageDoneCountByPlayerIdQuery();
    const [getHealDoneCountByPlayerIdAsync] = useLazyGetHealDoneCountByPlayerIdQuery();
    const [getDamageTakenCountByPlayerIdAsync] = useLazyGetDamageTakenCountByPlayerIdQuery();
    const [getResourceRecoveryCountByPlayerIdAsync] = useLazyGetResourceRecoveryCountByPlayerIdQuery();

    const helperComponents = {
        "DamageDone": DamageDoneHelper,
        "HealDone": HealDoneHelper,
        "DamageTaken": DamageTakenHelper,
        "ResourceRecovery": ResourceRecoveryHelper
    };

    const playersDetails = {
        "DamageDone": getDamageDoneByPlayerIdAsync,
        "HealDone": getHealDoneByPlayerIdAsync,
        "DamageTaken": getDamageTakenByPlayerIdAsync,
        "ResourceRecovery": getResourceRecoveryByPlayerIdAsync
    }

    const counts = {
        "DamageDone": getDamageDoneCountByPlayerIdAsync,
        "HealDone": getHealDoneCountByPlayerIdAsync,
        "DamageTaken": getDamageTakenCountByPlayerIdAsync,
        "ResourceRecovery": getResourceRecoveryCountByPlayerIdAsync
    };

    const getPlayerDetailsAsync = async (page, pageSize) => {
        const arg = {
            combatPlayerId,
            page,
            pageSize
        };

        const detailsResult = await playersDetails[detailsType](arg);
        if (detailsResult.data !== undefined) {
            return detailsResult.data;
        }

        return null;
    }

    const getCombatDataListAsync = async (page = 1, pageSize = 10) => {
        const data = await getPlayerDetailsAsync(page, pageSize);
        const HelperComponent = helperComponents[detailsType] || DamageDoneHelper;

        return (<HelperComponent
            detailsData={data}
        />);
    }

    const getCountAsync = async () => {
        const response = await counts[detailsType](combatPlayerId);

        if (response.data !== undefined) {
            return response.data;
        }

        return 0;
    }

    return { getCombatDataListAsync, getPlayerDetailsAsync, getCountAsync };
}

export default useCombatDetailsData;