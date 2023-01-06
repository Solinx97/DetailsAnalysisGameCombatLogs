import DamageDoneHelper from '../components/heleprs/DamageDoneHelper';
import DamageTakenHelper from '../components/heleprs/DamageTakenHelper';
import HealDoneHelper from '../components/heleprs/HealDoneHelper';
import ResourceRecoveryHelper from '../components/heleprs/ResourceRecoveryHelper';

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
        }

        return list;
    }

    return [getListAsync, getFilteredList, getDetailsDataAsync];
}

export default useHealDoneHelper;