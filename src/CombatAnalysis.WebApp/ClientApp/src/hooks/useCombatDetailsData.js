import DamageDoneHelper from '../components/helpers/DamageDoneHelper';
import DamageTakenHelper from '../components/helpers/DamageTakenHelper';
import HealDoneHelper from '../components/helpers/HealDoneHelper';
import ResourceRecoveryHelper from '../components/helpers/ResourceRecoveryHelper';

const useHealDoneHelper = (combatPlayerId, detailsType) => {
    const getDetailsDataAsync = async () => {
        const response = await fetch(`api/v1/${detailsType}/${combatPlayerId}`);
        const detailsData = await response.json();

        return detailsData;
    }

    const getListAsync = async () => {
        let list = null;
        const data = await getDetailsDataAsync();

        switch (detailsType) {
            case "DamageDone":
                list = <DamageDoneHelper detailsData={data} />
                break;
            case "HealDone":
                list = <HealDoneHelper detailsData={data} />
                break;
            case "DamageTaken":
                list = <DamageTakenHelper detailsData={data} />
                break;
            case "ResourceRecovery":
                list = <ResourceRecoveryHelper detailsData={data} />
                break;
            default:
                list = <DamageDoneHelper detailsData={data} />
                break;
        }

        return list;
    }

    const getFilteredList = (data) => {
        let list = null;

        switch (detailsType) {
            case "DamageDone":
                list = <DamageDoneHelper detailsData={data} />
                break;
            case "HealDone":
                list = <HealDoneHelper detailsData={data} />
                break;
            case "DamageTaken":
                list = <DamageTakenHelper detailsData={data} />
                break;
            case "ResourceRecovery":
                list = <ResourceRecoveryHelper detailsData={data} />
                break;
            default:
                list = <DamageDoneHelper detailsData={data} />
                break;
        }

        return list;
    }

    return [getListAsync, getFilteredList, getDetailsDataAsync];
}

export default useHealDoneHelper;