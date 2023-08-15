import { useTranslation } from 'react-i18next';

const DescriptionItem = ({ setChatName, setChatShortName }) => {
    const { t } = useTranslation("communication/chats/createGroupChat");

    return (
        <>
            <div>
                <div className="form-group">
                    <label htmlFor="group-chat-name">{t("Name")}</label>
                    <input type="text" className="form-control" name="name" id="group-chat-name" onChange={(e) => setChatName(e.target.value)} required />
                </div>
                <div className="form-group">
                    <label htmlFor="short-group-chat-name">{t("ShortName")}</label>
                    <input type="text" className="form-control" name="shortName" id="short-group-chat-name" onChange={(e) => setChatShortName(e.target.value)} required />
                </div>
            </div>
            <div>
                <input type="button" value="Next" className="btn btn-success" />
                <input type="button" value={t("Close")} className="btn btn-light" />
            </div>
        </>
    );
}

export default DescriptionItem;