import { faTrash, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useGetCustomerByIdQuery } from '../../store/api/Customer.api';
import UserInformation from './UserInformation';

const PostTitle = ({ post, dateFormatting, deletePostAsync }) => {
    const { data: targetCustomer, isLoading } = useGetCustomerByIdQuery(post?.ownerId);

    const [showPostOwner, setShowPostOwner] = useState(false);

    const switchPostOwnerInformation = () => {
        setShowPostOwner((item) => !item);
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <>
            <li className="posts__title list-group-item">
                <div className="posts__title_username">
                    <div>{targetCustomer?.username}</div>
                    <FontAwesomeIcon
                        icon={faWindowRestore}
                        title="Show details"
                        onClick={switchPostOwnerInformation}
                    />
                </div>
                <div className="posts__title_details">
                    <div>{dateFormatting(post.when)}</div>
                    <FontAwesomeIcon
                        icon={faTrash}
                        title="Remove post"
                        onClick={deletePostAsync}
                    />
                </div>
            </li>
            {showPostOwner &&
                <UserInformation
                    customer={targetCustomer}
                    closeUserInformation={() =>setShowPostOwner((item) => !item)}
                />
            }
        </>
    );
}

export default PostTitle;