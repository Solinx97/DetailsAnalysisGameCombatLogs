import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../store/api/Account.api';
import Loading from '../Loading';
import User from './User';

const PostTitle = ({ post, dateFormatting, deletePostAsync, isMyPost }) => {
    const { t } = useTranslation("communication/postTitle");

    const { data: taregtUser, isLoading } = useGetUserByIdQuery(post?.appUserId);

    const [userInformation, setUserInformation] = useState(null);

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <>
            <div className="posts__title">
                <div className="content">
                    <div className="username">
                        <User
                            targetUserId={taregtUser?.id}
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
                <div className="post-remove">
                    {isMyPost &&
                        <FontAwesomeIcon
                            icon={faCircleXmark}
                            title={t("RemovePost")}
                            className="post-remove"
                            onClick={deletePostAsync}
                        />
                    }
                </div>
            </div>
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default PostTitle;