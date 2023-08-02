import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import { useEditAsyncMutation, useLoginAsyncMutation } from '../../../store/api/Account.api';

import "../../../styles/communication/profile.scss";

const Profile = () => {
    const { t } = useTranslation("communication/myEnvironment/profile");

    const [currentUser, customer] = useAuthentificationAsync();
    const [loginAsync] = useLoginAsyncMutation();
    const [editAsync] = useEditAsyncMutation();

    const [isEditMode, setIsEditMode] = useState(false);

    const email = useRef(null);
    const phoneNumber = useRef(null);
    const birthday = useRef(null);
    const message = useRef(null);
    const username = useRef(null);
    const aboutMe = useRef(null);
    const firstName = useRef(null);
    const lastName = useRef(null);
    const gender = useRef(null);
    const currentPassword = useRef(null);
    const password = useRef(null);
    const confirmPassword = useRef(null);

    useEffect(() => {
        getUserInformation();
        getCustomerInformation();
    }, [currentUser])

    useEffect(() => {
        getUserInformation();
        getCustomerInformation();
    }, [isEditMode])

    const getUserInformation = () => {
        const date = new Date(currentUser?.birthday);
        const correntMonthNumber = date.getMonth() === 0 ? 1 : date.getMonth();
        const month = correntMonthNumber < 10 ? `0${correntMonthNumber}` : correntMonthNumber;
        const day = date.getDate() < 10 ? `0${date.getDate()}` : date.getDate();

        let birthdayFromDate = `${date.getFullYear()}-${month}-${day}`;

        email.current.value = currentUser?.email;
        phoneNumber.current.value = currentUser?.phoneNumber;
        birthday.current.value = birthdayFromDate;
    }

    const getCustomerInformation = () => {
        email.current.value = currentUser?.email;
        message.current.value = customer?.message;
        username.current.value = customer?.username;
        aboutMe.current.value = customer?.aboutMe;
        firstName.current.value = customer?.firstName;
        lastName.current.value = customer?.lastName;
        gender.current.value = customer?.gender;
    }

    const editUserAsync = async () => {
        const updatesForUser = {
            id: currentUser?.id,
            email: currentUser?.email,
            phoneNumber: currentUser?.phoneNumber,
            birthday: currentUser?.birthday,
            password: currentUser?.password
        };

        updatesForUser.email = email.current.value;
        updatesForUser.phoneNumber = phoneNumber.current.value;
        updatesForUser.birthday = birthday.current.value;

        if (password.current.value !== "" && password.current.value !== currentUser?.password
            && password.current.value === confirmPassword.current.value) {
            updatesForUser.password = password.current.value;
        }

        const updatedItem = await editAsync(updatesForUser);
        if (updatedItem.data !== undefined) {
            const data = {
                email: updatesForUser.email,
                password: updatesForUser.password
            };

            await loginAsync(data);

            setIsEditMode(false);
        }
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        await editUserAsync();
    }

    const editForm = () => {
        return (
            <form onSubmit={handleSubmitAsync}>
                <div>{t("Privacy")}</div>
                <div className="privacy">
                    <div className="mb-3">
                        <label htmlFor="inputEmail" className="form-label">Email</label>
                        <input type="email" className="form-control" id="inputEmail" aria-describedby="emailHelp" ref={email} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputPhoneNumber" className="form-label">{t("PhoneNumber")}</label>
                        <input type="number" className="form-control" id="inputPhoneNumber" ref={phoneNumber} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputBithdayl" className="form-label">{t("Birthday")}</label>
                        <input type="date" className="form-control" id="inputBithdayl" ref={birthday} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputCurrentPassword" className="form-label">{t("CurrentPassword")}</label>
                        <input type="password" className="form-control" id="inputCurrentPassword" ref={currentPassword} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputPassword" className="form-label">{t("NewPassword")}</label>
                        <input type="password" className="form-control" id="inputPassword" ref={password} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputConfirmPassword" className="form-label">{t("ConfirmNewPassword")}</label>
                        <input type="password" className="form-control" id="inputConfirmPassword" ref={confirmPassword} />
                    </div>
                </div>
                <div>{t("General")}</div>
                <div className="general">
                    <div className="mb-3">
                        <label htmlFor="inputMessage" className="form-label">{t("Message")}</label>
                        <input type="text" className="form-control" id="inputMessage" ref={message} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputUsername" className="form-label">{t("Username")}</label>
                        <input type="text" className="form-control" id="inputUsername" ref={username} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputAboutMe" className="form-label">{t("AboutMe")}</label>
                        <input type="text" className="form-control" id="inputAboutMe" ref={aboutMe} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inutFirstName" className="form-label">{t("FirstName")}</label>
                        <input type="text" className="form-control" id="inutFirstName" ref={firstName} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputLastName" className="form-label">{t("LastName")}</label>
                        <input type="text" className="form-control" id="inputLastName" ref={lastName} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputGender" className="form-label">{t("Gender")}</label>
                        <input type="number" className="form-control" id="inputGender" ref={gender} />
                    </div>
                </div>
                <input type="submit" className="btn btn-outline-primary" value={t("Save")} />
                <input type="button" className="btn btn-outline-warning" value={t("Cancel")} onClick={() => setIsEditMode(false)} />
            </form>
        );
    }

    const information = () => {
        return (
            <div>
                <div>{t("Privacy")}</div>
                <div className="privacy">
                    <div className="mb-3">
                        <label htmlFor="inputEmail" className="form-label">Email</label>
                        <input type="email" className="form-control" id="inputEmail" aria-describedby="emailHelp" ref={email} disabled/>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputPhoneNumber" className="form-label">{t("PhoneNumber")}</label>
                        <input type="number" className="form-control" id="inputPhoneNumber" ref={phoneNumber} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputBithdayl" className="form-label">{t("Birthday")}</label>
                        <input type="date" className="form-control" id="inputBithdayl" ref={birthday} disabled />
                    </div>
                </div>
                <div>{t("General")}</div>
                <div className="general">
                    <div className="mb-3">
                        <label htmlFor="inputMessage" className="form-label">{t("Message")}</label>
                        <input type="text" className="form-control" id="inputMessage" ref={message} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputUsername" className="form-label">{t("Username")}</label>
                        <input type="text" className="form-control" id="inputUsername" ref={username} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputAboutMe" className="form-label">{t("AboutMe")}</label>
                        <input type="text" className="form-control" id="inputAboutMe" ref={aboutMe} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inutFirstName" className="form-label">{t("FirstName")}</label>
                        <input type="text" className="form-control" id="inutFirstName" ref={firstName} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputLastName" className="form-label">{t("LastName")}</label>
                        <input type="text" className="form-control" id="inputLastName" ref={lastName} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputGender" className="form-label">{t("Gender")}</label>
                        <input type="number" className="form-control" id="inputGender" ref={gender} disabled />
                    </div>
                </div>
                <input type="button" className="btn btn-outline-primary" value={t("Edit")} onClick={() => setIsEditMode(true)} />
            </div>
        );
    }

    const render = () => {
        return (
            <div className="profile">
                {isEditMode
                    ? editForm()
                    : information()
                }
            </div>
        );
    }

    return render();
}

export default Profile;