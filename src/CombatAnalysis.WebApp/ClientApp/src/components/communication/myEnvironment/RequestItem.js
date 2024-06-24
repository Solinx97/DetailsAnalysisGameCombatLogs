import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import User from '../User';

const RequestItem = ({ me, request, acceptRequestAsync, rejectRequestAsync }) => {
    const { t } = useTranslation("communication/myEnvironment/requestItem");

    const { data: user, isLoading } = useGetCustomerByIdQuery(request.customerId);

    const [userInformation, setUserInformation] = useState(null);

    if (isLoading) {
        return <></>;
    }

    return (
        <span className="request-to-connect">
            <div className="request-to-connect__username">
                <User
                    me={me}
                    targetCustomerId={user.id}
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