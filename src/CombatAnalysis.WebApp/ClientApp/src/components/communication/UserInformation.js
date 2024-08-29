import { faCircleXmark, faCommentDots, faPersonCircleQuestion, faSquarePlus, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useCreatePersonalChatAsyncMutation, useLazyIsExistAsyncQuery } from '../../store/api/communication/chats/PersonalChat.api';
import { useCreatePersonalChatMessageCountAsyncMutation } from '../../store/api/communication/chats/PersonalChatMessagCount.api';
import { useFriendSearchMyFriendsQuery } from '../../store/api/communication/myEnvironment/Friend.api';
import { useCreateRequestAsyncMutation, useLazyRequestIsExistQuery } from '../../store/api/communication/myEnvironment/RequestToConnect.api';
import PeopleInvitesToCommunity from './people/PeopleInvitesToCommunity';
import SelectedUserProfile from './people/SelectedUserProfile';

import './../../styles/communication/userInformation.scss';

const successNotificationTimeout = 2000;
const failedNotificationTimeout = 2000;

const UserInformation = ({ me, person, closeUserInformation, actionAfterRequests = null }) => {
    const { t } = useTranslation("communication/userInformation");

    const navigate = useNavigate();

    const [isExistAsync] = useLazyIsExistAsyncQuery();
    const [createPersonalChatAsync] = useCreatePersonalChatAsyncMutation();
    const [createPersonalChatCountAsyncMut] = useCreatePersonalChatMessageCountAsyncMutation();
    const [createRequestAsync] = useCreateRequestAsyncMutation();
    const [isRequestExistAsync] = useLazyRequestIsExistQuery();

    const [showSuccessNotification, setShowSuccessNotification] = useState(false);
    const [showFailedNotification, setShowFailedNotification] = useState(false);
    const [openInviteToCommunity, setOpenInviteToCommunity] = useState(false);

    const { data: myFriends, isLoading } = useFriendSearchMyFriendsQuery(me?.id);

    const checkExistOfChatsAsync = async (targetUserId) => {
        const queryParams = {
            userId: me?.id,
            targetUserId: targetUserId?.id
        };

        const isExist = await isExistAsync(queryParams);
        return isExist.data !== undefined ? isExist.data : true;
    }

    const createChatAsync = async (targetUser) => {
        if (actionAfterRequests !== null) {
            actionAfterRequests();
        }

        const isExist = await checkExistOfChatsAsync(targetUser);
        if (isExist) {
            navigate("/chats");
            return;
        }

        const newChat = {
            initiatorUsername: me?.username,
            companionUsername: targetUser.username,
            lastMessage: " ",
            initiatorId: me?.id,
            companionId: targetUser.id
        };

        const createdChat = await createPersonalChatAsync(newChat);
        if (createdChat.data !== undefined) {
            let countIsCreated = await createPersonalChatCountAsync(createdChat.data.id, me?.id);
            if (!countIsCreated) {
                return;
            }

            countIsCreated = await createPersonalChatCountAsync(createdChat.data.id, targetUser.id);
            if (countIsCreated) {
                navigate("/chats");
            }
        }
    }

    const createPersonalChatCountAsync = async (chatId, userId) => {
        const newMessagesCount = {
            count: 0,
            appUSerId: userId,
            chatId: +chatId,
        };

        const createdMessagesCount = await createPersonalChatCountAsyncMut(newMessagesCount);
        return createdMessagesCount.data !== undefined;
    }

    const checkIfRequestExistAsync = async (id) => {
        const arg = {
            userId: me?.id,
            targetUserId: id
        };

        const isExist = await isRequestExistAsync(arg);
        if (isExist.error !== undefined) {
            return true;
        }

        return isExist.data;
    }

    const createRequestToConnectAsync = async (people) => {
        if (actionAfterRequests) {
            actionAfterRequests();
        }

        const isExist = await checkIfRequestExistAsync(people.id);
        if (isExist) {
            setShowFailedNotification(true);

            setTimeout(() => {
                setShowFailedNotification(false);
            }, failedNotificationTimeout);

            return;
        }

        const newRequest = {
            toAppUserId: people.id,
            when: new Date(),
            appUserId: me?.id,
        };

        const createdRequest = await createRequestAsync(newRequest);
        if (createdRequest.data !== undefined) {
            setShowSuccessNotification(true);

            setTimeout(() => {
                setShowSuccessNotification(false);
            }, successNotificationTimeout);
        }
    }

    const moreDetails = () => {
        if (actionAfterRequests !== null) {
            actionAfterRequests();
        }

        navigate(`/user?id=${person.id}`)
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="user-information">
            <div className={`alert alert-success sent-request${showSuccessNotification ? "_active" : ""}`} role="alert">
                <div>{t("SentRequest")}</div>
                <p>{person.username}</p>
            </div>
            <div className={`alert alert-warning sent-request${showFailedNotification ? "_active" : ""}`} role="alert">
                <div>{t("AlreadySentRequest")}</div>
                <p>{person.username}</p>
            </div>
            <div className="user-information__container box-shadow">
                <div className="user-information__menu">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title={t("Close")}
                        onClick={closeUserInformation}
                    />
                </div>
                <div className="user-information__username">
                    {person.username}
                </div>
                <SelectedUserProfile
                    person={person}
                />
                <ul className="links">
                    <li>
                        <FontAwesomeIcon
                            icon={faCommentDots}
                            title={t("StartChat")}
                            onClick={async () => await createChatAsync(person)}
                        />
                    </li>
                    <li>
                        {myFriends.filter(friend => friend.whoFriendId === person.id || friend.forWhomId === person.id).length > 0
                            ? <FontAwesomeIcon
                                icon={faUserPlus}
                                title={t("AlreadyFriend")}
                                className="user-friend"
                            />
                            : <FontAwesomeIcon
                                icon={faUserPlus}
                                title={t("RequestToConnect")}
                                onClick={async () => await createRequestToConnectAsync(person)}
                            />
                        }
                    </li>
                    <li>
                        <FontAwesomeIcon
                            icon={faSquarePlus}
                            title={t("InviteToCommunity")}
                            onClick={() => setOpenInviteToCommunity((item) => !item)}
                        />
                    </li>
                </ul>
                <div className="details">
                    <div className="btn-shadow" onClick={moreDetails}>
                        <FontAwesomeIcon
                            icon={faPersonCircleQuestion}
                        />
                        <div>{t("MoreDetails")}</div>
                    </div>
                </div>
            </div>
            {openInviteToCommunity &&
                <div className="invite">
                    <PeopleInvitesToCommunity
                        customer={me}
                        setOpenInviteToCommunity={setOpenInviteToCommunity}
                        people={person}
                    />
                </div>
            }
        </div>
    );
}

export default memo(UserInformation);