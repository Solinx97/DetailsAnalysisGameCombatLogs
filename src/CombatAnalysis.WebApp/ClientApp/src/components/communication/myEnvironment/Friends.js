import { useEffect, useState, useRef } from 'react';
import { faCheck } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import Tab from '../../general/Tab';

import "../../../styles/communication/friends.scss";

const Friends = () => {
    const tabs = [
        {
            label: "Friends",
            content: <div>Tab friends</div>
        },
        {
            label: "Requests",
            content: <div>Tab requests</div>
        },
    ];

    const render = () => {
        return (<div className="friends">
            <Tab tabs={tabs}/>
        </div>);
    }

    return render();
}

export default Friends;