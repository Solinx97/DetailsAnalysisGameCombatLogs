import { useEffect, useState, useRef } from 'react';
import { useSelector } from 'react-redux';
import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import "../../../styles/communication/requestToConnect.scss";

const RequestToConnect = () => {
    const user = useSelector((state) => state.user.value);

    const [requestsToConnect, setRequestsToConnect] = useState(<></>);
    const [myRequestsToConnect, setMyRequestsToConnect] = useState(<></>);

    useEffect(() => {
        let getRequests = async () => {
            await getRequestsAsync();
            await getMyRequestsAsync();
        }

        getRequests();
    }, [])

    const getRequestsAsync = async () => {
        const response = await fetch(`/api/v1/RequestToConnect/searchByToUserId/${user.id}`);
        const status = response.status;

        if (status === 200) {
            const allRequests = await response.json();
            fillRequestsToConnect(allRequests);
        }
    }

    const getMyRequestsAsync = async () => {
        const response = await fetch(`/api/v1/RequestToConnect/searchByOwnerId/${user.id}`);
        const status = response.status;

        if (status === 200) {
            const allMyRequests = await response.json();
            fillMyRequestsToConnect(allMyRequests);
        }
    }

    const fillRequestsToConnect = (allRequests) => {
        const list = allRequests.map((element) => createCard(element));

        setRequestsToConnect(list);
    }

    const fillMyRequestsToConnect = (allRequests) => {
        const list = allRequests.map((element) => createCard(element));

        setMyRequestsToConnect(list);
    }

    const createCard = (element) => {
        return (<li key={element.id} className="request-to-connect">
            <div>{element.username}</div>
            <ul className="request-to-connect__answer">
                <li className="accept"><FontAwesomeIcon icon={faCircleCheck} title="Accept" /></li>
                <li className="reject"><FontAwesomeIcon icon={faCircleXmark} title="Reject" /></li>
            </ul>
        </li>);
    }

    const render = () => {
        return (<div>
            <div>Requests</div>
            <ul>{requestsToConnect}</ul>
            <div>My requests</div>
            <ul>{myRequestsToConnect}</ul>
        </div>);
    }

    return render();
}

export default RequestToConnect;