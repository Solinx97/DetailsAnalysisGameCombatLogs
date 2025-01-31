import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import User from '../User';

const MyRequestItem = ({ me, request, cancelMyRequestAsync }) => {
    const { t } = useTranslation("communication/myEnvironment/myRequestItem");

    const { data: user, isLoading } = useGetUserByIdQuery(request.toAppUserId);

    const [userInformation, setUserInformation] = useState(null);

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="request-to-connect">
            <div className="request-to-connect__username">
                <User
                    me={me}
                    targetUserId={user?.id}
                    setUserInformation={setUserInformation}
                    allowRemoveFriend={false}
                    actionAfterRequests={null}
                />
            </div>
            <div className="request-to-connect__answer">
                <div className="reject">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title={t("Cancel")}
                        onClick={async () => await cancelMyRequestAsync(request.id)}
                    />
                </div>
            </div>
            {userInformation}
        </div>
    );
}

export default MyRequestItem;