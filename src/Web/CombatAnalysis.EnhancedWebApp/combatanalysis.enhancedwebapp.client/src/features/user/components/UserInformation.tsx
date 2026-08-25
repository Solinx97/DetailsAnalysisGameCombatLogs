import type { RootState } from '@/app/Store';
import { faCircleXmark, faCommentDots, faPersonCircleQuestion, faSquarePlus, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useChatHub } from '../../../shared/hooks/useChatHub';
import { useLazyIsExistQuery } from '../../chat/api/PersonalChat.api';
import { useGetUserByIdQuery } from '../api/Account.api';
import { useFindFriendByUserIdQuery } from '../api/Friend.api';
import { useCreateRequestAsyncMutation, useLazyRequestIsExistQuery } from '../api/RequestToConnect.api';
import type { AppUserModel } from '../types/AppUserModel';
import type { FriendModel } from '../types/FriendModel';
import type { RequestToConnectModel } from '../types/RequestToConnectModel';
import PeopleInvitesToCommunity from './people/PeopleInvitesToCommunity';
import SelectedUserProfile from './selectedUser/SelectedUserProfile';

import './UserInformation.scss';

const successNotificationTimeout = 2000;
const failedNotificationTimeout = 2000;

interface UserInformationProps {
    personId: string;
    closeUserInformation(): void;
}

const UserInformation: React.FC<UserInformationProps> = ({ personId, closeUserInformation }) => {
    const { t } = useTranslation("communication/userInformation");

    const chatHub = useChatHub();

    const navigate = useNavigate();

    const myself = useSelector((state: RootState) => state.user.value);

    const { data: person, isLoading: personIsLoading } = useGetUserByIdQuery(personId);

    const [isExistAsync] = useLazyIsExistQuery();
    const [createRequestAsync] = useCreateRequestAsyncMutation();
    const [isRequestExistAsync] = useLazyRequestIsExistQuery();

    const [showSuccessNotification, setShowSuccessNotification] = useState(false);
    const [showFailedNotification, setShowFailedNotification] = useState(false);
    const [openInviteToCommunity, setOpenInviteToCommunity] = useState(false);

    const { data: myFriends, isLoading } = useFindFriendByUserIdQuery(myself?.id ?? "");

    const checkExistOfChatsAsync = async (targetUser: AppUserModel) => {
        if (!myself) {
            return;
        }

        const isExist = await isExistAsync({ userId: myself?.id, targetUserId: targetUser?.id });
        return isExist.data !== undefined ? isExist.data : true;
    }

    const createChatAsync = async (targetUser: AppUserModel) => {
        if (!chatHub) {
            return;
        }

        const isExist = await checkExistOfChatsAsync(targetUser);
        if (isExist) {
            navigate("/chats");
            return;
        }

        if (!chatHub.personalChatHubConnectionRef.current) {
            await chatHub.connectToPersonalChatAsync();
        }

        chatHub.subscribeToPersonalChat(() => {
            navigate("/chats");
        });

        await chatHub.personalChatHubConnectionRef.current?.invoke("CreateChat", myself?.id, targetUser.id);
    }

    const checkIfRequestExistAsync = async (targetUserId: string) => {
        if (!myself) {
            return;
        }

        const isExist = await isRequestExistAsync({ userId: myself.id, targetUserId: targetUserId });
        if (isExist.error !== undefined) {
            return true;
        }

        return isExist.data;
    }

    const createRequestToConnectAsync = async (people: AppUserModel) => {
        const isExist = await checkIfRequestExistAsync(people.id);
        if (isExist || !myself) {
            setShowFailedNotification(true);

            setTimeout(() => {
                setShowFailedNotification(false);
            }, failedNotificationTimeout);

            return;
        }

        const newRequest: RequestToConnectModel = {
            id: 0,
            toAppUserId: people.id,
            when: new Date(),
            appUserId: myself.id,
        };

        const createdRequest = await createRequestAsync(newRequest).unwrap();
        if (createdRequest) {
            setShowSuccessNotification(true);

            setTimeout(() => {
                setShowSuccessNotification(false);
            }, successNotificationTimeout);
        }
    }

    const moreDetails = () => {
        navigate(`/people/user?id=${person?.id}`);
    }

    const isFriend = () => {
        if (!myFriends || !person) {
            return false;
        }

        const isFriend = myFriends.filter((friend: FriendModel) => friend.whoFriendId === person.id || friend.forWhomId === person.id).length > 0;

        return isFriend;
    }

    if (isLoading || personIsLoading || !myFriends || !person) {
        return (<></>);
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
                    <li title={t("StartChat") || ""}>
                        <FontAwesomeIcon
                            icon={faCommentDots}
                            onClick={async () => await createChatAsync(person)}
                        />
                    </li>
                    <li title={(isFriend() ? t("AlreadyFriend") : t("RequestToConnect"))|| ""}>
                        <FontAwesomeIcon
                            icon={isFriend() ? faUserPlus : faUserPlus}
                            className={`${isFriend() ? "user-friend" : ""}`}
                            onClick={isFriend() ? () => {} : async () => await createRequestToConnectAsync(person)}
                        />
                    </li>
                    <li title={t("InviteToCommunity") || ""}>
                        <FontAwesomeIcon
                            icon={faSquarePlus}
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
                        myself={myself}
                        targetUser={person}
                        setOpenInviteToCommunity={setOpenInviteToCommunity}
                    />
                </div>
            }
        </div>
    );
}

export default memo(UserInformation);
