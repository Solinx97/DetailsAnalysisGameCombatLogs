const CommunityRulesItem = ({ setPolicy, t }) => {
    const handleTypeOfCommunityChange = (event) => {
        setPolicy(event.target.value);
    }

    const handleInviteChange = (event) => {

    }

    const handleRemoveChange = (event) => {

    }

    return (
        <ul className="rules">
            <li>
                <div className="rules__title">{t("TypeOfCommunity")}</div>
                <div className="rules__content">
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="type-of-community" id="public" value="0"
                            onChange={handleTypeOfCommunityChange} defaultChecked />
                        <label className="form-check-label" htmlFor="public">{t("Public")}</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="type-of-community" id="private" value="1"
                            onChange={handleTypeOfCommunityChange} />
                        <label className="form-check-label" htmlFor="private">{t("Private")}</label>
                    </div>
                </div>
            </li>
            <li>
                <div className="rules__title">{t("InviteOtherPeople")}</div>
                <div className="rules__content">
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="invite-people" id="invite-people-anyone" value="0"
                            onChange={handleInviteChange} defaultChecked />
                        <label className="form-check-label" htmlFor="invite-people-anyone">{t("Anyone")}</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="invite-people" id="invite-people-special" value="1"
                            onChange={handleInviteChange} />
                        <label className="form-check-label" htmlFor="invite-people-special">{t("Owner")}</label>
                    </div>
                </div>
            </li>
            <li>
                <div className="rules__title">{t("RemoveAnotherPeople")}</div>
                <div className="rules__content">
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="remove-people" id="remove-people-anyone" value="0"
                            onChange={handleRemoveChange} defaultChecked />
                        <label className="form-check-label" htmlFor="remove-people-anyone">{t("Anyone")}</label>
                    </div>
                    <div className="form-check form-check-inline">
                        <input className="form-check-input" type="radio" name="remove-people" id="remove-people-special" value="1"
                            onChange={handleRemoveChange} />
                        <label className="form-check-label" htmlFor="remove-people-special">{t("Owner")}</label>
                    </div>
                </div>
            </li>
        </ul>
    );
}

export default CommunityRulesItem;