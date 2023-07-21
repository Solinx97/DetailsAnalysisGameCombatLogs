import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import { useCreateFriendAsyncMutation } from '../../../store/api/Friend.api';
import { useRemoveRequestAsyncMutation, useSearchByOwnerIdQuery, useSearchByToUserIdQuery } from '../../../store/api/RequestToConnect.api';

import "../../../styles/communication/requestToConnect.scss";

const RequestToConnect = () => {
    const [, customer] = useAuthentificationAsync();

    const { data: allRequests, isLoading: reqIsLoading } = useSearchByToUserIdQuery(customer?.id);
    const { data: allMyRequests, isLoading: myReqIsLoading } = useSearchByOwnerIdQuery(customer?.id);
    const [createFriendAsync] = useCreateFriendAsyncMutation();
    const [removeRequestAsync] = useRemoveRequestAsyncMutation();

    const acceptRequestAsync = async (request) => {
        const newFriend = {
            username: request.username,
            whoFriendId: request.toUserId,
            forWhomId: request.ownerId
        };

        const creadetFriend = await createFriendAsync(newFriend);
        if (creadetFriend.error !== undefined) {
            return;
        }

        const deletedItem = await removeRequestAsync(request.id);
        if (deletedItem.error !== undefined) {
            return;
        }
    }

    const rejectRequestAsync = async (requestId) => {
        const deletedItem = await removeRequestAsync(requestId);
        if (deletedItem.error !== undefined) {
            return;
        }
    }

    const cancelMyRequestAsync = async (requestId) => {
        const deletedItem = await removeRequestAsync(requestId);
        if (deletedItem.error !== undefined) {
            return;
        }
    }

    const requestsPanel = () => {
        return (
            <div>
                {
                    allRequests?.map((item) => (
                        <li key={item.id} className="request-to-connect">
                            <div>{item.username}</div>
                            <div className="request-to-connect__answer">
                                <div className="accept">
                                    <FontAwesomeIcon
                                        icon={faCircleCheck}
                                        title="Accept"
                                        onClick={async () => await acceptRequestAsync(item)}
                                    />
                                </div>
                                <div className="reject">
                                    <FontAwesomeIcon
                                        icon={faCircleXmark}
                                        title="Reject"
                                        onClick={async () => await rejectRequestAsync(item.id)}
                                    />
                                </div>
                            </div>
                        </li>
                    ))
                }
                {
                    allMyRequests?.map((item) => (
                        <li key={item.id} className="request-to-connect">
                            <div>{item.username}</div>
                            <div className="request-to-connect__answer">
                                <div className="reject">
                                    <FontAwesomeIcon
                                        icon={faCircleXmark}
                                        title="Cancel"
                                        onClick={async () => await cancelMyRequestAsync(item.id)}
                                    />
                                </div>
                            </div>
                        </li>
                    ))
                }
            </div>
        );
    }

    if (reqIsLoading || myReqIsLoading) {
        return <></>;
    }

    if (allRequests.length === undefined
        && allMyRequests.length === undefined) {
        return (<></>);
    }

    return requestsPanel();
}

export default RequestToConnect;