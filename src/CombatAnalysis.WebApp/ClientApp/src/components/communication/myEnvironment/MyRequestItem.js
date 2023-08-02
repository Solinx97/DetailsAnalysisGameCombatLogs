import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const MyRequestItem = ({ request, cancelMyRequestAsync }) => {
    const { t } = useTranslation("communication/myEnvironment/myRequestItem");

    const { data: user, isLoading } = useGetCustomerByIdQuery(request.toUserId);

    if (isLoading) {
        return <></>;
    }

    return (
        <span className="request-to-connect">
            <div>{user?.username}</div>
            <div className="request-to-connect__answer">
                <div className="reject">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title={t("Cancel")}
                        onClick={async () => await cancelMyRequestAsync(request.id)}
                    />
                </div>
            </div>
        </span>
    );
}

export default MyRequestItem;