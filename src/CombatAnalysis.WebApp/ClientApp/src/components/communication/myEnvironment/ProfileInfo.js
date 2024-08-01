import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';

const ProfileInfo = ({ form, setIsEditMode, t }) => {
    const [privacyHidden, setPrivacyHidden] = useState(false);
    const [generalHidden, setGeneralHidden] = useState(false);

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
                <div className="actions">
                    <div className="btn-shadow" onClick={() => setIsEditMode(true)}>{t("Edit")}</div>
                </div>
            </div>
            {!privacyHidden &&
                <div className="privacy">
                    <div className="mb-3">
                        <label htmlFor="inputPhoneNumber" className="form-label">{t("PhoneNumber")}</label>
                        <input type="number" className="form-control" id="inputPhoneNumber" value={form.phoneNumber} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputBithday" className="form-label">{t("Birthday")}</label>
                        <input type="date" className="form-control" id="inputBithday" value={form.birthday} disabled />
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
                        <input type="text" className="form-control" id="inputUsername" value={form.username} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputAboutMe" className="form-label">{t("AboutMe")}</label>
                        <input type="text" className="form-control" id="inputAboutMe" value={form.aboutMe} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inutFirstName" className="form-label">{t("FirstName")}</label>
                        <input type="text" className="form-control" id="inutFirstName" value={form.firstName} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputLastName" className="form-label">{t("LastName")}</label>
                        <input type="text" className="form-control" id="inputLastName" value={form.lastName} disabled />
                    </div>
                </div>
            }
        </div>
    );
}

export default ProfileInfo;