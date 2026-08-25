import logger from '@/utils/Logger';
import useFormatting from '@/shared/hooks/useFormatting';
import { faCircleXmark, faComments } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import User from '@/features/user/components/User';
import type { AppUserModel } from '@/features/user/types/AppUserModel';
import { useState, type JSX } from 'react';
import { useNavigate } from 'react-router-dom';
import { useRemoveCommunityPostMutation } from '../../api/CommunityPost.api';
import type { CommunityPostModel } from '../../types/CommunityPostModel';
import type { useRemoveUserPostMutation } from '../../api/UserPost.api';
import type { PostModel } from '../../types/PostModel';

interface PostTitleProps {
    user: AppUserModel;
    post: PostModel;
    isMyPost: boolean;
    feedVersion: number;
    t: (key: string) => string;
    removeUserPost?: ReturnType<
        typeof useRemoveUserPostMutation
    >[0];
    removeCommunityPost?: ReturnType<
        typeof useRemoveCommunityPostMutation
    >[0];
}

const PostTitle: React.FC<PostTitleProps> = ({ user, post, isMyPost, removeUserPost, removeCommunityPost, feedVersion, t }) => {
    const { dateFormatting } = useFormatting();

    const navigate = useNavigate();

    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    const isCommunityPost = (post: PostModel): post is CommunityPostModel => {
        return "communityId" in post;
    }

    const removeCommunityPostAsync = async () => {
        try {
            if (isCommunityPost(post) && removeCommunityPost) {
                await removeCommunityPost({ id: post.id, communityId: post.communityId ?? 0, appUserId: user.id, feedVersion }).unwrap();
            }
            else if (removeUserPost) {
                await removeUserPost({ id: post.id, appUserId: user.id, feedVersion }).unwrap();
            }
        } catch (error) {
            logger.error("Failed to remove post comment", error);
        }
    }

    const goToCommunityAsync = async () => {
        if (isCommunityPost(post) && removeCommunityPost) {
            navigate(`/community?id=${post.communityId}`);
        }
    }

    const getPostTitle = () => {
        if (isCommunityPost(post) && removeCommunityPost) {
            return (
                <div className="community-post"
                    onClick={goToCommunityAsync}
                    title={t("GoToCommunity")}>
                    <div className="community-post type">{t("Community")}</div>
                    <div className="community-post content">
                        <FontAwesomeIcon
                            icon={faComments}
                        />
                        <div>{post.communityName}</div>
                    </div>
                </div>
            );
        }
        else {
            return (
                <User
                    targetUserId={user.id}
                    targetUsername={user.username}
                    setUserInformation={setUserInformation}
                />
            );
        }
    }

    return (
        <>
            <div className="posts__title">
                <div className="content">
                    <div className="username">
                        {getPostTitle()}
                    </div>
                    <div className="when">{dateFormatting(post?.createdAt.toString())}</div>
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
                            onClick={removeCommunityPostAsync}
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

export default PostTitle;