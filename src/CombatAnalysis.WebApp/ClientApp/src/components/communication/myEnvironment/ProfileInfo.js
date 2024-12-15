import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useSelector } from 'react-redux';
import { useLazyVerifyEmailQuery } from '../../../store/api/core/User.api';

const ProfileInfo = ({ setIsEditMode, getDate, t }) => {
    const privacy = useSelector((state) => state.userPrivacy.value);
    const user = useSelector((state) => state.user.value);

    const [privacyHidden, setPrivacyHidden] = useState(false);
    const [generalHidden, setGeneralHidden] = useState(false);

    const [verifyEmailAsync] = useLazyVerifyEmailQuery();

    const goToVerifyEmailAsync = async () => {
        const identityServerVerifyEmailPath = process.env.REACT_APP_IDENTITY_SERVER_VERIFY_EMAIL_PATH;

        const response = await verifyEmailAsync({ identityPath: identityServerVerifyEmailPath, email: privacy.email });

        if (response.data !== undefined) {
            const uri = response.data.uri;
            window.location.href = uri;
        }
    }

    if (!privacy || !user) {
        return (<div>Loading...</div>);
    }

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
                        <label className="form-label">{t("Email")}</label>
                        <div className="privacy__email-container">
                            {privacy.emailVerified
                                ? <div className="verified-email">{privacy.email}</div>
                                : <>
                                    <div className="email">{privacy.email}</div>
                                    <div className="verification" onClick={goToVerifyEmailAsync}>{t("VerifyAccount")}</div>
                                </>
                            }
                        </div>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputPhoneNumber" className="form-label">{t("PhoneNumber")}</label>
                        <input type="number" className="form-control" id="inputPhoneNumber" value={user.phoneNumber} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputBithday" className="form-label">{t("Birthday")}</label>
                        <input type="date" className="form-control" id="inputBithday" value={getDate(user)} disabled />
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
                        <input type="text" className="form-control" id="inputUsername" value={user.username} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputAboutMe" className="form-label">{t("AboutMe")}</label>
                        <input type="text" className="form-control" id="inputAboutMe" value={user.aboutMe} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inutFirstName" className="form-label">{t("FirstName")}</label>
                        <input type="text" className="form-control" id="inutFirstName" value={user.firstName} disabled />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="inputLastName" className="form-label">{t("LastName")}</label>
                        <input type="text" className="form-control" id="inputLastName" value={user.lastName} disabled />
                    </div>
                </div>
            }
        </div>
    );
}

export default ProfileInfo;