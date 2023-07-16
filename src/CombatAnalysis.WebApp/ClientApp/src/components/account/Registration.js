import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import AccountService from '../../services/AccountService';
import CustomerService from '../../services/CustomerService';

import "../../styles/account/registration.scss";

const Registration = () => {
    const accountService = new AccountService();
    const customerService = new CustomerService();

    const navigate = useNavigate();
    const { t, i18n } = useTranslation("registration");

    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [showErrorMessage, setShowErrorMessage] = useState(false);

    const registrationAsync = async () => {
        setShowErrorMessage(false);

        const data = {
            email: email,
            password: password
        };

        try {
            const createdUser = await accountService.registrationAsync(data);
            if (createdUser === null) {
                return;
            }

            await createCustomerAsync(createdUser);
        } catch (SyntaxError) {
            setShowErrorMessage(true);
        }
    }

    const createCustomerAsync = async (accountData) => {
        const data = {
            id: "",
            message: " ",
            username: username,
            aboutMe: " ",
            gender: 1,
            firstName: " ",
            lastName: " ",
            appUserId: accountData.id
        };

        const createdCustomer = await customerService.createAsync(data);
        if (createdCustomer !== null) {
            navigate('/');
        }
    }

    const handleEmailChange = (event) => {
        setEmail(event.target.value);
    }

    const handleUsernameChange = (event) => {
        setUsername(event.target.value);
    }

    const handlePasswordChange = (event) => {
        setPassword(event.target.value);
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        await registrationAsync();
    }

    const render = () => {
        return (
            <form className="registration" onSubmit={handleSubmitAsync}>
                <div className="mb-3">
                    <label htmlFor="inputEmail" className="form-label">{t("Email")}</label>
                    <input type="email" className="form-control" id="inputEmail" aria-describedby="emailHelp" onChange={handleEmailChange} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputUsername" className="form-label">Username</label>
                    <input type="text" className="form-control" id="inputUsername" onChange={handleUsernameChange} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputPassword" className="form-label">{t("Password")}</label>
                    <input type="password" className="form-control" id="inputPassword" onChange={handlePasswordChange} />
                </div>
                <input type="submit" className="btn btn-primary" value="Registration" />
                <div className="registration__error-message" style={{ display: showErrorMessage ? "flex" : "none" }}>This email already used!</div>
            </form>
        );
    }

    return render();
}

export default Registration;