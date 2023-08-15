import { useTranslation } from 'react-i18next';

const PolicyItem = ({ setChatPolicyType }) => {
    const { t } = useTranslation("communication/chats/createGroupChat");

    return (
        <>
            <div className="chat-policy">
                <p>{t("ChatPolicy")}</p>
                <div className="form-check form-check-inline">
                    <input className="form-check-input" type="radio" name="chat-policy" id="public" value="0" defaultChecked onChange={() => setChatPolicyType(0)} />
                    <label className="form-check-label" htmlFor="public">{t("Public")}</label>
                </div>
                <div className="form-check form-check-inline">
                    <input className="form-check-input" type="radio" name="chat-policy" id="private" value="1" onChange={() => setChatPolicyType(1)} />
                    <label className="form-check-label" htmlFor="private">{t("Private")}</label>
                </div>
                <div className="form-check form-check-inline">
                    <input className="form-check-input" type="radio" name="chat-policy" id="privatelinks" value="2" disabled onChange={() => setChatPolicyType(2)} />
                    <label className="form-check-label" htmlFor="privatelinks">{t("PrivateWithLink")}</label>
                </div>
            </div>
            <div>
                <input type="button" value="Next" className="btn btn-success" />
                <input type="button" value={t("Close")} className="btn btn-light" />
            </div>
        </>
    );
}

export default PolicyItem;