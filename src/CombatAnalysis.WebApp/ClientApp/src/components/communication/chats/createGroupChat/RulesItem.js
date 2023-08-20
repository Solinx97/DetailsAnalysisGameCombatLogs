import { useTranslation } from 'react-i18next';

const RulesItem = ({ connector }) => {
    const { t } = useTranslation("communication/chats/createGroupChat");

    const handleInviteChange = (event) => {

    }

    const handleRemoveChange = (event) => {

    }

    const handlePinMessageChange = (event) => {

    }

    const handleAnnounceChange = (event) => {

    }

    return (
        <div className="create-group-chat__item">
            <div>{t("Rules")}</div>
            <ul className="chat-rules">
                <li>
                    <div>{t("InviteAnotherPeople")}</div>
                    <div className="chat-rules__rules">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="invite-people" id="invite-people-anyone" value="0"
                                onChange={handleInviteChange} defaultChecked disabled/>
                            <label className="form-check-label" htmlFor="invite-people-anyone">{t("Anyone")}</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="invite-people" id="invite-people-special" value="1"
                                onChange={handleInviteChange} disabled/>
                            <label className="form-check-label" htmlFor="invite-people-special">{t("SpecialPeople")}</label>
                        </div>
                    </div>
                </li>
                <li>
                    <div>{t("RemoveAnotherPeople")}</div>
                    <div className="chat-rules__rules">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="remove-people" id="remove-people-anyone" value="0"
                                onChange={handleRemoveChange} defaultChecked disabled/>
                            <label className="form-check-label" htmlFor="remove-people-anyone">{t("Anyone")}</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="remove-people" id="remove-people-special" value="1"
                                onChange={handleRemoveChange} disabled/>
                            <label className="form-check-label" htmlFor="remove-people-special">{t("SpecialPeople")}</label>
                        </div>
                    </div>
                </li>
                <li>
                    <div>{t("PinMessage")}</div>
                    <div className="chat-rules__rules">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="pin-message" id="pin-message-anyone" value="0"
                                onChange={handlePinMessageChange} defaultChecked disabled/>
                            <label className="form-check-label" htmlFor="pin-message-anyone">{t("Anyone")}</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="pin-message" id="pin-message-special" value="1"
                                onChange={handlePinMessageChange} disabled/>
                            <label className="form-check-label" htmlFor="pin-message-special">{t("SpecialPeople")}</label>
                        </div>
                    </div>
                </li>
                <li>
                    <div>{t("Announcements")}</div>
                    <div className="chat-rules__rules">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="announce" id="announce-anyone" value="0"
                                onChange={handleAnnounceChange} defaultChecked disabled/>
                            <label className="form-check-label" htmlFor="announce-anyone">{t("Anyone")}</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="pin-message" id="announce-special" value="1"
                                onChange={handleAnnounceChange} disabled/>
                            <label className="form-check-label" htmlFor="announce-special">{t("SpecialPeople")}</label>
                        </div>
                    </div>
                </li>
            </ul>
            {connector}
        </div>
    );
}

export default RulesItem;