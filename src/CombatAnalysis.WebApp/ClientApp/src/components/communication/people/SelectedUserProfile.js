import { useTranslation } from 'react-i18next';

const SelectedUserProfile = ({ person }) => {
    const { t } = useTranslation("communication/people/user");

    return (
        <ul className="user-information__common-information">
            <li className="user-information-item">
                <div className="user-information-item__title">{t("FirstName")}</div>
                <div className="user-information-item__content">{person?.firstName}</div>
            </li>
            <li className="user-information-item">
                <div className="user-information-item__title">{t("LastName")}</div>
                <div className="user-information-item__content">{person?.lastName}</div>
            </li>
            {person?.aboutMe !== "" &&
                <li className="user-information-item">
                    <div className="user-information-item__title">{t("AboutMe")}</div>
                    <div className="user-information-item__content">{person?.aboutMe}</div>
                </li>
            }
        </ul>
    );
}

export default SelectedUserProfile;