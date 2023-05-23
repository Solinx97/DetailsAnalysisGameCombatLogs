import { useEffect, useState, useRef } from 'react';

const RequestToConnect = () => {
    const [requestsToConnect, setRequestsToConnect] = useState(<></>);
    const [myRequestsToConnect, setMyRequestsToConnect] = useState(<></>);

    useEffect(() => {
        let getRequests = async () => {
            await getRequestsAsync();
            await getMyRequestsAsync();
        }

        getRequests();
    }, [])

    const getRequestsAsync = () => {

    }

    const getMyRequestsAsync = () => {

    }

    const render = () => {
        return (<div>
            <div>Requests</div>
            <ul></ul>
            <div>My requests</div>
            <ul></ul>
        </div>);
    }

    return render();
}

export default RequestToConnect;