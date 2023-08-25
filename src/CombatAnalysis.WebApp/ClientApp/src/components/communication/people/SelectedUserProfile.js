import { useTranslation } from 'react-i18next';

const SelectedUserProfile = ({ customer }) => {
    const { t } = useTranslation("communication/people/user");

    return (
        <ul className="user-information__common-information">
            <li className="user-information-item">
                <div className="title">{t("Message")}</div>
                <div className="content">{customer?.message}</div>
            </li>
            <li className="user-information-item">
                <div className="title">{t("FirstName")}</div>
                <div className="content">{customer?.firstName}</div>
            </li>
            <li className="user-information-item">
                <div className="title">{t("LastName")}</div>
                <div className="content">{customer?.lastName}</div>
            </li>
            <li className="user-information-item">
                <div className="title">{t("AboutMe")}</div>
                <div className="content">{customer?.aboutMe}</div>
            </li>
        </ul>
    );
}

export default SelectedUserProfile;