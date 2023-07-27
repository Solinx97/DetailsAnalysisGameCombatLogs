import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const RequestItem = ({ request, acceptRequestAsync, rejectRequestAsync }) => {
    const { data: user, isLoading } = useGetCustomerByIdQuery(request.toUserId);

    if (isLoading) {
        return <></>;
    }

    return (
        <span className="request-to-connect">
            <div>{user?.username}</div>
            <div className="request-to-connect__answer">
                <div className="accept">
                    <FontAwesomeIcon
                        icon={faCircleCheck}
                        title="Accept"
                        onClick={async () => await acceptRequestAsync(request)}
                    />
                </div>
                <div className="reject">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title="Reject"
                        onClick={async () => await rejectRequestAsync(request.id)}
                    />
                </div>
            </div>
        </span>
    );
}

export default RequestItem;