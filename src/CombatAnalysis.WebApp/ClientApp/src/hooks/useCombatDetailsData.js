import DamageDoneHelper from '../components/helpers/DamageDoneHelper';
import DamageTakenHelper from '../components/helpers/DamageTakenHelper';
import HealDoneHelper from '../components/helpers/HealDoneHelper';
import ResourceRecoveryHelper from '../components/helpers/ResourceRecoveryHelper';
import {
    useLazyGetDamageDoneByPlayerIdQuery, useLazyGetHealDoneByPlayerIdQuery, useLazyGetDamageTakenByPlayerIdQuery, useLazyGetResourceRecoveryByPlayerIdQuery
} from '../store/api/CombatParserApi';

const useHealDoneHelper = (combatPlayerId, detailsType) => {
    const [getDamageDoneByPlayerIdAsync] = useLazyGetDamageDoneByPlayerIdQuery();
    const [getHealDoneByPlayerIdAsync] = useLazyGetHealDoneByPlayerIdQuery();
    const [getDamageTakenByPlayerIdAsync] = useLazyGetDamageTakenByPlayerIdQuery();
    const [getResourceRecoveryByPlayerIdAsync] = useLazyGetResourceRecoveryByPlayerIdQuery();

    const getListAsync = async () => {
        const data = await getPlayerDetailsAsync();

        switch (detailsType) {
            case "DamageDone":
                return <DamageDoneHelper detailsData={data} />
            case "HealDone":
                return <HealDoneHelper detailsData={data} />
            case "DamageTaken":
                return <DamageTakenHelper detailsData={data} />
            case "ResourceRecovery":
                return <ResourceRecoveryHelper detailsData={data} />
            default:
                return <DamageDoneHelper detailsData={data} />
        }
    }

    const getPlayerDetailsAsync = async () => {
        let detailsResult = null;
        switch (detailsType) {
            case "DamageDone":
                detailsResult = await getDamageDoneByPlayerIdAsync(combatPlayerId);
                break;
            case "HealDone":
                detailsResult = await getHealDoneByPlayerIdAsync(combatPlayerId);
                break;
            case "DamageTaken":
                detailsResult = await getDamageTakenByPlayerIdAsync(combatPlayerId);
                break;
            case "ResourceRecovery":
                detailsResult = await getResourceRecoveryByPlayerIdAsync(combatPlayerId);
                break;
            default:
                detailsResult = await getDamageDoneByPlayerIdAsync(combatPlayerId);
                break;
        }

        if (detailsResult.data !== undefined) {
            return detailsResult.data;
        }

        return null;
    }

    const getFilteredList = (data) => {
        switch (detailsType) {
            case "DamageDone":
                return <DamageDoneHelper detailsData={data} />
            case "HealDone":
                return <HealDoneHelper detailsData={data} />
            case "DamageTaken":
                return <DamageTakenHelper detailsData={data} />
            case "ResourceRecovery":
                return <ResourceRecoveryHelper detailsData={data} />
            default:
                return <DamageDoneHelper detailsData={data} />
        }
    }

    return [getListAsync, getFilteredList, getPlayerDetailsAsync];
}

export default useHealDoneHelper;