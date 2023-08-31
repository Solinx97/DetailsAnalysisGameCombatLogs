import { useTranslation } from 'react-i18next';

const CommonItem = ({ chatName, setChatName, connector }) => {
    const { t } = useTranslation("communication/chats/createGroupChat");

    return (
        <div className="create-group-chat__item">
            <div className="title">{t("Description")}</div>
            <div>
                <div>
                    <div className="form-group">
                        <label htmlFor="group-chat-name">{t("Name")}</label>
                        <input type="text" className="form-control" name="name" id="group-chat-name"
                            onChange={(e) => setChatName(e.target.value)} defaultValue={chatName} required />
                    </div>
                </div>
                {connector}
            </div>
        </div>
    );
}

export default CommonItem;