import { useState } from 'react';
import { useSelector } from 'react-redux';
import Friends from './Friends';
import MyFeed from './MyFeed';
import Profile from './Profile';
import RequestsToConnect from './RequestsToConnect';

import "../../../styles/communication/myEnvironment.scss";
import { useEffect } from 'react';

const MyEnvironment = () => {
    const [currentMenuItem, setCurrentMenuItem] = useState(0);
    const [myFeed, setMyFeed] = useState(<></>);

    const user = useSelector((state) => state.user.value);

    useEffect(() => {
        setMyFeed(<MyFeed usersId={[user.id]}/>);
    }, [])

    useEffect(() => {
        setMyFeed(<MyFeed usersId={[user === null ? 0 : user.id]} />);
    }, [user])

    const render = () => {
        return (<div className="my-environment">
            <ul className="my-environment__menu">
                <li>
                    <button type="button" className="btn btn-outline-success" onClick={() => setCurrentMenuItem(1)}>Profile</button>
                </li>
                <li>
                    <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(0)}>Feed</button>
                    <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(2)}>Friends</button>
                    <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(3)}>Community</button>
                </li>
                <li>
                    <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(4)}>Requests</button>
                    <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(5)}>Invites</button>
                    <button type="button" className="btn btn-outline-info" onClick={() => setCurrentMenuItem(6)}>Recomendations</button>
                </li>
            </ul>
            <div>
                {currentMenuItem === 0 ? myFeed : null}
                {currentMenuItem === 1 ? <Profile /> : null}
                {currentMenuItem === 2 ? <Friends /> : null}
                {currentMenuItem === 4 ? <RequestsToConnect /> : null}
            </div>
        </div>);
    }

    return render();
}

export default MyEnvironment;