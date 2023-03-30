import { useEffect, useState } from "react";
import { useSelector } from 'react-redux';
import { faCommentDots, faUserPlus, faSquarePlus, faPlus } from '@fortawesome/free-solid-svg-icons';
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
        const response = await fetch(`/api/v1/Account`);
        const status = response.status;

        if (status === 200) {
            const allPeople = await response.json();
            fillCards(allPeople);
        }
    }

    const checkExistNewChatAsync = async (targetUser) => {
        const response = await fetch(`/api/v1/PersonalChat/isExist?initiatorId=${user.id}&companionId=${targetUser.id}`);
        if (response.status == 200) {
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

        if (response.status == 200) {
            updateCurrentMenuItem(0, user.id, targetUser.id);
        }
    }

    const fillCards = (allPeople) => {
        const list = allPeople.map((element) => createPersonalCard(element));

        setPeople(list);
    }

    const createPersonalCard = (element) => {
        return (<li key={element.id}>
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{element.email}</h5>
                    <p className="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
                </div>
                <ul className="list-group list-group-flush">
                    <li className="list-group-item">Cras justo odio</li>
                    <li className="list-group-item">Dapibus ac facilisis in</li>
                    <li className="list-group-item">Vestibulum at eros</li>
                </ul>
                <div className="card-body__links">
                    <FontAwesomeIcon icon={faCommentDots} title="Start chat" onClick={async () => await startChatAsync(element)} />
                    <FontAwesomeIcon icon={faUserPlus} title="Request to connect" />
                    <FontAwesomeIcon icon={faSquarePlus} title="Add to community" />
                    <FontAwesomeIcon icon={faPlus} title="Add to chat" />
                </div>
            </div>
        </li>);
    }

    const render = () => {
        return (<div className="people">
            <ul className="people__cards">
                {people}
            </ul>
        </div>);
    }

    return render();
}

export default People;