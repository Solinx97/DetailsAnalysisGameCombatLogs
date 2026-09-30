import WoWGameDataContext from '@/context/WoWGameDataContext';
import { faLocationCrosshairs } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useState } from 'react';
import AccountTransmogSets from './AccountTransmogSets';
import AccountTransmogSlots from './AccountTransmogSlots';

const AccountTransmogs: React.FC = () => {
    const context = useContext(WoWGameDataContext);

    if (!context) {
        throw new Error("Child must be inside WoWGameDataContext.Provider");
    }

    const { t } = context;

    const [showSets, setShowSets] = useState<boolean>(false);
    const [showSlots, setShowSlots] = useState<boolean>(false);

    const showTransmogsHandle = (isSet: boolean, status: boolean) => {
        setShowSets(isSet ? status : !status);
        setShowSlots(isSet ? !status : status);
    }

    return (
        <div className="account-collection">
            <div className="account-collection__title">
                <h6>{t("Transmogs")}:</h6>
                <div className="btn-shadow"
                    onClick={() => showTransmogsHandle(true, !showSets)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Sets")}</div>
                </div>
                <div className="btn-shadow"
                    onClick={() => showTransmogsHandle(false, !showSlots)}>
                    <FontAwesomeIcon
                        icon={faLocationCrosshairs}
                    />
                    <div>{t("Slots")}</div>
                </div>
            </div>
            <div className="account-collection-details">
                {showSets
                    ? <AccountTransmogSets />
                    : showSlots
                        ? <AccountTransmogSlots />
                        : <></>
                }
            </div>
        </div>
    );
}

export default AccountTransmogs;