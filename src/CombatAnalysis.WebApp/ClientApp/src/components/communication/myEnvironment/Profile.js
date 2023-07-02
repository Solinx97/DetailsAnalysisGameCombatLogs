import { useEffect, useRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { checkAuth } from '../../../features/AuthenticationReducer';
import { customerUpdate } from '../../../features/CustomerReducer';
import { userUpdate } from '../../../features/UserReducer';

import "../../../styles/communication/profile.scss";

const Profile = () => {
    const user = useSelector((state) => state.user.value);
    const customer = useSelector((state) => state.customer.value);
    const dispatch = useDispatch();

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
    }, [])

    useEffect(() => {
        getUserInformation();
        getCustomerInformation();
    }, [isEditMode])

    const getUserInformation = () => {
        const date = new Date(user.birthday);
        const correntMonthNumber = date.getMonth() === 0 ? 1 : date.getMonth();
        const month = correntMonthNumber < 10 ? `0${correntMonthNumber}` : correntMonthNumber;
        const day = date.getDate() < 10 ? `0${date.getDate()}` : date.getDate();

        let birthdayFromDate = `${date.getFullYear()}-${month}-${day}`;

        email.current.value = user.email;
        phoneNumber.current.value = user.phoneNumber;
        birthday.current.value = birthdayFromDate;
    }

    const getCustomerInformation = () => {
        email.current.value = user.email;
        message.current.value = customer.message;
        username.current.value = customer.username;
        aboutMe.current.value = customer.aboutMe;
        firstName.current.value = customer.firstName;
        lastName.current.value = customer.lastName;
        gender.current.value = customer.gender;
    }

    const editUserAsync = async () => {
        let updatesForUser = {
            id: user.id,
            email: user.email,
            phoneNumber: user.phoneNumber,
            birthday: user.birthday,
            password: user.password
        };

        updatesForUser.email = email.current.value;
        updatesForUser.phoneNumber = phoneNumber.current.value;
        updatesForUser.birthday = birthday.current.value;

        if (password.current.value !== "" && password.current.value !== user.password
            && password.current.value === confirmPassword.current.value) {
            updatesForUser.password = password.current.value;
        }

        const response = await fetch('api/v1/Account', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatesForUser)
        });

        if (response.status === 200) {
            await editCustomerAsync();

            dispatch(userUpdate(updatesForUser));
            dispatch(checkAuth(true));
        }
    }

    const editCustomerAsync = async () => {
        let updatesForCustomer = {
            id: customer.id,
            message: customer.message,
            username: customer.username,
            aboutMe: customer.aboutMe,
            firstName: customer.firstName,
            lastName: customer.lastName,
            gender: +customer.gender,
            appUserId: user.id
        };

        updatesForCustomer.message = message.current.value;
        updatesForCustomer.username = username.current.value;
        updatesForCustomer.aboutMe = aboutMe.current.value;
        updatesForCustomer.firstName = firstName.current.value;
        updatesForCustomer.lastName = lastName.current.value;
        updatesForCustomer.gender = gender.current.value;

        const response = await fetch('api/v1/Customer', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatesForCustomer)
        });

        if (response.status === 200) {
            dispatch(customerUpdate(updatesForCustomer));
        }
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        await editUserAsync();
    }

    const editForm = () => {
        return (<form onSubmit={handleSubmitAsync}>
            <div>Privacy</div>
            <div className="privacy">
                <div className="mb-3">
                    <label htmlFor="inputEmail" className="form-label">Email</label>
                    <input type="email" className="form-control" id="inputEmail" aria-describedby="emailHelp" ref={email} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputPhoneNumber" className="form-label">Phone number</label>
                    <input type="number" className="form-control" id="inputPhoneNumber" ref={phoneNumber} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputBithdayl" className="form-label">Birthday</label>
                    <input type="date" className="form-control" id="inputBithdayl" ref={birthday} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputCurrentPassword" className="form-label">Current password</label>
                    <input type="password" className="form-control" id="inputCurrentPassword" ref={currentPassword} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputPassword" className="form-label">New password</label>
                    <input type="password" className="form-control" id="inputPassword" ref={password} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputConfirmPassword" className="form-label">Confirm new password</label>
                    <input type="password" className="form-control" id="inputConfirmPassword" ref={confirmPassword} />
                </div>
            </div>
            <div>General</div>
            <div className="general">
                <div className="mb-3">
                    <label htmlFor="inputMessage" className="form-label">Message</label>
                    <input type="text" className="form-control" id="inputMessage" ref={message} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputUsername" className="form-label">Username</label>
                    <input type="text" className="form-control" id="inputUsername" ref={username} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputAboutMe" className="form-label">About me</label>
                    <input type="text" className="form-control" id="inputAboutMe" ref={aboutMe} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inutFirstName" className="form-label">First name</label>
                    <input type="text" className="form-control" id="inutFirstName" ref={firstName} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputLastName" className="form-label">Last name</label>
                    <input type="text" className="form-control" id="inputLastName" ref={lastName} />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputGender" className="form-label">Gender</label>
                    <input type="number" className="form-control" id="inputGender" ref={gender} />
                </div>
            </div>
            <input type="submit" className="btn btn-outline-primary" value="Save" />
            <input type="button" className="btn btn-outline-warning" value="Cancel" onClick={() => setIsEditMode(false)} />
        </form>);
    }

    const information = () => {
        return (<div>
            <div>Privacy</div>
            <div className="privacy">
                <div className="mb-3">
                    <label htmlFor="inputEmail" className="form-label">Email</label>
                    <input type="email" className="form-control" id="inputEmail" aria-describedby="emailHelp" ref={email} disabled/>
                </div>
                <div className="mb-3">
                    <label htmlFor="inputPhoneNumber" className="form-label">Phone number</label>
                    <input type="number" className="form-control" id="inputPhoneNumber" ref={phoneNumber} disabled />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputBithdayl" className="form-label">Birthday</label>
                    <input type="date" className="form-control" id="inputBithdayl" ref={birthday} disabled />
                </div>
            </div>
            <div>General</div>
            <div className="general">
                <div className="mb-3">
                    <label htmlFor="inputMessage" className="form-label">Message</label>
                    <input type="text" className="form-control" id="inputMessage" ref={message} disabled />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputUsername" className="form-label">Username</label>
                    <input type="text" className="form-control" id="inputUsername" ref={username} disabled />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputAboutMe" className="form-label">About me</label>
                    <input type="text" className="form-control" id="inputAboutMe" ref={aboutMe} disabled />
                </div>
                <div className="mb-3">
                    <label htmlFor="inutFirstName" className="form-label">First name</label>
                    <input type="text" className="form-control" id="inutFirstName" ref={firstName} disabled />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputLastName" className="form-label">Last name</label>
                    <input type="text" className="form-control" id="inputLastName" ref={lastName} disabled />
                </div>
                <div className="mb-3">
                    <label htmlFor="inputGender" className="form-label">Gender</label>
                    <input type="number" className="form-control" id="inputGender" ref={gender} disabled />
                </div>
            </div>
            <input type="button" className="btn btn-outline-primary" value="Edit" onClick={() => setIsEditMode(true)} />
        </div>);
    }

    const render = () => {
        return (<div className="profile">
            {isEditMode
                ? editForm()
                : information()
            }
        </div>);
    }

    return render();
}

export default Profile;