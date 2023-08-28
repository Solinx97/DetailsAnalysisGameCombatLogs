import { faCircleXmark, faCommentDots, faSquarePlus, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { NavLink, useNavigate } from 'react-router-dom';
import { useCreatePersonalChatAsyncMutation, useLazyIsExistAsyncQuery } from '../../store/api/PersonalChat.api';
import { useCreatePersonalChatMessageCountAsyncMutation } from '../../store/api/PersonalChatMessagCount.api';
import { useCreateRequestAsyncMutation, useLazyRequestIsExistQuery } from '../../store/api/RequestToConnect.api';
import { useFriendSearchByUserIdQuery } from '../../store/api/UserApi';
import PeopleInvitesToCommunity from './people/PeopleInvitesToCommunity';

import './../../styles/communication/userInformation.scss';

const successNotificationTimeout = 2000;
const failedNotificationTimeout = 2000;

const UserInformation = ({ me, people, closeUserInformation }) => {
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

    const { data: isFriend, isLoading } = useFriendSearchByUserIdQuery({
        userId: me?.id,
        targetUserId: people?.id
    });

    const checkExistOfChatsAsync = async (targetCustomer) => {
        const queryParams = {
            userId: me?.id,
            targetUserId: targetCustomer?.id
        };

        const isExist = await isExistAsync(queryParams);
        return isExist.data !== undefined ? isExist.data : true;
    }

    const createChatAsync = async (targetCustomer) => {
        const isExist = await checkExistOfChatsAsync(targetCustomer);
        if (isExist) {
            navigate("/chats");
            return;
        }

        const newChat = {
            initiatorUsername: me?.username,
            companionUsername: targetCustomer.username,
            lastMessage: " ",
            initiatorId: me?.id,
            companionId: targetCustomer.id
        };

        const createdChat = await createPersonalChatAsync(newChat);
        if (createdChat.data !== undefined) {
            let countIsCreated = await createPersonalChatCountAsync(createdChat.data.id, me?.id);
            if (!countIsCreated) {
                return;
            }

            countIsCreated = await createPersonalChatCountAsync(createdChat.data.id, targetCustomer.id);
            if (countIsCreated) {
                navigate("/chats");
            }
        }
    }

    const createPersonalChatCountAsync = async (chatId, userId) => {
        const newMessagesCount = {
            count: 0,
            userId: userId,
            personalChatId: +chatId,
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
        const isExist = await checkIfRequestExistAsync(people.id);
        if (isExist) {
            setShowFailedNotification(true);

            setTimeout(() => {
                setShowFailedNotification(false);
            }, failedNotificationTimeout);

            return;
        }

        const newRequest = {
            username: me?.username,
            toUserId: people.id,
            when: new Date(),
            ownerId: me?.id,
        };

        const createdRequest = await createRequestAsync(newRequest);
        if (createdRequest.data !== undefined) {
            setShowSuccessNotification(true);

            setTimeout(() => {
                setShowSuccessNotification(false);
            }, successNotificationTimeout);
        }
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="user-information">
            <div className={`alert alert-success sent-request${showSuccessNotification ? "_active" : ""}`} role="alert">
                <div>{t("SentRequest")}</div>
                <p>{people.username}</p>
            </div>
            <div className={`alert alert-warning sent-request${showFailedNotification ? "_active" : ""}`} role="alert">
                <div>{t("AlreadySentRequest")}</div>
                <p>{people.username}</p>
            </div>
            <div className="user-information__container">
                <div className="user-information__menu">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title={t("Close")}
                        onClick={closeUserInformation}
                    />
                </div>
                <div className="user-information__username">
                    {people.username}
                </div>
                <ul className="user-information__common-information">
                    <li className="user-information-item">
                        <div className="title">{t("FirstName")}</div>
                        <div className="content">{people.firstName}</div>
                    </li>
                    <li className="user-information-item">
                        <div className="title">{t("LastName")}</div>
                        <div className="content">{people.lastName}</div>
                    </li>
                    <li className="user-information-item">
                        <div className="title">{t("AboutMe")}</div>
                        <div className="content">{people.aboutMe}</div>
                    </li>
                </ul>
                <ul className="links list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faCommentDots}
                            title={t("StartChat")}
                            onClick={async () => await createChatAsync(people)}
                        />
                    </li>
                    <li className="list-group-item">
                        {isFriend
                            ? <FontAwesomeIcon
                                icon={faUserPlus}
                                title={t("AlreadyFriend")}
                                className="user-friend"
                            />
                            : <FontAwesomeIcon
                                icon={faUserPlus}
                                title={t("RequestToConnect")}
                                onClick={async () => await createRequestToConnectAsync(people)}
                            />
                        }
                    </li>
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faSquarePlus}
                            title={t("InviteToCommunity")}
                            onClick={() => setOpenInviteToCommunity((item) => !item)}
                        />
                    </li>
                </ul>
                <div className="details">
                    <NavLink  to={`/user?id=${people.id}`}>{t("MoreDetails")}</NavLink>
                </div>
            </div>
            {openInviteToCommunity &&
                <div className="invite">
                    <PeopleInvitesToCommunity
                        customer={me}
                        setOpenInviteToCommunity={setOpenInviteToCommunity}
                        people={people}
                    />
                </div>
            }
        </div>
    );
}

export default memo(UserInformation);