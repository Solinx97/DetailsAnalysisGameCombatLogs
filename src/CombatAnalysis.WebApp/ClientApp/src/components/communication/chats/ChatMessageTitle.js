import { faPen, faTrash, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const ChatMessageTitle = ({ itIsMe, setEditModeIsOn, openMessageMenu, editModeIsOn, deleteMessageAsync, message }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    const { data: user, isLoading } = useGetCustomerByIdQuery(message?.ownerId);

    if (isLoading) {
        return <></>;
    }

    return (
        <div>
            <div className="message-title">
                <div className="username-container">
                    <div className="username-container__username" title={user?.usernam}>{user?.username}</div>
                    {itIsMe &&
                        <FontAwesomeIcon
                        icon={faUser}
                        title={t("Information")}
                        />
                    }
                </div>

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
        </div>

    );
}

export default ChatMessageTitle;