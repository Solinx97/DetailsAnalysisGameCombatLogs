import DamageDoneGeneralHelper from '../components/heleprs/DamageDoneGeneralHelper';
import DamageTakenGeneralHelper from '../components/heleprs/DamageTakenGeneralHelper';
import HealDoneGeneralHelper from '../components/heleprs/HealDoneGeneralHelper';
import ResourceRecoveryGeneralHelper from '../components/heleprs/ResourceRecoveryGeneralHelper';

const useHealDoneHelper = (combatPlayerId, detailsType) => {
    const getGeneralDataAsync = async () => {
        const response = await fetch(`api/v1/${detailsType}General/${combatPlayerId}`);
        const generalData = await response.json();

        return generalData;
    }

    const getGeneralListAsync = async () => {
        let list = null;
        const data = await getGeneralDataAsync();

        switch (detailsType) {
            case "DamageDone":
                list = <DamageDoneGeneralHelper generalData={data} />
                break;
            case "HealDone":
                list = <HealDoneGeneralHelper generalData={data} />
                break;
            case "DamageTaken":
                list = <DamageTakenGeneralHelper generalData={data} />
                break;
            case "ResourceRecovery":
                list = <ResourceRecoveryGeneralHelper generalData={data} />
                break;
        }

        return list;
    }

    return [getGeneralListAsync, getGeneralDataAsync];
}

export default useHealDoneHelper;