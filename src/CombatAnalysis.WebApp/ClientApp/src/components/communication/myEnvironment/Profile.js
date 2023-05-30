import { useEffect, useRef } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { userUpdate } from '../../../features/UserReducer';
import { checkAuth } from '../../../features/AuthenticationReducer';

const Profile = () => {
    const user = useSelector((state) => state.user.value);
    const dispatch = useDispatch();

    const email = useRef(null);
    const currentPassword = useRef(null);
    const password = useRef(null);
    const confirmPassword = useRef(null);

    useEffect(() => {
        getUserInformation();
    }, [])

    const getUserInformation = () => {
        email.current.value = user.email;
    }

    const editUserAsync = async () => {
        let updatesForUser = {
            id: user.id,
            email: user.email,
            password: user.password
        };

        if (email.current.value !== "" && email.current.value !== user.email) {
            updatesForUser.email = email.current.value;
        }

        if (password.current.value !== "" && password.current.value !== user.password) {
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
            dispatch(userUpdate(updatesForUser));
            dispatch(checkAuth(true));
        }
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        await editUserAsync();
    }

    const render = () => {
        return (<div>
            <div>
                <div>Privacy</div>
                <form onSubmit={handleSubmitAsync}>
                    <div className="mb-3">
                        <label htmlFor="inputEmail" className="form-label">Email</label>
                        <input type="email" className="form-control" id="inputEmail" aria-describedby="emailHelp" ref={email} />
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
                    <input type="submit" className="btn btn-primary" value="Save" />
                </form>
            </div>
        </div>);
    }

    return render();
}

export default Profile;