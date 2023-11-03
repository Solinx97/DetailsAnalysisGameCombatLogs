import { faCirclePlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../store/api/Customer.api';

const PersonalChatList = ({ chat, companionId, log }) => {
    const { t } = useTranslation("combatDetails/mainInformation");

    const { data: user, isLoading } = useGetCustomerByIdQuery(companionId);

    const [sent, showSent] = useState(false);

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="chat">
            <div>{user?.username}</div>
            {sent
                ? <div className="sent">
                    <div>{t("Sent")}</div>
                </div>
                : <div className="btn-shadow" title={t("SendToChat")}>
                    <FontAwesomeIcon
                        icon={faCirclePlus}
                    />
                    <div>{t("Add")}</div>
                </div>
            }
        </div>
    );
}

export default PersonalChatList;