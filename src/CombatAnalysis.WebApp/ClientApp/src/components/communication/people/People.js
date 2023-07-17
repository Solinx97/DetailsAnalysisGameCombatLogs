import { faCommentDots, faSquarePlus, faUserPlus, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useCallback, useEffect, useState } from "react";
import { useSelector } from 'react-redux';
import CustomerService from '../../../services/CustomerService';
import PersonalChatService from '../../../services/PersonalChatService';
import RequestToConnectService from '../../../services/RequestToConnectService';
import UserInformation from './../UserInformation';

import '../../../styles/communication/people.scss';

const People = ({ updateCurrentMenuItem }) => {
    const customerService = new CustomerService();
    const personalChatService = new PersonalChatService();
    const requestToConnectService = new RequestToConnectService();

    const customer = useSelector((state) => state.customer.value);

    const timeForHideNotifications = 4000;

    const [people, setPeople] = useState(<></>);
    const [userInformation, setUserInformation] = useState(<></>);
    const [showSuccessfullRequest, setShowSuccessfullRequest] = useState(false);

    let showUserInformationTimeout = null;

    useEffect(() => {
        let getPeople = async () => {
            await getPeopleAsync();
        }

        getPeople();
    }, [])

    const getPeopleAsync = async () => {
        const people = await customerService.getAllAsync();
        if (people !== null) {
            fillPeopleCards(people);
        }
    }

    const checkExistNewChatAsync = async (targetCustomer) => {
        const isExist = await personalChatService.isExistAsync(customer.id, targetCustomer.id);
        return isExist !== null ? isExist : true;
    }

    const startChatAsync = async (targetCustomer) => {
        const isExist = await checkExistNewChatAsync(targetCustomer);
        if (isExist) {
            updateCurrentMenuItem(0, customer.id, targetCustomer.id);
            return;
        }

        const newChat = {
            id: 0,
            initiatorUsername: customer.username,
            companionUsername: targetCustomer.username,
            lastMessage: " ",
            initiatorId: customer.id,
            companionId: targetCustomer.id
        };

        const createdChat = await personalChatService.createAsync(newChat);
        if (createdChat !== null) {
            updateCurrentMenuItem(0, customer.id, targetCustomer.id);
        }
    }

    const createRequestToConnectAsync = async (people) => {
        setShowSuccessfullRequest(false);

        const newRequest = {
            id: 0,
            username: customer.username,
            toUserId: people.id,
            when: new Date(),
            ownerId: customer.id,
        };

        const createdRequest = await requestToConnectService.createAsync(newRequest);
        if (createdRequest !== null) {
            setShowSuccessfullRequest(true);

            setTimeout(() => {
                setShowSuccessfullRequest(false);
            }, timeForHideNotifications);
        }
    }

    const fillPeopleCards = (people) => {
        const list = people.filter(peopleListFilter).map((element) => createPersonalCard(element));
        
        setPeople(list);
    }

    const peopleListFilter = useCallback((value) => {
        if (value.id !== customer.id) {
            return value;
        }
    }, [])

    const openUserInformationWithTimeout = (customer) => {
        showUserInformationTimeout = setTimeout(() => {
            setUserInformation(<UserInformation customer={customer} closeUserInformation={closeUserInformation} />);
        }, 1000);
    }

    const openUserInformation = (customer) => {
        setUserInformation(<UserInformation customer={customer} closeUserInformation={closeUserInformation} />);
    }

    const clearUserInformationTimeout = () => {
        clearInterval(showUserInformationTimeout);
    }

    const closeUserInformation = () => {
        setUserInformation(<></>);
    }

    const createPersonalCard = (customer) => {
        return (
            <li key={customer.id}>
                <div className="card">
                    <div className="card-body">
                        <div>[icon]</div>
                        <h5 className="card-title" onMouseOver={() => openUserInformationWithTimeout(customer)}
                            onMouseLeave={() => clearUserInformationTimeout()}>{customer.username}</h5>
                        <FontAwesomeIcon icon={faWindowRestore} title="Show details" onClick={() => openUserInformation(customer)} />
                    </div>
                    <ul className="card__links list-group list-group-flush">
                        <li className="list-group-item"><FontAwesomeIcon icon={faCommentDots} title="Start chat" onClick={async () => await startChatAsync(customer)} /></li>
                        <li className="list-group-item"><FontAwesomeIcon icon={faUserPlus} title="Request to connect" onClick={async () => await createRequestToConnectAsync(customer)} /></li>
                        <li className="list-group-item"><FontAwesomeIcon icon={faSquarePlus} title="Add to community" /></li>
                    </ul>
                </div>
            </li>
        );
    }

    const render = () => {
        return (
            <div className="people">
                <div>
                    <div>Looking for</div>
                </div>
                <div className="people__successfully-request" style={{ display: showSuccessfullRequest ? "flex" : "none" }}>You have successfully sent a connection request</div>
                <ul className="people__cards">
                    {people}
                </ul>
                {userInformation}
            </div>
        );
    }

    return render();
}

export default People;