import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyCheckIfUserExistQuery } from '../../store/api/Account.api';

import "../../styles/account/registration.scss";
import { useEffect } from 'react';

const ConfidentialRegistrationStep = ({ setStep, user, updateConfidentialInformation }) => {
    const { t } = useTranslation("account/registration");

    const [checkIfUserExistAsync] = useLazyCheckIfUserExistQuery();

    const [email, setEmail] = useState(user.email);
    const [phoneNumber, setPhoneNumber] = useState(user.phoneNumber);
    const [birthday, setBirthday] = useState(user.birthday);
    const [password, setPassword] = useState(user.password);
    const [confirmPassword, setConfirmPassword] = useState("");
    const [showUserErrorMessage, setShowUserErrorMessage] = useState(false);
    const [minDateOfBirthday, setMinDateOfBirthday] = useState("2024-01-01");

    useEffect(() => {
        getMinBirthdayDate();
    }, [])

    const checkIfCredentialExistAsync = async () => {
        const checkIfUserExist = await checkIfUserExistAsync(email);
        if (checkIfUserExist.data !== undefined && checkIfUserExist.data) {
            setShowUserErrorMessage(true);
            return true;
        }
    }

    const handleEmailChange = (event) => {
        setEmail(event.target.value);
    }

    const handlePhoneNumberChange = (event) => {
        setPhoneNumber(event.target.value);
    }

    const handleBirthdayChange = (event) => {
        setBirthday(event.target.value);
    }

    const handlePasswordChange = (event) => {
        setPassword(event.target.value);
    }

    const handleConfirmPasswordChange = (event) => {
        setConfirmPassword(event.target.value);
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        if (password !== confirmPassword) {
            return;
        }

        const isExist = await checkIfCredentialExistAsync();
        if (isExist) {
            return;
        }

        updateConfidentialInformation(email, phoneNumber, birthday, password);
        setStep(1);
    }

    const getMinBirthdayDate = () => {
        const date = new Date();
        const dateOfMonth = date.getDate();
        const month = date.getMonth();
        const actuallyMonth = month < 10 ? `0${month + 1}` : month + 1;
        const year = date.getFullYear();

        const minDate = `${year}-${actuallyMonth}-${dateOfMonth}`;

        setMinDateOfBirthday(minDate);
        setBirthday(minDate);
    }

    return (
        <form className="registration" onSubmit={async (event) => await handleSubmitAsync(event)}>
            <div className="mb-3">
                <label htmlFor="input-email" className="form-label">{t("Email")}</label>
                <input type="email" className="form-control" id="input-email" aria-describedby="emailHelp"
                    onChange={handleEmailChange} value={email} required />
            </div>
            <div className="mb-3">
                <label htmlFor="input-phone-number" className="form-label">{t("PhoneNumber")}</label>
                <input type="number" className="form-control" id="input-phone-number"
                    onChange={handlePhoneNumberChange} value={phoneNumber} required />
            </div>
            <div className="mb-3">
                <label htmlFor="input-birthday" className="form-label">{t("Birthday")}</label>
                <input type="date" className="form-control" id="input-birthday"
                    onChange={handleBirthdayChange} value={birthday} max={minDateOfBirthday} required />
            </div>
            <div className="mb-3">
                <label htmlFor="new-password" className="form-label">{t("Password")}</label>
                <input className="form-control" id="new-password" type="password" autoComplete="new-password"
                    onChange={handlePasswordChange} value={password} required />
            </div>
            <div className="mb-3">
                <label htmlFor="confirm-password" className="form-label">{t("ConfirmPassword")}</label>
                <input className="form-control" id="confirm-password" type="password" autoComplete="new-password"
                    onChange={handleConfirmPasswordChange} value={confirmPassword} required />
            </div>
            <div className="actions">
                <input type="submit" className="btn btn-info" value={t("NextStep")} />
            </div>
            {showUserErrorMessage &&
                <div className="registration__error-message">{t("EmailExist")}</div>
            }
        </form>
    );
}

export default ConfidentialRegistrationStep;