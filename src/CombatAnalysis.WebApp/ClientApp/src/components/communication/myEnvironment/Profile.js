import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import ProfileEdit from './ProfileEdit';
import ProfileInfo from './ProfileInfo';

import "../../../styles/communication/myEnvironment/profile.scss";

const Profile = () => {
    const { t } = useTranslation("communication/myEnvironment/profile");

    const user = useSelector((state) => state.user.value);

    const [isEditMode, setIsEditMode] = useState(false);

    const [form, setForm] = useState({
        id: "",
        phoneNumber: 0,
        birthday: new Date(),
        username: "",
        aboutMe: "",
        firstName: "",
        lastName: "",
        identityUserId: "",
    });

    useEffect(() => {
        const birthdayFromDate = getDate();

        setForm({
            id: user?.id,
            phoneNumber: user?.phoneNumber,
            birthday: birthdayFromDate,
            username: user?.username,
            aboutMe: user?.aboutMe,
            firstName: user?.firstName,
            lastName: user?.lastName,
            identityUserId: user?.identityUserId,
        });
    }, [user]);

    const getDate = () => {
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
                    form={form}
                    setForm={setForm}
                    t={t}
                    setIsEditMode={setIsEditMode }
                />
                : <ProfileInfo
                    form={form}
                    setIsEditMode={setIsEditMode}
                    t={t}
                />
            }
        </div>
    );
}

export default Profile;