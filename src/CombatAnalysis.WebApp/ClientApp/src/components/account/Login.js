import React, { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useLoginAsyncMutation } from '../../store/api/Account.api';
import { useLazySearchByUserIdAsyncQuery } from '../../store/api/Customer.api';
import { updateCustomer } from '../../store/slicers/CustomerSlice';
import { updateUser } from '../../store/slicers/UserSlice';

import "../../styles/account/login.scss";

const Login = () => {
    const dispatch = useDispatch();

    const [loginAsync] = useLoginAsyncMutation();
    const [getCustomerAsync] = useLazySearchByUserIdAsyncQuery();

    const navigate = useNavigate();
    const { t } = useTranslation("account/login");

    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const email = useRef(null);
    const password = useRef(null);

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        setShowErrorMessage(false);

        const data = {
            email: email.current.value,
            password: password.current.value
        };

        const user = await loginAsync(data);
        if (user.data !== undefined) {
            dispatch(updateUser(user.data));

            const customer = await getCustomerAsync(user.data.id);
            if (customer.data !== undefined) {
                dispatch(updateCustomer(customer.data));

                navigate('/');
            }
        }
        else {
            setShowErrorMessage(true);
        }
    }

    return (
        <form className="login" onSubmit={handleSubmitAsync}>
            <div className="mb-3">
                <label htmlFor="inputEmail" className="form-label">{t("Email")}</label>
                <input type="email" className="form-control" id="inputEmail" aria-describedby="emailHelp" ref={email} />
            </div>
            <div className="mb-3">
                <label htmlFor="inputPassword" className="form-label">{t("Password")}</label>
                <input type="password" className="form-control" id="inputPassword" ref={password} />
            </div>
            <input type="submit" className="btn btn-primary" value={t("Login")} />
            <div className="login__error-message" style={{ display: showErrorMessage ? "flex" : "none" }}>{t("IncorrectData")}</div>
        </form>
    );
}

export default Login;