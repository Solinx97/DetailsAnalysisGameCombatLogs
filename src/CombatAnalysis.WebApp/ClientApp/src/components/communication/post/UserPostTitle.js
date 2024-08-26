import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../store/api/Account.api';
import { useRemoveUserPostMutation } from '../../../store/api/communication/UserPost.api';
import Loading from '../../Loading';
import User from '../User';

const UserPostTitle = ({ post, dateFormatting, isMyPost }) => {
    const { t } = useTranslation("communication/postTitle");

    const { data: targetUser, isLoading } = useGetUserByIdQuery(post?.appUserId);
    const [removeUserPost] = useRemoveUserPostMutation();

    const [userInformation, setUserInformation] = useState(null);

    const removeUserPostAsync = async () => {
        await removeUserPost(post.id);  
    }

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <>
            <div className="posts__title">
                <div className="content">
                    <div className="username">
                        <User
                            targetUserId={targetUser?.id}
                            setUserInformation={setUserInformation}
                            allowRemoveFriend={false}
                        />
                    </div>
                    <div className="when">{dateFormatting(post?.createdAt)}</div>
                </div>
                <ul className="tags">
                    {post?.tags?.split(';').filter(x => x.length > 0).map((tag, index) => (
                        <li key={index} className="tag">{tag}</li>
                    ))}
                </ul>
                {isMyPost &&
                    <div className="post-remove">
                        <FontAwesomeIcon
                            icon={faCircleXmark}
                            title={t("RemovePost")}
                            className="post-remove"
                            onClick={removeUserPostAsync}
                        />
                    </div>
                }
            </div>
            {userInformation &&
                <div className="posts__user-information">{userInformation}</div>
            }
        </>
    );
}

export default UserPostTitle;