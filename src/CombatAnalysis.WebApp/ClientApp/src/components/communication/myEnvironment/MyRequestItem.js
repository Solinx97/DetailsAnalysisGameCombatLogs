import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const MyRequestItem = ({ request, cancelMyRequestAsync }) => {
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
                        title="Cancel"
                        onClick={async () => await cancelMyRequestAsync(request.id)}
                    />
                </div>
            </div>
        </span>
    );
}

export default MyRequestItem;