import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyCheckIfCustomerExistQuery } from '../../store/api/Customer.api';

import "../../styles/account/registration.scss";

const GeneralRegistrationStep = ({ setStep, customer, updateGeneralInformation, registrationAsync }) => {
    const { t } = useTranslation("account/registration");

    const [checkIfCustomerExistAsync] = useLazyCheckIfCustomerExistQuery();

    const [username, setUsername] = useState(customer.username);
    const [firstName, setFirstName] = useState(customer.firstName);
    const [lastName, setLastName] = useState(customer.lastName);
    const [aboutMe, setAboutMe] = useState(customer.aboutMe);
    const [gender, setGender] = useState(customer.gender);
    const [showCustomerErrorMessage, setShowCustomerErrorMessage] = useState(false);

    const checkIfCredentialExistAsync = async () => {
        const checkIfCustomerExist = await checkIfCustomerExistAsync(username);
        if (checkIfCustomerExist.data !== undefined && checkIfCustomerExist.data) {
            setShowCustomerErrorMessage(true);
            return true;
        }
    }

    const handleUsernameChange = (event) => {
        setUsername(event.target.value);
    }

    const handleFirstNameChange = (event) => {
        setFirstName(event.target.value);
    }

    const handleLastNameChange = (event) => {
        setLastName(event.target.value);
    }

    const handleAboutMeChange = (event) => {
        setAboutMe(event.target.value);
    }

    const handleManSelected = () => {
        setGender(0);
    }

    const handleWomanSelected = () => {
        setGender(1);
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        const isExist = await checkIfCredentialExistAsync();
        if (isExist) {
            return;
        }

        updateGeneralInformation(username, aboutMe, firstName, lastName, gender);
        await registrationAsync();
    }

    return (
        <form className="registration" onSubmit={async (event) => await handleSubmitAsync(event)}>
            <div className="mb-3">
                <label htmlFor="inputUsername" className="form-label">{t("Username")}</label>
                <input type="text" className="form-control" id="inputUsername" aria-describedby="emailHelp"
                    onChange={handleUsernameChange} value={username} required />
            </div>
            <div className="mb-3">
                <label htmlFor="inputFirstName" className="form-label">{t("FirstName")}</label>
                <input type="text" className="form-control" id="inputFirstName" aria-describedby="emailHelp"
                    onChange={handleFirstNameChange} value={firstName} required />
            </div>
            <div className="mb-3">
                <label htmlFor="inputLastName" className="form-label">{t("LastName")}</label>
                <input type="text" className="form-control" id="inputLastName"
                    onChange={handleLastNameChange} value={lastName} required />
            </div>
            <div className="mb-3">
                <label htmlFor="textareaAboutMe" className="form-label">{t("AboutMe")}</label>
                <textarea className="form-control" id="textareaAboutMe" rows="4"
                    onChange={handleAboutMeChange} value={aboutMe} />
            </div>
            <div>
                <div className="form-check form-check-inline">
                    <input className="form-check-input" type="radio" name="inlineRadioOptions" id="man" value="0"
                        onChange={handleManSelected} checked={gender === 0} />
                    <label className="form-check-label" htmlFor="man">{t("Man")}</label>
                </div>
                <div className="form-check form-check-inline">
                    <input className="form-check-input" type="radio" name="inlineRadioOptions" id="woman" value="1"
                        onChange={handleWomanSelected} checked={gender === 1}/>
                    <label className="form-check-label" htmlFor="woman">{t("Woman")}</label>
                </div>
            </div>
            <div className="actions">
                <input type="button" className="btn btn-light" value={t("LastStep")} onClick={() => setStep(0)} />
                <input type="submit" className="btn btn-success" value={t("Registration")} />
            </div>
            {showCustomerErrorMessage &&
                <div className="registration__error-message">{t("UsernameExist")}</div>
            }
        </form>
    );
}

export default GeneralRegistrationStep;