import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { useEditAsyncMutation } from '../../../store/api/Account.api';
import { useEditCustomerAsyncMutation, useLazyGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { updateCustomer } from '../../../store/slicers/CustomerSlice';
import { updateUser } from '../../../store/slicers/UserSlice';

import "../../../styles/communication/myEnvironment/profile.scss";

const Profile = () => {
    const { t } = useTranslation("communication/myEnvironment/profile");

    const dispatch = useDispatch();
    const customer = useSelector((state) => state.customer.value);
    const user = useSelector((state) => state.user.value);

    const [editUserAsyncMut] = useEditAsyncMutation();
    const [editCustomerAsyncMut] = useEditCustomerAsyncMutation();
    const [getCustomerById] = useLazyGetCustomerByIdQuery();

    const [isEditMode, setIsEditMode] = useState(false);
    const [privacyHidden, setPrivacyHidden] = useState(false);
    const [generalHidden, setGeneralHidden] = useState(false);

    const email = useRef(null);
    const phoneNumber = useRef(null);
    const birthday = useRef(null);
    const message = useRef(null);
    const username = useRef(null);
    const aboutMe = useRef(null);
    const firstName = useRef(null);
    const lastName = useRef(null);
    const currentPassword = useRef(null);
    const password = useRef(null);
    const confirmPassword = useRef(null);

    useEffect(() => {
        getUserInformation(user);
        getCustomerInformation(customer);
    }, []);

    useEffect(() => {
        if (email.current === null) {
            return;
        }

        getUserInformation(user);
    }, [isEditMode, email.current, privacyHidden]);

    useEffect(() => {
        if (message.current === null) {
            return;
        }

        getCustomerInformation(customer);
    }, [isEditMode, message.current, generalHidden]);

    const getUserInformation = (user) => {
        const date = new Date(user.birthday);
        const correntMonthNumber = date.getMonth() === 0 ? 1 : date.getMonth() + 1;
        const month = correntMonthNumber < 10 ? `0${correntMonthNumber}` : correntMonthNumber;
        const day = date.getDate() < 10 ? `0${date.getDate()}` : date.getDate();

        const birthdayFromDate = `${date.getFullYear()}-${month}-${day}`;

        email.current.value = user.email;
        phoneNumber.current.value = user.phoneNumber;
        birthday.current.value = birthdayFromDate;
    }

    const getCustomerInformation = (customer) => {
        message.current.value = customer.message;
        username.current.value = customer.username;
        aboutMe.current.value = customer.aboutMe;
        firstName.current.value = customer.firstName;
        lastName.current.value = customer.lastName;
    }

    const editUserAsync = async () => {
        const updateForUser = {
            id: user?.id,
            email: email.current.value,
            phoneNumber: phoneNumber.current.value,
            birthday: birthday.current.value,
            password: user?.password
        };

        if (password.current.value !== "" && password.current.value !== user?.password
            && password.current.value === confirmPassword.current.value) {
            updateForUser.password = password.current.value;
        }

        const updatedUser = await editUserAsyncMut(updateForUser);
        if (updatedUser.data !== undefined) {
            dispatch(updateUser(updatedUser.data));

            await editCustomerAsync(updatedUser.data.id);
        }

        setIsEditMode(false);
    }

    const editCustomerAsync = async (userId) => {
        const updateForCustomer = {
            id: customer?.id,
            message: message.current.value,
            username: username.current.value,
            aboutMe: aboutMe.current.value,
            firstName: firstName.current.value,
            lastName: lastName.current.value,
            gender: customer.gender,
            appUserId: userId
        };

        const updatedItem = await editCustomerAsyncMut(updateForCustomer);
        if (updatedItem.data !== undefined) {
            const updatedCustomer = await getCustomerById(customer?.id);

            dispatch(updateCustomer(updatedCustomer.data));
        }
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        await editUserAsync();
    }

    const openEditForm = () => {
        setIsEditMode(false);
    }

    const editForm = () => {
        return (
            <form className="profile__edit-profile" onSubmit={handleSubmitAsync}>
                <div className="title">
                    <div>{t("Privacy")}</div>
                    {privacyHidden
                        ? <FontAwesomeIcon
                            icon={faArrowDown}
                            onClick={() => setPrivacyHidden(!privacyHidden)}
                        />
                        : <FontAwesomeIcon
                            icon={faArrowUp}
                            onClick={() => setPrivacyHidden(!privacyHidden)}
                        />
                    }
                </div>
                {!privacyHidden &&
                    <div className="privacy">
                        <div className="mb-3">
                            <label htmlFor="inputPhoneNumber" className="form-label">{t("PhoneNumber")}</label>
                            <input type="number" className="form-control" id="inputPhoneNumber" ref={phoneNumber} />
                        </div>
                        <div className="mb-3">
                            <label htmlFor="inputBithdayl" className="form-label">{t("Birthday")}</label>
                            <input type="date" className="form-control" id="inputBithdayl" ref={birthday} />
                        </div>
                    </div>
                }
                <div className="title">
                    <div>{t("General")}</div>
                    {generalHidden
                        ? <FontAwesomeIcon
                            icon={faArrowDown}
                            onClick={() => setGeneralHidden(!generalHidden)}
                        />
                        : <FontAwesomeIcon
                            icon={faArrowUp}
                            onClick={() => setGeneralHidden(!generalHidden)}
                        />
                    }
                </div>
                {!generalHidden &&
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
                    </div>
                }
                <div className="actions">
                    <div className="btn-shadow save" onClick={handleSubmitAsync}>{t("Save")}</div>
                    <div className="btn-shadow" onClick={openEditForm}>{t("Cancel")}</div>
                </div>
            </form>
        );
    }

    const information = () => {
        return (
            <div className="profile__information">
                <div className="title">
                    <div>{t("Privacy")}</div>
                    {privacyHidden
                        ? <FontAwesomeIcon
                            icon={faArrowDown}
                            onClick={() => setPrivacyHidden(!privacyHidden)}
                        />
                        : <FontAwesomeIcon
                            icon={faArrowUp}
                            onClick={() => setPrivacyHidden(!privacyHidden)}
                        />
                    }
                </div>
                {!privacyHidden &&
                    <div className="privacy">
                        <div className="mb-3">
                            <label htmlFor="inputEmail" className="form-label">Email</label>
                            <input type="email" className="form-control" id="inputEmail" aria-describedby="emailHelp" ref={email} disabled />
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
                }
                <div className="title">
                    <div>{t("General")}</div>
                    {generalHidden
                        ? <FontAwesomeIcon
                            icon={faArrowDown}
                            onClick={() => setGeneralHidden(!generalHidden)}
                        />
                        : <FontAwesomeIcon
                            icon={faArrowUp}
                            onClick={() => setGeneralHidden(!generalHidden)}
                        />
                    }
                </div>
                {!generalHidden &&
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
                    </div>
                }
                <div className="actions">
                    <div className="btn-shadow" onClick={() => setIsEditMode(true)}>{t("Edit")}</div>
                </div>
            </div>
        );
    }

    return (
        <div className="profile">
            {isEditMode
                ? editForm()
                : information()
            }
        </div>
    );
}

export default Profile;