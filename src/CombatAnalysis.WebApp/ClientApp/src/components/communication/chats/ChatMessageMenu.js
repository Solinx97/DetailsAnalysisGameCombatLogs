import { faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';

const ChatMessageMenu = ({ editModeIsOn, setEditModeIsOn, deleteMessageAsync, message }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    return (
        <div className="message-menu">
            <FontAwesomeIcon
                icon={faPen}
                title={t("Edit")}
                className={`message-menu__handler${editModeIsOn && "_active"} edit`}
                onClick={() => setEditModeIsOn((item) => !item)}
            />
            <FontAwesomeIcon
                icon={faTrash}
                title={t("Delete")}
                className="message-menu__handler delete"
                onClick={async () => await deleteMessageAsync(message?.id)}
            />
        </div>
    );
}

export default ChatMessageMenu;