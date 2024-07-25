import { faCircleXmark, faComments } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useGetUserByIdQuery } from '../../store/api/Account.api';
import { useLazyGetCommunityPostByPostIdQuery } from '../../store/api/communication/community/CommunityPost.api';
import User from './User';
import Loading from '../Loading';

const postType = {
    user: 0,
    community: 1
}

const PostTitle = ({ post, dateFormatting, deletePostAsync, isMyPost }) => {
    const { t } = useTranslation("communication/postTitle");

    const navigate = useNavigate();

    const [getCommunityPostByPostId] = useLazyGetCommunityPostByPostIdQuery();

    const { data: taregtUser, isLoading } = useGetUserByIdQuery(post?.appUserId);

    const [userInformation, setUserInformation] = useState(null);
    const [communityPostStyle, setCommunityPostStyle] = useState("");

    const getCommunityPostAsync = async () => {
        const communityPost = await getCommunityPostByPostId(post.id);

        if (communityPost.data !== undefined) {
            return communityPost.data[0];
        }

        return null;
    }

    const communityPostOverHandler = (e) => {
        setCommunityPostStyle("_active");
    }

    const communityPostLeaveHandler = (e) => {
        setCommunityPostStyle("");
    }

    const goToCommunityAsync = async () => {
        const communityPost = await getCommunityPostAsync();

        if (communityPost !== null) {
            navigate(`/community?id=${communityPost.communityId}`);
        }
    }

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <>
            <div className="posts__title">
                <div className="content">
                    <div className="username">
                        {post?.postType === postType["user"]
                            ? <User
                                targetUserId={taregtUser?.id}
                                setUserInformation={setUserInformation}
                                allowRemoveFriend={false}
                            />
                            : <div className={`community-post${communityPostStyle}`}
                                onMouseOver={communityPostOverHandler}
                                onMouseLeave={communityPostLeaveHandler}
                                onClick={goToCommunityAsync}
                                title={t("GoToCommunity")}>
                                <div className="community-post type">{t("Community")}</div>
                                <div className="community-post content">
                                    <FontAwesomeIcon
                                        icon={faComments}
                                    />
                                    <div>{post?.owner}</div>
                                </div>
                            </div>
                        }
                    </div>
                    <div className="when">{dateFormatting(post?.when)}</div>
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