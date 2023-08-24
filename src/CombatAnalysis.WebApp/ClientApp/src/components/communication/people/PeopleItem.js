import { faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import UserInformation from './../UserInformation';

let showUserInformationTimeout = null;
const showUserInformationTime = 1000;

const PeopleItem = ({ people, customer }) => {
    const { t } = useTranslation("communication/people/people");

    const [userInformation, setUserInformation] = useState(null);

    const openUserInformationWithTimeout = (targetCustomer) => {
        showUserInformationTimeout = setTimeout(() => {
            setUserInformation(
                <UserInformation
                    customer={customer}
                    people={targetCustomer}
                    closeUserInformation={closeUserInformation}
                />
            );
        }, showUserInformationTime);
    }

    const openUserInformation = (targetCustomer) => {
        setUserInformation(
            <UserInformation
                customer={customer}
                people={targetCustomer}
                closeUserInformation={closeUserInformation}
            />
        );
    }

    const clearUserInformationTimeout = () => {
        clearInterval(showUserInformationTimeout);
    }

    const closeUserInformation = () => {
        setUserInformation(null);
    }

    return (
        <div>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title" onMouseOver={() => openUserInformationWithTimeout(people)}
                        onMouseLeave={() => clearUserInformationTimeout()}>{people.username}</h5>
                    <FontAwesomeIcon
                        icon={faWindowRestore}
                        title={t("ShowDetails")}
                        onClick={() => openUserInformation(people)}
                    />
                </div>
            </div>
            {userInformation}
        </div>
    );
}

export default PeopleItem;