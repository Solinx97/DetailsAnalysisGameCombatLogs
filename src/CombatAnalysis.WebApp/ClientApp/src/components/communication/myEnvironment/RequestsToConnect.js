import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useCreateFriendAsyncMutation } from '../../../store/api/communication/myEnvironment/Friend.api';
import { useRemoveRequestAsyncMutation, useSearchByOwnerIdQuery, useSearchByToUserIdQuery } from '../../../store/api/communication/myEnvironment/RequestToConnect.api';
import MyRequestItem from './MyRequestItem';
import RequestItem from './RequestItem';

import "../../../styles/communication/myEnvironment/requestToConnect.scss";

const RequestToConnect = () => {
    const { t } = useTranslation("communication/myEnvironment/requestsToConnect");

    const customer = useSelector((state) => state.customer.value);

    const { data: allRequests, isLoading: reqIsLoading } = useSearchByToUserIdQuery(customer?.id);
    const { data: allMyRequests, isLoading: myReqIsLoading } = useSearchByOwnerIdQuery(customer?.id);
    const [createFriendAsync] = useCreateFriendAsyncMutation();
    const [removeRequestAsync] = useRemoveRequestAsyncMutation();

    const acceptRequestAsync = async (request) => {
        const newFriend = {
            username: request.username,
            whoFriendId: request.toUserId,
            forWhomId: request.customerId
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

    return (
        <>
            {allRequests?.length > 0 &&
                <div>
                    <div>{t("Requests")}</div>
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
                    <div>{t("MyRequests")}</div>
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