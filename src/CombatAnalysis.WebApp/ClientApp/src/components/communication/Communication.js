import { useEffect, useState, useRef } from 'react';
import Chats from './Chats';
import { faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import "../../styles/communication/communication.scss";

const Communication = () => {
    const [currentMenuItem, setCurrentMenuItem] = useState(0);

    const render = () => {
        return (<div className="communication">
            <div className="communication__menu">
                <div className="menu-item" onClick={() => setCurrentMenuItem(0)}>
                    {currentMenuItem === 0 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                    <div>Chats</div>
                </div>
                <div className="menu-item" onClick={() => setCurrentMenuItem(1)}>
                    {currentMenuItem === 1 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null }
                    <div>People</div>
                </div>
                <div className="menu-item" onClick={() => setCurrentMenuItem(2)}>
                    {currentMenuItem === 2 ? < FontAwesomeIcon icon={faCircleArrowRight} title="Current action" /> : null}
                    <div>Invites</div>
                </div>
            </div>
            <div className="communication__action">
                <Chats />
            </div>
        </div>);
    }

    return render();
}

export default Communication;