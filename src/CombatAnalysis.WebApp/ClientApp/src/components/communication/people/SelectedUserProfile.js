import { useTranslation } from 'react-i18next';

const SelectedUserProfile = ({ customer }) => {
    const { t } = useTranslation("communication/people/user");

    return (
        <ul className="user-information__common-information">
            {customer?.message !== " " &&
                <li className="user-information-item">
                    <div className="user-information-item__title">{t("Message")}</div>
                    <div className="user-information-item__content">{customer?.message}</div>
                </li>
            }
            <li className="user-information-item">
                <div className="user-information-item__title">{t("FirstName")}</div>
                <div className="user-information-item__content">{customer?.firstName}</div>
            </li>
            <li className="user-information-item">
                <div className="user-information-item__title">{t("LastName")}</div>
                <div className="user-information-item__content">{customer?.lastName}</div>
            </li>
            {customer?.aboutMe !== " " &&
                <li className="user-information-item">
                    <div className="user-information-item__title">{t("AboutMe")}</div>
                    <div className="user-information-item__content">{customer?.aboutMe}</div>
                </li>
            }
        </ul>
    );
}

export default SelectedUserProfile;