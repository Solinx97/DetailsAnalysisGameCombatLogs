import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useDispatch } from 'react-redux';
import { useEditAsyncMutation } from '../../../store/api/Account.api';
import { updateUser } from '../../../store/slicers/UserSlice';

const ProfileEdit = ({ form, setForm, t, setIsEditMode }) => {
    const dispatch = useDispatch();

    const [privacyHidden, setPrivacyHidden] = useState(false);
    const [generalHidden, setGeneralHidden] = useState(false);

    const [editUserAsyncMut] = useEditAsyncMutation();

    const updateUserAsync = async () => {
        const updatedUser = await editUserAsyncMut(form);
        if (updatedUser.data !== undefined) {
            dispatch(updateUser(updatedUser.data));
        }

        setIsEditMode(false);
    }

    const handleForm = (e) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value
        });
    }

    const handleSubmitAsync = async (event) => {
        event.preventDefault();

        await updateUserAsync();
    }

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
                        <input type="number" className="form-control" id="inputPhoneNumber" name="phoneNumber" onChange={handleForm} value={form.phoneNumber} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputBithdayl" className="form-label">{t("Birthday")}</label>
                        <input type="date" className="form-control" id="inputBithdayl" name="birthday" onChange={handleForm} value={form.birthday} />
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
                        <label htmlFor="inputUsername" className="form-label">{t("Username")}</label>
                        <input type="text" className="form-control" id="inputUsername" name="username" onChange={handleForm} value={form.username} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputAboutMe" className="form-label">{t("AboutMe")}</label>
                        <input type="text" className="form-control" id="inputAboutMe" name="aboutMe" onChange={handleForm} value={form.aboutMe} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inutFirstName" className="form-label">{t("FirstName")}</label>
                        <input type="text" className="form-control" id="inutFirstName" name="firstName" onChange={handleForm} value={form.firstName} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputLastName" className="form-label">{t("LastName")}</label>
                        <input type="text" className="form-control" id="inputLastName" name="lastName" onChange={handleForm} value={form.lastName} />
                    </div>
                </div>
            }
            <div className="actions">
                <div className="btn-shadow save" onClick={handleSubmitAsync}>{t("Save")}</div>
                <div className="btn-shadow" onClick={() => setIsEditMode(false)}>{t("Cancel")}</div>
            </div>
        </form>
    );
}

export default ProfileEdit;