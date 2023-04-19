import { useEffect, useState, useRef } from 'react';
import { faCheck } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import Friends from './Friends';

import "../../../styles/communication/myEnvironment.scss";

const MyEnvironment = () => {
    const [currentMenuItem, setCurrentMenuItem] = useState(0);

    const render = () => {
        return (<div className="my-environment">
            <ul className="my-environment__menu">
                <li onClick={() => setCurrentMenuItem(0)}>
                    {currentMenuItem === 0 ? < FontAwesomeIcon icon={faCheck} /> : null}
                    <div>Friends</div>
                </li>
                <li onClick={() => setCurrentMenuItem(1)}>
                    {currentMenuItem === 1 ? < FontAwesomeIcon icon={faCheck} /> : null}
                    <div>Requests to connect</div>
                </li>
                <li onClick={() => setCurrentMenuItem(2)}>
                    {currentMenuItem === 2 ? < FontAwesomeIcon icon={faCheck} /> : null}
                    <div>Invites to community</div>
                </li>
                <li onClick={() => setCurrentMenuItem(3)}>
                    {currentMenuItem === 3 ? < FontAwesomeIcon icon={faCheck} /> : null}
                    <div>Invites to chat</div>
                </li>
            </ul>
            <div className="my-environment__action">
                {currentMenuItem === 0 ? <Friends /> : null}
            </div>
        </div>);
    }

    return render();
}

export default MyEnvironment;