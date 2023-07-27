import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import { useCreateFriendAsyncMutation } from '../../../store/api/Friend.api';
import { useSearchByToUserIdQuery } from '../../../store/api/UserApi';
import { useRemoveRequestAsyncMutation, useSearchByOwnerIdQuery } from '../../../store/api/RequestToConnect.api';

import "../../../styles/communication/requestToConnect.scss";
import MyRequestItem from './MyRequestItem';
import RequestItem from './RequestItem';

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

    if (reqIsLoading || myReqIsLoading) {
        return <></>;
    }

    if (allRequests.length === undefined
        && allMyRequests.length === undefined) {
        return (<></>);
    }

    return (
        <>
            {allRequests?.length > 0 &&
                <div>
                    <div><strong>Requests</strong></div>
                    <ul>
                        {
                            allRequests?.map((item) => (
                                <li key={item.id}>
                                    <RequestItem
                                        request={item}
                                        acceptRequestAsync={acceptRequestAsync}
                                        rejectRequestAsync={rejectRequestAsync}
                                    />
                                </li>
                            ))
                        }
                    </ul>
                </div>
            }
            {allMyRequests?.length > 0 &&
                <div>
                    <div><strong>My requests</strong></div>
                    <ul>
                        {
                            allMyRequests?.map((item) => (
                                <li key={item.id}>
                                    <MyRequestItem
                                        request={item}
                                        cancelMyRequestAsync={cancelMyRequestAsync}
                                    />
                                </li>
                            ))
                        }
                    </ul>
                </div>
            }
        </>
    );
}

export default RequestToConnect;