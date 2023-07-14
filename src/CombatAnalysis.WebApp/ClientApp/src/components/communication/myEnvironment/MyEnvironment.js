import { useState } from 'react';
import Friends from './Friends';
import MyCommunities from './MyCommunities';
import MyFeed from './MyFeed';
import Profile from './Profile';

import '../../../styles/communication/myEnvironment.scss';

const MyEnvironment = ({ openCommunity }) => {
    const [currentMenuItem, setCurrentMenuItem] = useState(0);

    const render = () => {
        return (<div className="my-environment">
            <ul className="my-environment__menu">
                <li>
                    <button type="button" className="btn btn-outline-success" onClick={() => setCurrentMenuItem(1)}>Profile</button>
                </li>
                <li>
                    <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(0)}>Feed</button>
                    <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(2)}>Friends</button>
                    <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(3)}>Communities</button>
                </li>
                <li>
                    <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(6)}>Recomendations</button>
                </li>
            </ul>
            <div>
                {currentMenuItem === 0 ? <MyFeed/> : null}
                {currentMenuItem === 1 ? <Profile /> : null}
                {currentMenuItem === 2 ? <Friends /> : null}
                {currentMenuItem === 3 ? <MyCommunities openCommunity={openCommunity} /> : null}
            </div>
        </div>);
    }

    return render();
}

export default MyEnvironment;