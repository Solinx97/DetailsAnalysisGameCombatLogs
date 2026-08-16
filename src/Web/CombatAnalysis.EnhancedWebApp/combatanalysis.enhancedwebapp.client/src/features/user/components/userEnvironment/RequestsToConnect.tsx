import type { RootState } from '@/app/Store';
import logger from '@/utils/Logger';
import Loading from '@/shared/components/Loading';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useCreateFriendAsyncMutation } from '../../api/Friend.api';
import { useRemoveRequestAsyncMutation, useFindByOwnerIdQuery, useFindByUserIdQuery } from '../../api/RequestToConnect.api';
import type { FriendModel } from '../../types/FriendModel';
import type { RequestToConnectModel } from '../../types/RequestToConnectModel';
import MyRequestItem from './MyRequestItem';
import RequestItem from './RequestItem';

import './RequestToConnect.scss';

const RequestToConnect = () => {
    const { t } = useTranslation('communication/myEnvironment/requestsToConnect');

    const myself = useSelector((state: RootState) => state.user.value);

    const { data: allRequests, isLoading: reqIsLoading } = useFindByUserIdQuery(myself?.id ?? "");
    const { data: allMyRequests, isLoading: myReqIsLoading } = useFindByOwnerIdQuery(myself?.id ?? "");
    const [createFriendAsync] = useCreateFriendAsyncMutation();
    const [removeRequestAsync] = useRemoveRequestAsyncMutation();

    const acceptRequestAsync = async (request: RequestToConnectModel) => {
        try {
            const newFriend: FriendModel = {
                id: 0,
                whoFriendId: request.toAppUserId,
                whoFriendUsername: "",
                forWhomId: request.appUserId,
                forWhomUsername: "",
            };

            await createFriendAsync(newFriend).unwrap();
            await removeRequestAsync(request.id).unwrap();
        } catch (error) {
            logger.error("Failed to accept request", error);
        }

    }

    const rejectRequestAsync = async (requestId: number) => {
        try {
            await removeRequestAsync(requestId).unwrap();
        } catch (error) {
            logger.error("Failed to reject request", error);
        }

    }

    const cancelMyRequestAsync = async (requestId: number) => {
        try {
            await removeRequestAsync(requestId).unwrap();
        } catch (error) {
            logger.error("Failed to cancel request", error);
        }
    }

    if (reqIsLoading || myReqIsLoading) {
        return (<Loading />);
    }

    return (
        <>
            {(allRequests && allRequests.length > 0) &&
                <div>
                    <div>{t("Requests")}</div>
                    <ul>
                        {allRequests?.map((item) => (
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
            {(allMyRequests && allMyRequests.length > 0) &&
                <div>
                    <div>{t("MyRequests")}</div>
                    <ul>
                        {allMyRequests?.map((item) => (
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