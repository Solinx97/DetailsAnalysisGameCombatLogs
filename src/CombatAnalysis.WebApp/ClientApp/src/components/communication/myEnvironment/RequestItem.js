import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import User from '../User';

const RequestItem = ({ me, request, acceptRequestAsync, rejectRequestAsync }) => {
    const { t } = useTranslation("communication/myEnvironment/requestItem");

    const { data: user, isLoading } = useGetUserByIdQuery(request.appUserId);
        
    const [userInformation, setUserInformation] = useState(null);

    if (isLoading) {
        return <></>;
    }

    return (
        <span className="request-to-connect">
            <div className="request-to-connect__username">
                <User
                    me={me}
                    targetUserId={user.id}
                    setUserInformation={setUserInformation}
                    allowRemoveFriend={false}
                    actionAfterRequests={null}
                />
            </div>
            <div className="request-to-connect__answer">
                <div className="accept">
                    <FontAwesomeIcon
                        icon={faCircleCheck}
                        title={t("Accept")}
                        onClick={async () => await acceptRequestAsync(request)}
                    />
                </div>
                <div className="reject">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title={t("Reject")}
                        onClick={async () => await rejectRequestAsync(request.id)}
                    />
                </div>
            </div>
            {userInformation}
        </span>
    );
}

export default RequestItem;