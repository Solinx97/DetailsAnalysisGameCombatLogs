import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { type JSX, memo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import User from '../../../user/components/User';
import { useRemoveUserPostMutation } from '../../api/UserPost.api';
import type { UserPostModel } from '../../types/UserPostModel';

interface UserPostTitleProps {
    post: UserPostModel;
    isMyPost: boolean;
    dateFormatting: (stringOfDate: string) => string;
}

const UserPostTitle: React.FC<UserPostTitleProps> = ({ post, isMyPost, dateFormatting }) => {
    const { t } = useTranslation("communication/postTitle");

    const { data: targetUser , isLoading} = useGetUserByIdQuery(post.appUserId);
    const [removeUserPost] = useRemoveUserPostMutation();

    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    const removeUserPostAsync = async () => {
        await removeUserPost({ id: post.id, appUserId: targetUser?.id ?? "" });
    }

    if (isLoading) {
        return (<></>);
    }

    return (
        <>
            <div className="posts__title">
                <div className="content">
                    <div className="username">
                        <User
                            targetUserId={targetUser?.id ?? ""}
                            targetUsername={targetUser?.username ?? ""}
                            setUserInformation={setUserInformation}
                        />
                    </div>
                    <div className="when">{dateFormatting(post.createdAt.toString())}</div>
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