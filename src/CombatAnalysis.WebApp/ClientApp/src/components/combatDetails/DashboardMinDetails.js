import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
    useLazyGetDamageDoneGeneralyByPlayerIdQuery,
    useLazyGetDamageTakenGeneralyByPlayerIdQuery,
    useLazyGetHealDoneGeneralyByPlayerIdQuery,
    useLazyGetResourceRecoveryGeneralyByPlayerIdQuery
} from '../../store/api/CombatParserApi';

const itemsMinCount = 5;

const DashboardMinDetails = ({ combatPlayerId, closeHandle, detailsType }) => {
    const { t } = useTranslation("combatDetails/combatGeneralDetails");

    const [getDamageDoneGeneralyByPlayerIdAsync] = useLazyGetDamageDoneGeneralyByPlayerIdQuery();
    const [getHealDoneGeneralyByPlayerIdAsync] = useLazyGetHealDoneGeneralyByPlayerIdQuery();
    const [getDamageTakenGeneralyByPlayerIdAsync] = useLazyGetDamageTakenGeneralyByPlayerIdQuery();
    const [getResourceRecoveryGeneralyByPlayerIdAsync] = useLazyGetResourceRecoveryGeneralyByPlayerIdQuery();

    const [itemsCount, setItemsCount] = useState(itemsMinCount);
    const [data, setData] = useState([]);
    const [detailsTypeName, setDetailsTypeName] = useState("");

    useEffect(() => {
        if (combatPlayerId === undefined) {
            return;
        }

        const getDamageDoneAsyn = async () => {
            const detailsResult = await getPlayerGeneralDetailsAsync();
            console.log(detailsResult);
            setData(detailsResult);
        }

        getDamageDoneAsyn();
    }, [combatPlayerId]);

    const getPlayerGeneralDetailsAsync = async () => {
        let detailsResult = null;
        switch (detailsType) {
            case 0:
                setDetailsTypeName(t("Damage"));
                detailsResult = await getDamageDoneGeneralyByPlayerIdAsync(combatPlayerId);
                break;
            case 1:
                setDetailsTypeName(t("Healing"));
                detailsResult = await getHealDoneGeneralyByPlayerIdAsync(combatPlayerId);
                break;
            case 2:
                setDetailsTypeName(t("DamageTaken"));
                detailsResult = await getDamageTakenGeneralyByPlayerIdAsync(combatPlayerId);
                break;
            case 3:
                setDetailsTypeName(t("ResourcesRecovery"));
                detailsResult = await getResourceRecoveryGeneralyByPlayerIdAsync(combatPlayerId);
                break;
            default:
                setDetailsTypeName(t("Damage"));
                detailsResult = await getDamageDoneGeneralyByPlayerIdAsync(combatPlayerId);
                break;
        }

        if (detailsResult.data !== undefined) {
            return detailsResult.data;
        }

        return [];
    }

    if (data.length === 0) {
        return <></>;
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
                            <div>{damage.spellOrItem}</div>
                            <div className="value">
                                <div>{damage.value}</div>
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
            {data.length > itemsMinCount
                ? itemsCount === itemsMinCount
                    ? < div className="extend" onClick={() => setItemsCount(data.length)}>More</div>
                    : <div className="extend" onClick={() => setItemsCount(itemsMinCount)}>Less</div>
                : null
            }
        </div>
    );
}

export default memo(DashboardMinDetails);