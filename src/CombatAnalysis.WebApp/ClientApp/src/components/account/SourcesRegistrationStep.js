import React from 'react';
import { useTranslation } from 'react-i18next';

import "../../styles/account/registration.scss";
import Communities from '../communication/community/Communities';

const SourcesRegistrationStep = ({ setStep, registrationAsync }) => {
    const { t } = useTranslation("account/registration");

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        await registrationAsync();
    }

    return (
        <form className="registration" onSubmit={handleSubmitAsync}>
            <div className="mb-3">
                <label htmlFor="inputUsername" className="form-label">{t("JoinToCommunities")}</label>
                <Communities
                    showCommunitiesAtStart={true}
                />
            </div>
            <div className="actions">
                <input type="button" className="btn btn-light" value={t("LastStep")} onClick={() => setStep(1)} />
                <input type="submit" className="btn btn-success" value={t("Registration")} />
            </div>
        </form>
    );
}

export default SourcesRegistrationStep;