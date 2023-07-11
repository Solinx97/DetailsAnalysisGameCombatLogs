import { faCircleCheck, faCircleXmark, faRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';

import "../../../styles/communication/requestToConnect.scss";

const RequestToConnect = () => {
    const customer = useSelector((state) => state.customer.value);

    const timeForHideNotifications = 4000;

    const [requestsToConnect, setRequestsToConnect] = useState(<></>);
    const [myRequestsToConnect, setMyRequestsToConnect] = useState(<></>);
    const [showAcceptMessage, setShowAcceptMessage] = useState(false);
    const [showRejectMessage, setShowRejectMessage] = useState(false);
    const [showCancelMessage, setShowCancelMessage] = useState(false);
    const [showErrorMessage, setShowErrorMessage] = useState(false);

    useEffect(() => {
        let getRequests = async () => {
            await getRequestsAsync();
            await getMyRequestsAsync();
        }

        getRequests();
    }, [])

    const getRequestsAsync = async () => {
        const response = await fetch(`/api/v1/RequestToConnect/searchByToUserId/${customer.id}`);
        const status = response.status;

        if (status === 200) {
            const allRequests = await response.json();
            fillRequestsToConnect(allRequests);
        }
    }

    const getMyRequestsAsync = async () => {
        const response = await fetch(`/api/v1/RequestToConnect/searchByOwnerId/${customer.id}`);
        const status = response.status;

        if (status === 200) {
            const allMyRequests = await response.json();
            fillMyRequestsToConnect(allMyRequests);
        }
    }

    const fillRequestsToConnect = (allRequests) => {
        const list = allRequests.length !== 0
            ? allRequests.map((element) => requestsCard(element))
            : (<div className="requests__empty">You don't have any requests</div>);
        
        setRequestsToConnect(list);
    }

    const fillMyRequestsToConnect = (allMyRequests) => {
        const list = allMyRequests.length !== 0
            ? allMyRequests.map((element) => myRequestsCard(element))
            : (<div className="my-requests__empty">You didn't do any requests</div>);

        setMyRequestsToConnect(list);
    }

    const acceptRequestAsync = async (request) => {
        setShowAcceptMessage(false);

        const newFriend = {
            id: 0,
            username: request.username,
            whoFriendId: request.toUserId,
            forWhomId: request.ownerId
        };

        let response = await fetch("/api/v1/Friend", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newFriend)
        });

        if (response.status !== 200) {
            setShowErrorMessage(true);
            setTimeout(() => {
                setShowErrorMessage(false);
            }, timeForHideNotifications);

            return;
        }

        response = await fetch(`/api/v1/RequestToConnect/${request.id}`, {
            method: 'DELETE',
        });

        if (response.status !== 200) {
            setShowErrorMessage(true);
            setTimeout(() => {
                setShowErrorMessage(false);
            }, timeForHideNotifications);

            return;
        }

        setShowAcceptMessage(true);
        setTimeout(() => {
            setShowAcceptMessage(false);
        }, timeForHideNotifications);

        await getRequestsAsync();
    }

    const rejectRequestAsync = async (requestId) => {
        setShowRejectMessage(false);

        const response = await fetch(`/api/v1/RequestToConnect/${requestId}`, {
            method: 'DELETE',
        });

        if (response.status !== 200) {
            return;
        }

        setShowRejectMessage(true);
        setTimeout(() => {
            setShowRejectMessage(false);
        }, timeForHideNotifications);

        await getRequestsAsync();
    }

    const cancelMyRequestAsync = async (requestId) => {
        setShowCancelMessage(false);

        const response = await fetch(`/api/v1/RequestToConnect/${requestId}`, {
            method: 'DELETE',
        });

        if (response.status !== 200) {
            return;
        }

        setShowCancelMessage(true);
        setTimeout(() => {
            setShowCancelMessage(false);
        }, timeForHideNotifications);

        await getMyRequestsAsync();
    }

    const requestsCard = (request) => {
        return (<li key={request.id} className="request-to-connect">
            <div>{request.username}</div>
            <div className="request-to-connect__answer">
                <div className="accept"><FontAwesomeIcon icon={faCircleCheck} title="Accept"
                    onClick={async () => await acceptRequestAsync(request)} /></div>
                <div className="reject"><FontAwesomeIcon icon={faCircleXmark} title="Reject"
                    onClick={async () => await rejectRequestAsync(request.id)} /></div>
            </div>
        </li>);
    }

    const myRequestsCard = (element) => {
        return (<li key={element.id} className="request-to-connect">
            <div>{element.username}</div>
            <div className="request-to-connect__answer">
                <div className="reject"><FontAwesomeIcon icon={faCircleXmark} title="Cancel"
                    onClick={async () => await cancelMyRequestAsync(element.id)} /></div>
            </div>
        </li>);
    }

    const render = () => {
        return (<div>
            <div className="request-notifications">
                <div style={{ display: showErrorMessage ? "flex" : "none" }}>Something wrong. Try again</div>
                <div className="request-notifications__accept" style={{ display: showAcceptMessage ? "flex" : "none" }}>You accepted request to connect</div>
                <div className="request-notifications__reject" style={{ display: showRejectMessage ? "flex" : "none" }}>You rejected request to connect</div>
                <div className="request-notifications__reject" style={{ display: showCancelMessage ? "flex" : "none" }}>You canceled your request to connect</div>
            </div>
            <div className="requests">
                <div className="requests__title">
                    <div>Requests</div>
                    <FontAwesomeIcon icon={faRotate} title="Update" onClick={async () => await getRequestsAsync()} />
                </div>
                <ul>{requestsToConnect}</ul>
            </div>
            <div className="my-requests">
                <div className="my-requests__title">
                    <div>My requests</div>
                    <FontAwesomeIcon icon={faRotate} title="Update" onClick={async () => await getMyRequestsAsync()} />
                </div>
                <ul>{myRequestsToConnect}</ul>
            </div>

        </div>);
    }

    return render();
}

export default RequestToConnect;