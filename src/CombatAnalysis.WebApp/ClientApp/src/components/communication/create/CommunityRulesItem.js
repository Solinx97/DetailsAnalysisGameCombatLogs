import { useTranslation } from 'react-i18next';

const CommunityRulesItem = ({ setPolicy, connector }) => {
    const { t } = useTranslation("communication/chats/createGroupChat");

    const handleTypeOfCommunityChange = (event) => {
        setPolicy(event.target.value);
    }

    const handleInviteChange = (event) => {

    }

    const handleRemoveChange = (event) => {

    }

    return (
        <div className="create-community__item">
            <div>{t("Rules")}</div>
            <ul className="rules">
                <li>
                    <div>{t("TypeOfCommunity")}</div>
                    <div className="rules__content">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="type-of-community" id="pubic" value="0"
                                onChange={handleTypeOfCommunityChange} defaultChecked />
                            <label className="form-check-label" htmlFor="pubic">{t("Pubic")}</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="type-of-community" id="private" value="1"
                                onChange={handleTypeOfCommunityChange} />
                            <label className="form-check-label" htmlFor="private">{t("Private")}</label>
                        </div>
                    </div>
                </li>
                <li>
                    <div>{t("InviteAnotherPeople")}</div>
                    <div className="rules__content">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="invite-people" id="invite-people-anyone" value="0"
                                onChange={handleInviteChange} defaultChecked disabled />
                            <label className="form-check-label" htmlFor="invite-people-anyone">{t("Anyone")}</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="invite-people" id="invite-people-special" value="1"
                                onChange={handleInviteChange} disabled />
                            <label className="form-check-label" htmlFor="invite-people-special">{t("SpecialPeople")}</label>
                        </div>
                    </div>
                </li>
                <li>
                    <div>{t("RemoveAnotherPeople")}</div>
                    <div className="rules__content">
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="remove-people" id="remove-people-anyone" value="0"
                                onChange={handleRemoveChange} defaultChecked disabled />
                            <label className="form-check-label" htmlFor="remove-people-anyone">{t("Anyone")}</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="remove-people" id="remove-people-special" value="1"
                                onChange={handleRemoveChange} disabled />
                            <label className="form-check-label" htmlFor="remove-people-special">{t("SpecialPeople")}</label>
                        </div>
                    </div>
                </li>
            </ul>
            {connector}
        </div>
    );
}

export default CommunityRulesItem;