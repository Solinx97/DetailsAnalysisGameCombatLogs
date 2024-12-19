import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetDamageDoneGeneralByCombatPlayerIdQuery } from '../../store/api/combatParser/DamageDone.api';
import { useLazyGetDamageTakenGeneralByCombatPlayerIdQuery } from '../../store/api/combatParser/DamageTaken.api';
import { useLazyGetHealDoneGeneralByCombatPlayerIdQuery } from '../../store/api/combatParser/HealDone.api';
import { useLazyGetResourceRecoveryGeneralByCombatPlayerIdQuery } from '../../store/api/combatParser/ResourcesRecovery.api';

const itemsMinCount = 5;

const DashboardMinDetails = ({ combatPlayerId, closeHandle, detailsType }) => {
    const { t } = useTranslation("combatDetails/dashboard");

    const detailsTypeFunctions = [
        useLazyGetDamageDoneGeneralByCombatPlayerIdQuery(),
        useLazyGetHealDoneGeneralByCombatPlayerIdQuery(),
        useLazyGetDamageTakenGeneralByCombatPlayerIdQuery(),
        useLazyGetResourceRecoveryGeneralByCombatPlayerIdQuery()
    ];

    const detailsTypeNames = [
        t("Damage"),
        t("Healing"),
        t("DamageTaken"),
        t("ResourcesRecovery")
    ];

    const [getDetailsFunction] = detailsTypeFunctions[detailsType];

    const [itemsCount, setItemsCount] = useState(itemsMinCount);
    const [data, setData] = useState([]);
    const [detailsTypeName] = useState(detailsTypeNames[detailsType]);

    useEffect(() => {
        if (combatPlayerId === undefined) {
            return;
        }

        const fetchDataAsync = async () => {
            const detailsResult = await getDetailsFunction(combatPlayerId).unwrap();
            setData(detailsResult || []);
        }

        fetchDataAsync();
    }, [combatPlayerId, getDetailsFunction]);

    if (data.length === 0) {
        return <div>Loading...</div>;
    }

    return (
        <div className="min-details">
            <div className="min-details__title">
                <div>{detailsTypeName}</div>
                <FontAwesomeIcon
                    icon={faXmark}
                    onClick={closeHandle}
                />
            </div>
            <ul className="information"> 
                {data?.slice(0, itemsCount).filter(damage => damage.value > 0).map((damage, index) => (
                    <li key={index}>
                        <div className="min-details__spells-items">
                            <div>{damage.spell}</div>
                            <div className="value">
                                <div>{damage.value}</div>
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
            {data.length > itemsMinCount && (
                <div className="extend" onClick={() => setItemsCount(itemsCount === itemsMinCount ? data.length : itemsMinCount)}>
                    {itemsCount === itemsMinCount ? t("More") : t("Less")}
                </div>
            )}
        </div>
    );
}

export default memo(DashboardMinDetails);