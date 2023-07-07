import { faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useSelector } from 'react-redux';
import Chats from './chats/Chats';
import Communities from './Communities';
import Events from './events/Events';
import MyEnvironment from './myEnvironment/MyEnvironment';
import Post from './Post';
import People from './people/People';

import '../../styles/communication/communication.scss';
import { useEffect } from 'react';

const Communication = () => {
    const [currentMenuItem, setCurrentMenuItem] = useState(0);
    const [isOpenChat, setIsOpenChat] = useState(false);
    const [initiatorId, setInitiatorId] = useState("");
    const [companionId, setCompanionId] = useState("");
    const [feed, setFeed] = useState(<></>);

    const customer = useSelector((state) => state.customer.value);

    useEffect(() => {
        if (customer === null) {
            return;
        }

        let getUserFriends = async () => {
            await getUserFriendsAsync();
        }

        getUserFriends();
    }, [customer])

    const updateCurrentMenuItem = (menuItem, initiatorId, companionId) => {
        setCurrentMenuItem(menuItem);
        setIsOpenChat(true);
        setInitiatorId(initiatorId);
        setCompanionId(companionId);
    }

    const getFriendsIdByUserIdAsync = async () => {
        const response = await fetch(`/api/v1/Friend/searchByUserId/${customer.id}`);
        if (response.status !== 200) {
            return [];
        }

        const friends = await response.json();
        let customersId = [];
        for (var i = 0; i < friends.length; i++) {
            const customerId = friends[i].whoFriendId === customer.id ? friends[i].forWhomId : friends[i].whoFriendId;
            customersId.push(customerId);
        }

        return customersId;
    }

    const getUserFriendsAsync = async () => {
        const customerId = await getFriendsIdByUserIdAsync();
        customerId.push(customer.id);

        setFeed(<Post customersId={customerId} />);
    }

    const render = () => {
        return (<div className="communication">
            <ul className="communication__menu">
                <li className="menu-item" onClick={() => setCurrentMenuItem(0)}>
                    {currentMenuItem === 0 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                    <div>Feed</div>
                </li>
                <li className="menu-item" onClick={() => setCurrentMenuItem(1)}>
                    {currentMenuItem === 1 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                    <div>Chats</div>
                </li>
                <li className="menu-item_disabled">
                    {currentMenuItem === 2 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                    <div>Communities</div>
                </li>
                <li className="menu-item" onClick={() => setCurrentMenuItem(3)}>
                    {currentMenuItem === 3 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                    <div>Events</div>
                </li>
                <li className="menu-item" onClick={() => setCurrentMenuItem(4)}>
                    {currentMenuItem === 4 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null }
                    <div>People</div>
                </li>
                <li className="menu-item" onClick={() => setCurrentMenuItem(5)}>
                    {currentMenuItem === 5 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                    <div>My environment</div>
                </li>
            </ul>
            <div className="communication__action">
                {currentMenuItem === 0 ? feed : null}
                {currentMenuItem === 1 ? <Chats isOpenChat={isOpenChat} initiatorId={initiatorId} companionId={companionId} /> : null}
                {currentMenuItem === 2 ? <Communities /> : null}
                {currentMenuItem === 3 ? <Events /> : null}
                {currentMenuItem === 4 ? <People updateCurrentMenuItem={updateCurrentMenuItem} /> : null}
                {currentMenuItem === 5 ? <MyEnvironment /> : null}
            </div>
        </div>);
    }

    return render();
}

export default Communication;