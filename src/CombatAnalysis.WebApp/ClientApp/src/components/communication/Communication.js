import { useState } from 'react';
import Chats from './chats/Chats';
import Communities from './Communities';
import People from './people/People';
import MyEnvironment from './myEnvironment/MyEnvironment';
import { faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import "../../styles/communication/communication.scss";

const Communication = () => {
    const [currentMenuItem, setCurrentMenuItem] = useState(0);
    const [isOpenChat, setIsOpenChat] = useState(false);
    const [initiatorId, setInitiatorId] = useState("");
    const [companionId, setCompanionId] = useState("");

    const updateCurrentMenuItem = (menuItem, initiatorId, companionId) => {
        setCurrentMenuItem(menuItem);
        setIsOpenChat(true);
        setInitiatorId(initiatorId);
        setCompanionId(companionId);
    }

    const render = () => {
        return (<div className="communication">
            <ul className="communication__menu">
                <li className="menu-item" onClick={() => setCurrentMenuItem(0)}>
                    {currentMenuItem === 0 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                    <div>Chats</div>
                </li>
                <li className="menu-item_disabled">
                    {currentMenuItem === 1 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                    <div>Communities</div>
                </li>
                <li className="menu-item" onClick={() => setCurrentMenuItem(2)}>
                    {currentMenuItem === 2 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null }
                    <div>People</div>
                </li>
                <li className="menu-item" onClick={() => setCurrentMenuItem(3)}>
                    {currentMenuItem === 3 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                    <div>My environment</div>
                </li>
            </ul>
            <div className="communication__action">
                {currentMenuItem === 0 ? <Chats isOpenChat={isOpenChat} initiatorId={initiatorId} companionId={companionId} /> : null}
                {currentMenuItem === 1 ? <Communities /> : null}
                {currentMenuItem === 2 ? <People updateCurrentMenuItem={updateCurrentMenuItem} /> : null}
                {currentMenuItem === 3 ? <MyEnvironment /> : null}
            </div>
        </div>);
    }

    return render();
}

export default Communication;