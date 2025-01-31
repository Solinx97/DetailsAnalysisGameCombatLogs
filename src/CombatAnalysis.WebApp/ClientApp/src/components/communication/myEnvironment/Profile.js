import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import ProfileEdit from './ProfileEdit';
import ProfileInfo from './ProfileInfo';

import "../../../styles/communication/myEnvironment/profile.scss";

const Profile = () => {
    const { t } = useTranslation("communication/myEnvironment/profile");

    const [isEditMode, setIsEditMode] = useState(false);

    const getDate = (user) => {
        const date = new Date(user.birthday);
        const correntMonthNumber = date.getMonth() === 0 ? 1 : date.getMonth() + 1;
        const month = correntMonthNumber < 10 ? `0${correntMonthNumber}` : correntMonthNumber;
        const day = date.getDate() < 10 ? `0${date.getDate()}` : date.getDate();

        const birthdayFromDate = `${date.getFullYear()}-${month}-${day}`;

        return birthdayFromDate;
    }

    return (
        <div className="profile">
            {isEditMode
                ? <ProfileEdit
                    setIsEditMode={setIsEditMode}
                    t={t}
                    getDate={getDate}
                />
                : <ProfileInfo
                    setIsEditMode={setIsEditMode}
                    getDate={getDate}
                    t={t}
                />
            }
        </div>
    );
}

export default Profile;