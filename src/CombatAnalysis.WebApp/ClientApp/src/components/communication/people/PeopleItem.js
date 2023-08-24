import { faCommentDots, faSquarePlus, faUserPlus, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useCreatePersonalChatAsyncMutation, useLazyIsExistAsyncQuery } from '../../../store/api/PersonalChat.api';
import { useCreateRequestAsyncMutation, useLazyRequestIsExistQuery } from '../../../store/api/RequestToConnect.api';
import { useFriendSearchByUserIdQuery } from '../../../store/api/UserApi';
import UserInformation from './../UserInformation';
import InviteToCommunity from './InviteToCommunity';

const successNotificationTimeout = 2000;
const failedNotificationTimeout = 2000;
let showUserInformationTimeout = null;

const PeopleItem = ({ setUserInformation, people, customer }) => {
    const { t } = useTranslation("communication/people/people");

    const navigate = useNavigate();

    const [isExistAsync] = useLazyIsExistAsyncQuery();
    const [createPersonalChatAsync] = useCreatePersonalChatAsyncMutation();
    const [createRequestAsync] = useCreateRequestAsyncMutation();
    const [isRequestExistAsync] = useLazyRequestIsExistQuery();

    const [showSuccessNotification, setShowSuccessNotification] = useState(false);
    const [showFailedNotification, setShowFailedNotification] = useState(false);
    const [openInviteToCommunity, setOpenInviteToCommunity] = useState(false);

    const { data: isFriend, isLoading } = useFriendSearchByUserIdQuery({
        userId: customer?.id,
        targetUserId: people?.id
    });

    const checkIfChatExistAsync = async (targetCustomer) => {
        const queryParams = {
            userId: customer?.id,
            targetUserId: targetCustomer?.id
        };

        const isExist = await isExistAsync(queryParams);
        return isExist.data !== undefined ? isExist.data : true;
    }

    const startChatAsync = async (targetCustomer) => {
        const isExist = await checkIfChatExistAsync(targetCustomer);
        if (isExist) {
            navigate("/chats");
            return;
        }

        const newChat = {
            initiatorUsername: customer?.username,
            companionUsername: targetCustomer.username,
            lastMessage: " ",
            initiatorId: customer?.id,
            companionId: targetCustomer.id
        };

        const createdChat = await createPersonalChatAsync(newChat);
        if (createdChat.data !== undefined) {
            navigate("/chats");
        }
    }

    const checkIfRequestExistAsync = async (id) => {
        const arg = {
            userId: customer?.id,
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
            username: customer?.username,
            toUserId: people.id,
            when: new Date(),
            ownerId: customer?.id,
        };

        const createdRequest = await createRequestAsync(newRequest);
        if (createdRequest.data !== undefined) {
            setShowSuccessNotification(true);

            setTimeout(() => {
                setShowSuccessNotification(false);
            }, successNotificationTimeout);
        }
    }

    const openUserInformationWithTimeout = (targetCustomer) => {
        showUserInformationTimeout = setTimeout(() => {
            setUserInformation(<UserInformation customer={targetCustomer} closeUserInformation={closeUserInformation} />);
        }, 1000);
    }

    const openUserInformation = (targetCustomer) => {
        setUserInformation(<UserInformation customer={targetCustomer} closeUserInformation={closeUserInformation} />);
    }

    const clearUserInformationTimeout = () => {
        clearInterval(showUserInformationTimeout);
    }

    const closeUserInformation = () => {
        setUserInformation(<></>);
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div>
            <div className={`alert alert-success sent-request${showSuccessNotification ? "_active" : ""}`} role="alert">
                <div>{t("SentRequest")}</div>
                <p>{people.username}</p>
            </div>
            <div className={`alert alert-warning sent-request${showFailedNotification ? "_active" : ""}`} role="alert">
                <div>{t("AlreadySentRequest")}</div>
                <p>{people.username}</p>
            </div>
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
                <ul className="card__links list-group list-group-flush">
                    <li className="list-group-item">
                        <FontAwesomeIcon
                            icon={faCommentDots}
                            title={t("StartChat")}
                            onClick={async () => await startChatAsync(people)}
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
            </div>
            {openInviteToCommunity &&
                <InviteToCommunity
                    customer={customer}
                    setOpenInviteToCommunity={setOpenInviteToCommunity}
                    people={people}
                />
            }
        </div>
    );
}

export default PeopleItem;