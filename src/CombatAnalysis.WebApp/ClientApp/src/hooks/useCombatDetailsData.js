import DamageDoneHelper from '../components/helpers/DamageDoneHelper';
import DamageTakenHelper from '../components/helpers/DamageTakenHelper';
import HealDoneHelper from '../components/helpers/HealDoneHelper';
import ResourceRecoveryHelper from '../components/helpers/ResourceRecoveryHelper';

const useCombatDetailsData = (combatPlayerId, pageSize, detailsType, t) => {
    const helpersComponent = {
        "DamageDone": DamageDoneHelper,
        "HealDone": HealDoneHelper,
        "DamageTaken": DamageTakenHelper,
        "ResourceRecovery": ResourceRecoveryHelper
    };

    const getComponentByDetailsTypeAsync = async () => {
        const HelperComponent = helpersComponent[detailsType] || DamageDoneHelper;

        return (
            <HelperComponent
                combatPlayerId={combatPlayerId}
                pageSize={pageSize}
                getUserNameWithoutRealm={getUserNameWithoutRealm}
                t={t}
            />
        );
    }

    const getUserNameWithoutRealm = (username) => {
        if (!username.includes('-')) {
            return username;
        }

        const realmNameIndex = username.indexOf('-');
        const userNameWithoutRealm = username.substr(0, realmNameIndex);

        return userNameWithoutRealm;
    }

    return { getComponentByDetailsTypeAsync };
}

export default useCombatDetailsData;