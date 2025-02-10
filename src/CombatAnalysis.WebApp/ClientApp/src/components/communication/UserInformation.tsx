import { faCircleXmark, faCommentDots, faPersonCircleQuestion, faSquarePlus, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useChatHub } from '../../context/ChatHubProvider';
import { useLazyIsExistAsyncQuery } from '../../store/api/chat/PersonalChat.api';
import { useFriendSearchMyFriendsQuery } from '../../store/api/user/Friend.api';
import { useCreateRequestAsyncMutation, useLazyRequestIsExistQuery } from '../../store/api/user/RequestToConnect.api';
import { Friend } from '../../types/Friend';
import { UserInformationProps } from '../../types/components/communication/UserInformationProps';
import PeopleInvitesToCommunity from './people/PeopleInvitesToCommunity';
import SelectedUserProfile from './people/SelectedUserProfile';

import './../../styles/communication/userInformation.scss';

const successNotificationTimeout = 2000;
const failedNotificationTimeout = 2000;

const UserInformation: React.FC<UserInformationProps> = ({ me, person, closeUserInformation }) => {
    const { t } = useTranslation("communication/userInformation");

    const { subscribeToPersonalChat, personalChatHubConnection } = useChatHub();

    const navigate = useNavigate();

    const [isExistAsync] = useLazyIsExistAsyncQuery();
    const [createRequestAsync] = useCreateRequestAsyncMutation();
    const [isRequestExistAsync] = useLazyRequestIsExistQuery();

    const [showSuccessNotification, setShowSuccessNotification] = useState(false);
    const [showFailedNotification, setShowFailedNotification] = useState(false);
    const [openInviteToCommunity, setOpenInviteToCommunity] = useState(false);

    const { data: myFriends, isLoading } = useFriendSearchMyFriendsQuery(me?.id);

    const checkExistOfChatsAsync = async (targetUser: any) => {
        const queryParams = {
            userId: me?.id,
            targetUserId: targetUser?.id
        };

        const isExist = await isExistAsync(queryParams);
        return isExist.data !== undefined ? isExist.data : true;
    }

    const createChatAsync = async (targetUser: any) => {
        const isExist = await checkExistOfChatsAsync(targetUser);
        if (isExist) {
            navigate("/chats");
            return;
        }

        subscribeToPersonalChat(() => {
            navigate("/chats");
        });

        await personalChatHubConnection?.invoke("CreateChat", me?.id, targetUser.id);
    }

    const checkIfRequestExistAsync = async (targetUserId: string) => {
        const arg = {
            userId: me?.id,
            targetUserId: targetUserId
        };

        const isExist = await isRequestExistAsync(arg);
        if (isExist.error !== undefined) {
            return true;
        }

        return isExist.data;
    }

    const createRequestToConnectAsync = async (people: any) => {
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
                        title={t("Close") || ""}
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
                            title={t("StartChat") || ""}
                            onClick={async () => await createChatAsync(person)}
                        />
                    </li>
                    <li>
                        {myFriends.filter((friend: Friend) => friend.whoFriendId === person.id || friend.forWhomId === person.id).length > 0
                            ? <FontAwesomeIcon
                                icon={faUserPlus}
                                title={t("AlreadyFriend") || ""}
                                className="user-friend"
                            />
                            : <FontAwesomeIcon
                                icon={faUserPlus}
                                title={t("RequestToConnect") || ""}
                                onClick={async () => await createRequestToConnectAsync(person)}
                            />
                        }
                    </li>
                    <li>
                        <FontAwesomeIcon
                            icon={faSquarePlus}
                            title={t("InviteToCommunity") || ""}
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
                        me={me}
                        targetUser={person}
                        setOpenInviteToCommunity={setOpenInviteToCommunity}
                    />
                </div>
            }
        </div>
    );
}

export default memo(UserInformation);