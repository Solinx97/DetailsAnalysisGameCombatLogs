import { faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import User from '../User';

const ChatMessageTitle = ({ me, itIsMe, setEditModeIsOn, openMessageMenu, editModeIsOn, deleteMessageAsync, message }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    const [userInformation, setUserInformation] = useState(null);

    const { data: user, isLoading } = useGetCustomerByIdQuery(message?.customerId);

    const getMessageTime = () => {
        const timeItems = message?.time.split(":");
        const time = `${timeItems[0]}:${timeItems[1]}`;

        return time;
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div>
            <div className="message-title">
                <div className="username-container">
                    <User
                        me={me}
                        itIsMe={itIsMe}
                        targetCustomerId={user.id}
                        setUserInformation={setUserInformation}
                        allowRemoveFriend={false}
                    />
                </div>
                <div className="message-time">{getMessageTime()}</div>
                {openMessageMenu &&
                    <div className="message-menu">
                        <FontAwesomeIcon
                            icon={faPen}
                            title={t("Edit")}
                            className={`message-menu__handler${editModeIsOn && "_active"}`}
                            onClick={() => setEditModeIsOn((item) => !item)}
                        />
                        <FontAwesomeIcon
                            icon={faTrash}
                            title={t("Delete")}
                            className="message-menu__handler"
                            onClick={async () => await deleteMessageAsync(message?.id)}
                        />
                    </div>
                }
            </div>
            {userInformation}
        </div>
    );
}

export default ChatMessageTitle;