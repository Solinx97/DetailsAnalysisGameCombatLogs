import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import { useRemoveUserPostMutation } from '../../../store/api/post/UserPost.api';
import User from '../User';

const UserPostTitle = ({ post, dateFormatting, isMyPost }) => {
    const { t } = useTranslation("communication/postTitle");

    const { data: targetUser } = useGetUserByIdQuery(post?.appUserId);
    const [removeUserPost] = useRemoveUserPostMutation();

    const [userInformation, setUserInformation] = useState(null);

    const removeUserPostAsync = async () => {
        await removeUserPost(post.id);
    }

    return (
        <>
            <div className="posts__title">
                <div className="content">
                    <div className="username">
                        <User
                            targetUserId={targetUser ? targetUser.id : "0"}
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

export default memo(UserPostTitle);