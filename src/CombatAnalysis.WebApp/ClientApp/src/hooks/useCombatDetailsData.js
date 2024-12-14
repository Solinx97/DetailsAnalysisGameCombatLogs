import DamageDoneHelper from '../components/helpers/DamageDoneHelper';
import DamageTakenHelper from '../components/helpers/DamageTakenHelper';
import HealDoneHelper from '../components/helpers/HealDoneHelper';
import ResourceRecoveryHelper from '../components/helpers/ResourceRecoveryHelper';
import {
    useLazyGetDamageDoneCountByPlayerIdQuery,
    useLazyGetDamageTakenCountByPlayerIdQuery,
    useLazyGetHealDoneCountByPlayerIdQuery,
    useLazyGetResourceRecoveryCountByPlayerIdQuery
} from '../store/api/CombatParserApi';

const useCombatDetailsData = (combatPlayerId, pageSize, detailsType, t) => {
    const [getDamageDoneCountByPlayerIdAsync] = useLazyGetDamageDoneCountByPlayerIdQuery();
    const [getHealDoneCountByPlayerIdAsync] = useLazyGetHealDoneCountByPlayerIdQuery();
    const [getDamageTakenCountByPlayerIdAsync] = useLazyGetDamageTakenCountByPlayerIdQuery();
    const [getResourceRecoveryCountByPlayerIdAsync] = useLazyGetResourceRecoveryCountByPlayerIdQuery();

    const helpersComponent = {
        "DamageDone": DamageDoneHelper,
        "HealDone": HealDoneHelper,
        "DamageTaken": DamageTakenHelper,
        "ResourceRecovery": ResourceRecoveryHelper
    };

    const counts = {
        "DamageDone": getDamageDoneCountByPlayerIdAsync,
        "HealDone": getHealDoneCountByPlayerIdAsync,
        "DamageTaken": getDamageTakenCountByPlayerIdAsync,
        "ResourceRecovery": getResourceRecoveryCountByPlayerIdAsync
    };

    const getComponentByDetailsTypeAsync = async () => {
        const HelperComponent = helpersComponent[detailsType] || DamageDoneHelper;

        return (
            <HelperComponent
                combatPlayerId={combatPlayerId}
                pageSize={pageSize}
                getCountAsync={getCountAsync}
                t={t}
            />
        );
    }

    const getCountAsync = async () => {
        const response = await counts[detailsType](combatPlayerId);

        if (response.data !== undefined) {
            return response.data;
        }

        return 0;
    }

    return { getComponentByDetailsTypeAsync };
}

export default useCombatDetailsData;