import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../store/api/Customer.api';
import User from './User';

const PostTitle = ({ post, dateFormatting, deletePostAsync }) => {
    const { t } = useTranslation("communication/postTitle");

    const { data: targetCustomer, isLoading } = useGetCustomerByIdQuery(post?.customerId);

    const [userInformation, setUserInformation] = useState(null);

    if (isLoading) {
        return <></>;
    }

    return (
        <>
            <li className="posts__title list-group-item">
                <div className="posts__title-username">
                    <User
                        targetCustomerId={targetCustomer?.id}
                        setUserInformation={setUserInformation}
                        allowRemoveFriend={false}
                    />
                </div>
                <div className="posts__title-details">
                    <div>{dateFormatting(post?.when)}</div>
                    <FontAwesomeIcon
                        icon={faTrash}
                        title={t("RemovePost")}
                        onClick={deletePostAsync}
                    />
                </div>
            </li>
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default PostTitle;