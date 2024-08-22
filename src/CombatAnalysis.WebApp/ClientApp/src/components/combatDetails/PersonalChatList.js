import { faCirclePlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { format } from 'date-fns';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useCreatePersonalChatMessageAsyncMutation } from '../../store/api/communication/chats/PersonalChatMessage.api';
import { useGetCustomerByIdQuery } from '../../store/api/Customer.api';

const messageType = {
    default: 0,
    system: 1,
    log: 2
};

const PersonalChatList = ({ chat, companionId, log }) => {
    const { t } = useTranslation("combatDetails/mainInformation");

    const me = useSelector((state) => state.customer.value);
    const { data: user, isLoading } = useGetCustomerByIdQuery(companionId);
    const [createPersonalChatMessageAsync] = useCreatePersonalChatMessageAsyncMutation();

    const [sent, showSent] = useState(false);

    const createMessageAsync = async () => {
        const date = format(new Date(log.date), 'MM/dd/yyyy HH:mm');
        const message = `${log.id};${log.name};${date}`;

        const today = new Date();
        const newMessage = {
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            status: 0,
            type: messageType["log"],
            chatId: chat.id,
            customerId: me?.id
        };

        const createdMessage = await createPersonalChatMessageAsync(newMessage);
        if (createdMessage.data !== undefined) {
            showSent(true);
        }
    }

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
                : <div className="btn-shadow" title={t("SendToChat")} onClick={async () => await createMessageAsync()}>
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