import { faCommentDots, faSquarePlus, faUserPlus, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useSelector } from 'react-redux';
import UserInformation from './../UserInformation';

import "../../../styles/communication/people.scss";

const People = ({ updateCurrentMenuItem }) => {
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
        const response = await fetch("/api/v1/Customer");

        if (response.status === 200) {
            const allPeople = await response.json();

            fillCards(allPeople);
        }
    }

    const checkExistNewChatAsync = async (targetCustomer) => {
        const response = await fetch(`/api/v1/PersonalChat/isExist?initiatorId=${customer.id}&companionId=${targetCustomer.id}`);
        if (response.status === 200) {
            const isExist = await response.json();
            return isExist;
        }

        return true;
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

        const response = await fetch("/api/v1/PersonalChat", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newChat)
        });

        if (response.status === 200) {
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

        const response = await fetch("/api/v1/RequestToConnect", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newRequest)
        });

        if (response.status === 200) {
            setShowSuccessfullRequest(true);

            setTimeout(() => {
                setShowSuccessfullRequest(false);
            }, timeForHideNotifications);
        }
    }

    const fillCards = (allPeople) => {
        const list = allPeople.filter(peopleListFilter).map((element) => createPersonalCard(element));
        
        setPeople(list);
    }

    const peopleListFilter = (value) => {
        if (value.id !== customer.id) {
            return value;
        }
    }

    const openUserInformationWithTimeout = (customer) => {
        let timeout = setTimeout(() => {
            setUserInformation(<UserInformation customer={customer} closeUserInformation={closeUserInformation} />);
        }, 1000);

        showUserInformationTimeout = timeout;
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

    const createPersonalCard = (element) => {
        return (<li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <div>[icon]</div>
                    <h5 className="card-title" onMouseOver={() => openUserInformationWithTimeout(element)}
                        onMouseLeave={() => clearUserInformationTimeout()}>{element.username}</h5>
                    <FontAwesomeIcon icon={faWindowRestore} title="Show details" onClick={() => openUserInformation(element)} />
                </div>
                <ul className="card__links list-group list-group-flush">
                    <li className="list-group-item"><FontAwesomeIcon icon={faCommentDots} title="Start chat" onClick={async () => await startChatAsync(element)} /></li>
                    <li className="list-group-item"><FontAwesomeIcon icon={faUserPlus} title="Request to connect" onClick={async () => await createRequestToConnectAsync(element)} /></li>
                    <li className="list-group-item"><FontAwesomeIcon icon={faSquarePlus} title="Add to community" /></li>
                </ul>
            </div>
        </li>);
    }

    const render = () => {
        return (<div className="people">
            <div>
                <div>Populations people</div>
                <div>Active people</div>
                <div>Looking for</div>
                <div>Looking for by tag(s)</div>
            </div>
            <div className="people__successfully-request" style={{ display: showSuccessfullRequest ? "flex" : "none" }}>You have successfully sent a connection request</div>
            <ul className="people__cards">
                {people}
            </ul>
            {userInformation}
        </div>);
    }

    return render();
}

export default People;