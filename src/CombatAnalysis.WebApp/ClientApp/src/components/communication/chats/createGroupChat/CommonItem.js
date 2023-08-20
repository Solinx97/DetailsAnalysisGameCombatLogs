import { useTranslation } from 'react-i18next';

const CommonItem = ({ chatName, setChatName, chatShortName, setChatShortName, connector }) => {
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
                    <div className="form-group">
                        <label htmlFor="short-group-chat-name">{t("ShortName")}</label>
                        <input type="text" className="form-control" name="shortName" id="short-group-chat-name"
                            onChange={(e) => setChatShortName(e.target.value)} defaultValue={chatShortName} required />
                    </div>
                </div>
                {connector}
            </div>
        </div>
    );
}

export default CommonItem;