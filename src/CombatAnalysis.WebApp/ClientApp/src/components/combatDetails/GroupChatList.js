import { faCirclePlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { format } from 'date-fns';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useGetGroupChatByIdQuery } from '../../store/api/communication/chats/GroupChat.api';
import { useCreateGroupChatMessageAsyncMutation } from '../../store/api/communication/chats/GroupChatMessage.api';

const messageType = {
    default: 0,
    system: 1,
    log: 2
};

const GroupChatList = ({ groupChatId, log }) => {
    const { t } = useTranslation("combatDetails/mainInformation");

    const me = useSelector((state) => state.customer.value);
    const { data: groupChat, isLoading } = useGetGroupChatByIdQuery(groupChatId);
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();

    const [sent, showSent] = useState(false);

    const createMessageAsync = async () => {
        const date = format(new Date(log.date), 'MM/dd/yyyy HH:mm')
        const message = `${log.id};${log.name};${date}`;

        const today = new Date();
        const newMessage = {
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            status: 0,
            type: messageType["log"],
            groupChatId: groupChat.id,
            customerId: me?.id
        };

        const createdMessage = await createGroupChatMessageAsync(newMessage);
        if (createdMessage.data !== undefined) {
            showSent(true);
        }
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="chat">
            <div>{groupChat?.name}</div>
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

export default GroupChatList;