import { faCommentDots, faSquarePlus, faUserPlus, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useCallback, useState } from "react";
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import { useCreatePersonalChatAsyncMutation, useLazyIsExistAsyncQuery } from '../../../store/api/PersonalChat.api';
import { useCreateRequestAsyncMutation } from '../../../store/api/RequestToConnect.api';
import { useGetCustomersQuery } from '../../../store/api/UserApi';
import UserInformation from './../UserInformation';

import '../../../styles/communication/people.scss';

const People = ({ updateCurrentMenuItem }) => {
    const [, customer] = useAuthentificationAsync();

    const { data: people, isLoading } = useGetCustomersQuery();
    const [isExistAsync] = useLazyIsExistAsyncQuery();
    const [createPersonalChatAsync] = useCreatePersonalChatAsyncMutation();
    const [createRequestAsync] = useCreateRequestAsyncMutation();

    const [userInformation, setUserInformation] = useState(<></>);

    let showUserInformationTimeout = null;

    const checkExistNewChatAsync = async (targetCustomer) => {
        const isExist = await isExistAsync(customer?.id, targetCustomer.id);
        return isExist.data !== undefined ? isExist : true;
    }

    const startChatAsync = async (targetCustomer) => {
        const isExist = await checkExistNewChatAsync(targetCustomer);
        if (isExist.data) {
            updateCurrentMenuItem(1, customer?.id, targetCustomer.id);
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
            updateCurrentMenuItem(1, customer?.id, targetCustomer.id);
        }
    }

    const createRequestToConnectAsync = async (people) => {
        const newRequest = {
            username: customer?.username,
            toUserId: people.id,
            when: new Date(),
            ownerId: customer?.id,
        };

        customer && await createRequestAsync(newRequest);
    }

    const peopleListFilter = useCallback((value) => {
        if (value.id !== customer?.id) {
            return value;
        }
    }, [])

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
        <div className="people">
            <div>
                <div>People</div>
            </div>
            <ul className="people__cards">
                {
                    people.filter(peopleListFilter).map((item) => (
                        <li key={item.id}>
                            <div className="card">
                                <div className="card-body">
                                    <div>[icon]</div>
                                    <h5 className="card-title" onMouseOver={() => openUserInformationWithTimeout(item)}
                                        onMouseLeave={() => clearUserInformationTimeout()}>{item.username}</h5>
                                    <FontAwesomeIcon icon={faWindowRestore} title="Show details" onClick={() => openUserInformation(item)} />
                                </div>
                                <ul className="card__links list-group list-group-flush">
                                    <li className="list-group-item"><FontAwesomeIcon icon={faCommentDots} title="Start chat" onClick={async () => await startChatAsync(item)} /></li>
                                    <li className="list-group-item"><FontAwesomeIcon icon={faUserPlus} title="Request to connect" onClick={async () => await createRequestToConnectAsync(item)} /></li>
                                    <li className="list-group-item"><FontAwesomeIcon icon={faSquarePlus} title="Add to community" /></li>
                                </ul>
                            </div>
                        </li>
                    ))
                }
            </ul>
            {userInformation}
        </div>
    );
}

export default People;