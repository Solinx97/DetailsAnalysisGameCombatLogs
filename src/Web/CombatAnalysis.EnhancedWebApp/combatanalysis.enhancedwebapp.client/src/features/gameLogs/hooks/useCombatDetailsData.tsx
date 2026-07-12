import type { JSX } from 'react';
import DamageDoneHelper from '../components/helpers/DamageDoneHelper';
import DamageTakenHelper from '../components/helpers/DamageTakenHelper';
import HealDoneHelper from '../components/helpers/HealDoneHelper';
import ResourceRecoveryHelper from '../components/helpers/ResourceRecoveryHelper';

type CombatDetailsData = {
    getComponentByDetailsTypeAsync: () => Promise<JSX.Element>;
}

const useCombatDetailsData = (combatPlayerId: number, pageSize: number, detailsType: number, t: (key: string) => string): CombatDetailsData => {
    const helpersComponent = {
        0: DamageDoneHelper,
        1: HealDoneHelper,
        2: DamageTakenHelper,
        3: ResourceRecoveryHelper
    };

    const getComponentByDetailsTypeAsync = async (): Promise<JSX.Element> => {
        const HelperComponent = helpersComponent[detailsType as keyof typeof helpersComponent] || DamageDoneHelper;

        return (
            <HelperComponent
                combatPlayerId={combatPlayerId}
                pageSize={pageSize}
                t={t}
                getUserNameWithoutRealm={getUserNameWithoutRealm}
            />
        );
    }

    const getUserNameWithoutRealm = (username: string): string => {
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