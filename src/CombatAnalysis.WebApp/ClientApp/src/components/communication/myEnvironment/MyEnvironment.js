import { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import Post from '../Post';
import Friends from './Friends';
import Profile from './Profile';
import RequestsToConnect from './RequestsToConnect';

import "../../../styles/communication/myEnvironment.scss";

const MyEnvironment = () => {
    const [currentMenuItem, setCurrentMenuItem] = useState(0);
    const [myFeed, setMyFeed] = useState(<></>);

    const customer = useSelector((state) => state.customer.value);

    useEffect(() => {
        setMyFeed(<Post customersId={[customer.id]}/>);
    }, [])

    useEffect(() => {
        setMyFeed(<Post customersId={[customer === null ? 0 : customer.id]} />);
    }, [customer])

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