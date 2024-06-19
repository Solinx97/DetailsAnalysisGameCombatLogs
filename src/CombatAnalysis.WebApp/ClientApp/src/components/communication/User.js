import { faCircleXmark, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useGetCustomerByIdQuery, useLazyGetCustomerByIdQuery } from '../../store/api/Customer.api';
import { useRemoveFriendAsyncMutation } from '../../store/api/communication/myEnvironment/Friend.api';
import UserInformation from './UserInformation';

import "../../styles/communication/user.scss";

const User = ({ me, itIsMe, targetCustomerId, setUserInformation, allowRemoveFriend, actionAfterRequests = null }) => {
    const { t } = useTranslation("communication/myEnvironment/friends");

    const navigate = useNavigate();

    const [removeFriendAsyncMut] = useRemoveFriendAsyncMutation();
    const [getCustomerById] = useLazyGetCustomerByIdQuery();

    const { data: targetCustomer, isLoading } = useGetCustomerByIdQuery(targetCustomerId);

    const [userActive, setUserActive] = useState("");

    const removeFriendAsync = async () => {
        await removeFriendAsyncMut(targetCustomerId);
    }

    const openUserInformationAsync = async () => {
        const targetCustomer = await getCustomerById(targetCustomerId);

        setUserInformation(
            <UserInformation
                me={me}
                people={targetCustomer.data}
                closeUserInformation={closeUserInformation}
                actionAfterRequests={actionAfterRequests}
            />
        );
    }

    const userActiveHandler = (e) => {
        setUserActive("_active");
    }

    const userInactiveHandler = (e) => {
        setUserActive("");
    }

    const closeUserInformation = () => {
        setUserInformation(null);
    }

    const goToUser = () => {
        navigate(`/user?id=${targetCustomerId}`);
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <div className={`special-user__${itIsMe ? "me" : "another"}`}
            onMouseOver={userActiveHandler}
            onMouseLeave={userInactiveHandler}>
            <FontAwesomeIcon
                icon={faUser}
                title={t("ShowDetails")}
                className={`details${userActive}`}
                onClick={async () => await openUserInformationAsync()}
            />
            <div className="username" title={targetCustomer?.username}
                onClick={goToUser}>{targetCustomer?.username}</div>
            {allowRemoveFriend &&
                <FontAwesomeIcon
                    icon={faCircleXmark}
                    title={t("Remove")}
                    className="remove"
                    onClick={async () => await removeFriendAsync()}
                />
            }
        </div>
    );
}

export default User;