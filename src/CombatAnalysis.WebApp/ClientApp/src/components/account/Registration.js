import React, { useState, } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

import "../../styles/account/registration.scss";

const Registration = () => {
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

        const response = await fetch("api/v1/Account/registration", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (response.status === 200) {
            try {
                const createdUser = await response.json();
                await createCustomerAsync(createdUser);
            } catch (SyntaxError) {
                setShowErrorMessage(true);
            }
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

        const response = await fetch("api/v1/Customer", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        const result = await response;
        if (result.status === 200) {
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
        return (<form className="registration" onSubmit={handleSubmitAsync}>
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
        </form>);
    }

    return render();
}

export default Registration;