import { faCircleXmark, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useGetUserByIdQuery } from '../../store/api/user/Account.api';
import { useRemoveFriendAsyncMutation } from '../../store/api/user/Friend.api';
import { UserProps } from '../../types/components/communication/UserProps';
import UserInformation from './UserInformation';

import "../../styles/communication/user.scss";

const User: React.FC<UserProps> = ({ targetUserId, setUserInformation, allowRemoveFriend, actionAfterRequests = null, friendId = 0 }) => {
    const { t } = useTranslation("communication/myEnvironment/friends");

    const me = useSelector((state: any) => state.user.value);
    const navigate = useNavigate();

    const [removeFriendAsyncMut] = useRemoveFriendAsyncMutation();

    const { data: targetUser, isLoading } = useGetUserByIdQuery(targetUserId);

    const [userActive, setUserActive] = useState("");

    const removeFriendAsync = async () => {
        await removeFriendAsyncMut(friendId);
    }

    const openUserInformation = () => {
        setUserInformation(
            <UserInformation
                me={me}
                person={targetUser}
                closeUserInformation={closeUserInformation}
                actionAfterRequests={actionAfterRequests}
            />
        );
    }

    const userActiveHandler = (e: any) => {
        setUserActive("_active");
    }

    const userInactiveHandler = (e: any) => {
        setUserActive("");
    }

    const closeUserInformation = () => {
        setUserInformation(null);
    }

    const goToUser = () => {
        navigate(`/user?id=${targetUserId}`);

        window.location.reload();
    }

    if (isLoading) {
        return (<div className="special-user__another">Loading...</div>);
    }

    return (
        <div className="special-user__another"
            onMouseOver={userActiveHandler}
            onMouseLeave={userInactiveHandler}>
            <FontAwesomeIcon
                icon={faUser}
                title={t("ShowDetails") || ""}
                className={`details${userActive}`}
                onClick={openUserInformation}
            />
            <div className="username" title={targetUser?.username}
                onClick={goToUser}>{targetUser?.username}</div>
            {allowRemoveFriend &&
                <FontAwesomeIcon
                    icon={faCircleXmark}
                    title={t("Remove") || ""}
                    className="remove"
                    onClick={removeFriendAsync}
                />
            }
        </div>
    );
}

export default User;