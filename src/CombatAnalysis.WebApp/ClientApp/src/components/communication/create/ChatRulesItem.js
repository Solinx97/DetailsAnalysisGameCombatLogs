import { useTranslation } from 'react-i18next';

const ChatRulesItem = ({ setInvitePeople, setRemovePeople, setPinMessage, setAnnouncements, payload, connector }) => {
    const { t } = useTranslation("communication/create");

    const handleInviteChange = (event) => {
        setInvitePeople(event.target.value);
    }

    const handleRemoveChange = (event) => {
        setRemovePeople(event.target.value);
    }

    const handlePinMessageChange = (event) => {
        setPinMessage(event.target.value);
    }

    const handleAnnounceChange = (event) => {
        setAnnouncements(event.target.value);
    }

    return (
        <div className="create-community__item">
            <div className="title">{t("Rules")}</div>
            <ul className="rules">
                <li>
                    <div>{t("InviteAnotherPeople")}</div>
                    <div className="rules__content">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="invite-people" id="invite-people-anyone" value="0"
                                onChange={handleInviteChange} defaultChecked={payload["invitePeople"] === 0} />
                            <label className="form-check-label" htmlFor="invite-people-anyone">{t("Owner")}</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="invite-people" id="invite-people-special" value="1"
                                onChange={handleInviteChange} defaultChecked={payload["invitePeople"] === 1} />
                            <label className="form-check-label" htmlFor="invite-people-special">{t("Anyone")}</label>
                        </div>
                    </div>
                </li>
                <li>
                    <div>{t("RemoveAnotherPeople")}</div>
                    <div className="rules__content">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="remove-people" id="remove-people-anyone" value="0"
                                onChange={handleRemoveChange} defaultChecked={payload["removePeople"] === 0} />
                            <label className="form-check-label" htmlFor="remove-people-anyone">{t("Owner")}</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="remove-people" id="remove-people-special" value="1"
                                onChange={handleRemoveChange} defaultChecked={payload["removePeople"] === 1} />
                            <label className="form-check-label" htmlFor="remove-people-special">{t("Anyone")}</label>
                        </div>
                    </div>
                </li>
                <li>
                    <div>{t("PinMessage")}</div>
                    <div className="rules__content">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="pin-message" id="pin-message-anyone" value="0"
                                onChange={handlePinMessageChange} defaultChecked={payload["pinMessage"] === 0} />
                            <label className="form-check-label" htmlFor="pin-message-anyone">{t("Owner")}</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="pin-message" id="pin-message-special" value="1"
                                onChange={handlePinMessageChange} defaultChecked={payload["pinMessage"] === 1} />
                            <label className="form-check-label" htmlFor="pin-message-special">{t("Anyone")}</label>
                        </div>
                    </div>
                </li>
                <li>
                    <div>{t("Announcements")}</div>
                    <div className="rules__content">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="announce" id="announce-anyone" value="0"
                                onChange={handleAnnounceChange} defaultChecked={payload["announcements"] === 0} />
                            <label className="form-check-label" htmlFor="announce-anyone">{t("Owner")}</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="announce" id="announce-special" value="1"
                                onChange={handleAnnounceChange} defaultChecked={payload["announcements"] === 1} />
                            <label className="form-check-label" htmlFor="announce-special">{t("Anyone")}</label>
                        </div>
                    </div>
                </li>
            </ul>
            {connector}
        </div>
    );
}

export default ChatRulesItem;