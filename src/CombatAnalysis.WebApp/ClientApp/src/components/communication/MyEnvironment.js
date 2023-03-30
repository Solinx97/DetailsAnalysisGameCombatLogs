import { useEffect, useState, useRef } from 'react';
import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import "../../styles/communication/myEnvironment.scss";

const MyEnvironment = () => {
    const [invitesToChat, setInvitesToChat] = useState(<><p>invites</p></>);
    const [invitesToCommunity, setInvitesToCommunity] = useState(<><p>invites to community</p></>);
    const [requestsToConnect, setRequestsToConnect] = useState(<><p>requests to connect</p></>);
    const [fiends, setFriends] = useState(<><p>friends</p></>);

    const render = () => {
        return (<div>
            <div className="my-environment">
                <ul className="my-environment__menu">
                    <li>Friends</li>
                    <li>Requests to connect</li>
                    <li>Invites to community</li>
                    <li>Invites to chat</li>
                </ul>
                <div className="my-environment__content">
                    <ul>
                        {fiends}
                    </ul>
                    <ul>
                        {requestsToConnect}
                    </ul>
                    <ul>
                        {invitesToCommunity}
                    </ul>
                    <ul>
                        {invitesToChat}
                    </ul>
                </div>
            </div>
        </div>);
    }

    return render();
}

export default MyEnvironment;