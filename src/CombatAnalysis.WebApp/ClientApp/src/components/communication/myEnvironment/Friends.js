import { useEffect, useState, useRef } from 'react';
import { faCheck } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import "../../../styles/communication/friends.scss";

const Friends = () => {
    const render = () => {
        return (<div className="friends">
            <div>You still are not add an any friend</div>
        </div>);
    }

    return render();
}

export default Friends;