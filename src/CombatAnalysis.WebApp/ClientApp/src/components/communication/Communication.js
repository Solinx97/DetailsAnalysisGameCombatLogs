import { faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import Chats from './chats/Chats';
import Communities from './community/Communities';
import CommunityItem from './community/CommunityItem';
import Events from './events/Events';
import Feed from './Feed';
import MyEnvironment from './myEnvironment/MyEnvironment';
import People from './people/People';

import '../../styles/communication/communication.scss';

const Communication = () => {
    const [currentMenuItem, setCurrentMenuItem] = useState(0);
    const [community, setCommunity] = useState(null);
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
        return (
            <div className="communication">
                <ul className="communication__menu">
                    <li className="menu-item" onClick={() => setCurrentMenuItem(0)}>
                        {currentMenuItem === 0 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                        <div className="title">Feed</div>
                    </li>
                    <li className="menu-item" onClick={() => setCurrentMenuItem(1)}>
                        {currentMenuItem === 1 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                        <div className="title">Chats</div>
                    </li>
                    <li>
                        <div className="menu-item" onClick={() => setCurrentMenuItem(2)}>
                            {currentMenuItem === 2 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                            <div className="title">Communities</div>
                        </div>
                        {currentMenuItem === 6
                            ? <CommunityItem currentMenuItem={currentMenuItem} community={community} />
                            : null
                        }
                    </li>
                    <li className="menu-item" onClick={() => setCurrentMenuItem(3)}>
                        {currentMenuItem === 3 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                        <div className="title">Events</div>
                    </li>
                    <li className="menu-item" onClick={() => setCurrentMenuItem(4)}>
                        {currentMenuItem === 4 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null }
                        <div className="title">People</div>
                    </li>
                    <li className="menu-item" onClick={() => setCurrentMenuItem(5)}>
                        {currentMenuItem === 5 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                        <div className="title">My environment</div>
                    </li>
                </ul>
                <div className="communication__action">
                    {currentMenuItem === 0 ? <Feed /> : null}
                    {currentMenuItem === 1 ? <Chats isOpenChat={isOpenChat} initiatorId={initiatorId} companionId={companionId} /> : null}
                    {currentMenuItem === 2 ? <Communities /> : null}
                    {currentMenuItem === 3 ? <Events /> : null}
                    {currentMenuItem === 4 ? <People updateCurrentMenuItem={updateCurrentMenuItem} /> : null}
                    {currentMenuItem === 5 ? <MyEnvironment /> : null}
                </div>
            </div>
        );
    }

    return render();
}

export default Communication;