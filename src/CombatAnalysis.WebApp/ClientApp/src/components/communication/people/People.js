import { useEffect, useState } from "react";
import { useSelector } from 'react-redux';
import { faCommentDots, faUserPlus, faSquarePlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import "../../../styles/communication/people.scss";

const People = ({ updateCurrentMenuItem }) => {
    const user = useSelector((state) => state.user.value);

    const [people, setPeople] = useState(<></>);

    useEffect(() => {
        let getPeople = async () => {
            await getPeopleAsync();
        }

        getPeople();
    }, [])

    const getPeopleAsync = async () => {
        const response = await fetch("/api/v1/Account");
        const status = response.status;

        if (status === 200) {
            const allPeople = await response.json();
            fillCards(allPeople);
        }
    }

    const checkExistNewChatAsync = async (targetUser) => {
        const response = await fetch(`/api/v1/PersonalChat/isExist?initiatorId=${user.id}&companionId=${targetUser.id}`);
        if (response.status === 200) {
            const isExist = await response.json();
            return isExist;
        }

        return true;
    }

    const startChatAsync = async (targetUser) => {
        const isExist = await checkExistNewChatAsync(targetUser);
        if (isExist) {
            updateCurrentMenuItem(0, user.id, targetUser.id);
            return;
        }

        const newChat = {
            id: 0,
            initiatorUsername: user.email,
            companionUsername: targetUser.email,
            lastMessage: " ",
            initiatorId: user.id,
            companionId: targetUser.id
        };

        const response = await fetch("/api/v1/PersonalChat", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newChat)
        });

        if (response.status === 200) {
            updateCurrentMenuItem(0, user.id, targetUser.id);
        }
    }

    const createRequestToConnect = async (people) => {
        const newRequest = {
            id: 0,
            toUserId: people.id,
            when: new Date(),
            result: 0,
            ownerId: user.id,
        };

        const response = await fetch("/api/v1/RequestToConnect", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newRequest)
        });

        if (response.status === 200) {
            console.log(1);
        }
    }


    const fillCards = (allPeople) => {
        const list = allPeople.filter(peopleListFilter).map((element) => createPersonalCard(element));
        
        setPeople(list);
    }

    const peopleListFilter = (value) => {
        if (value.id !== user.id) {
            return value;
        }
    }

    const createPersonalCard = (element) => {
        return (<li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.email}</h5>
                    <p className="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item"><FontAwesomeIcon icon={faCommentDots} title="Start chat" onClick={async () => await startChatAsync(element)} /></li>
                    <li className="list-group-item"><FontAwesomeIcon icon={faUserPlus} title="Request to connect" onClick={async () => await createRequestToConnect(element)} /></li>
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
            <ul className="people__cards">
                {people}
            </ul>
        </div>);
    }

    return render();
}

export default People;