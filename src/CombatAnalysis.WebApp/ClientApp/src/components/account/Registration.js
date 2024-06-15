import React, { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useRegistrationAsyncMutation } from '../../store/api/Account.api';
import { useCreateAsyncMutation } from '../../store/api/Customer.api';
import { updateCustomer } from '../../store/slicers/CustomerSlice';
import { updateUser } from '../../store/slicers/UserSlice';
import ConfidentialRegistrationStep from './ConfidentialRegistrationStep';
import GeneralRegistrationStep from './GeneralRegistrationStep';
import LoginPreview from './LoginPreview';

import "../../styles/account/registration.scss";

const Registration = () => {
    const [registrationMutAsync] = useRegistrationAsyncMutation();
    const [createCustomerMutAsync] = useCreateAsyncMutation();

    const navigate = useNavigate();
    const dispatch = useDispatch();

    const [step, setStep] = useState(0);
    const [user, setUser] = useState({
        id: " ",
        email: "",
        phoneNumber: "",
        birthday: new Date(),
        password: ""
    });
    const [customer, setCustomer] = useState({
        id: " ",
        message: " ",
        username: "",
        aboutMe: " ",
        firstName: "",
        lastName: "",
        gender: 0,
        appUserId: ""
    });
    const [registrationContent, setRegistrationContent] = useState(null);

    useEffect(() => {
        getRegistrationContent();
    }, [step]);

    const updateConfidentialInformation = (email, phoneNumber, birthday, password) => {
        const updateUser = user;

        updateUser.email = email;
        updateUser.phoneNumber = phoneNumber;
        updateUser.birthday = birthday;
        updateUser.password = password;

        setUser(updateUser);
    }

    const updateGeneralInformation = (username, aboutMe, firstName, lastName, gender) => {
        const updateCustomer = customer;

        updateCustomer.username = username;
        updateCustomer.aboutMe = aboutMe;
        updateCustomer.firstName = firstName;
        updateCustomer.lastName = lastName;
        updateCustomer.gender = +gender;

        setCustomer(updateCustomer);
    }

    const registrationAsync = async () => {
        const createdUser = await registrationMutAsync(user);
        if (createdUser.error !== undefined) {
            return;
        }

        dispatch(updateUser(createdUser.data));

        await createCustomerAsync(createdUser.data.id);
    }

    const createCustomerAsync = async (userId) => {
        customer.appUserId = userId;

        const createdCustomer = await createCustomerMutAsync(customer);
        if (createdCustomer.error !== undefined) {
            return;
        }

        dispatch(updateCustomer(createdCustomer.data));

        navigate("/");
    }

    const getRegistrationContent = () => {
        let content;

        switch (step) {
            case 0:
                content = <ConfidentialRegistrationStep
                    setStep={setStep}
                    user={user}
                    updateConfidentialInformation={updateConfidentialInformation}
                />;
                break;
            case 1:
                content = <GeneralRegistrationStep
                    setStep={setStep}
                    customer={customer}
                    updateGeneralInformation={updateGeneralInformation}
                    registrationAsync={registrationAsync}
                />;
                break;
            default:
                content = <ConfidentialRegistrationStep
                    setStep={setStep}
                    updateConfidentialInformation={updateConfidentialInformation}
                />;
                break;
        }

        setRegistrationContent(content);
    }

    return (
        <div className="registration">
            <div className="registration__title">Registration</div>
            <div className="registration__container">
                <LoginPreview />
                {registrationContent}
            </div>
        </div>
    );
}

export default Registration;