import { faCommentDots, faSquarePlus, faUserPlus, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useCreatePersonalChatAsyncMutation, useLazyIsExistAsyncQuery } from '../../../store/api/PersonalChat.api';
import { useFriendSearchByUserIdQuery } from '../../../store/api/UserApi';
import { useCreateRequestAsyncMutation, useLazySearchByOwnerIdQuery, useLazySearchByToUserIdQuery } from '../../../store/api/RequestToConnect.api';
import UserInformation from './../UserInformation';

const PeopleItem = ({ setUserInformation, people, customer }) => {
    const { t } = useTranslation("communication/people/people");

    const navigate = useNavigate();

    const [isExistAsync] = useLazyIsExistAsyncQuery();
    const [createPersonalChatAsync] = useCreatePersonalChatAsyncMutation();
    const [createRequestAsync] = useCreateRequestAsyncMutation();
    const [sarchByToUserIdAsync] = useLazySearchByToUserIdQuery();
    const [sarchByOwnerIdAsync] = useLazySearchByOwnerIdQuery();

    const { data: isFriend, isLoading } = useFriendSearchByUserIdQuery({
        userId: customer?.id,
        targetUserId: people?.id
    });

    let showUserInformationTimeout = null;

    const checkExistChatAsync = async (targetCustomer) => {
        const queryParams = {
            userId: customer?.id,
            targetUserId: targetCustomer?.id
        };

        const isExist = await isExistAsync(queryParams);
        return isExist.data !== undefined ? isExist.data : true;
    }

    const startChatAsync = async (targetCustomer) => {
        const isExist = await checkExistChatAsync(targetCustomer);
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

    const checkExistRequestAsync = async (id) => {
        let request = await sarchByToUserIdAsync(id);
        if (request.error !== undefined) {
            return true;
        }
        else if (request.data.length > 0) {
            return true;
        }

        request = await sarchByOwnerIdAsync(id);
        if (request.error !== undefined) {
            return true;
        }

        return request.data.length > 0;
    }

    const createRequestToConnectAsync = async (people) => {
        const isExist = await checkExistRequestAsync(people.id);
        if (isExist) {
            return;
        }

        const newRequest = {
            username: customer?.username,
            toUserId: people.id,
            when: new Date(),
            ownerId: customer?.id,
        };

        customer && await createRequestAsync(newRequest);
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

    return (
        <div className="card">
            <div className="card-body">
                <div>[icon]</div>
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
                    {!isFriend &&
                        <FontAwesomeIcon
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
                    />
                </li>
            </ul>
        </div>
    );
}

export default PeopleItem;