import { faCircleXmark, faComments } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useGetCustomerByIdQuery } from '../../store/api/Customer.api';
import { useLazyGetCommunityPostByPostIdQuery } from '../../store/api/communication/community/CommunityPost.api';
import User from './User';

const postType = {
    user: 0,
    community: 1
}

const PostTitle = ({ post, dateFormatting, deletePostAsync, isMyPost }) => {
    const { t } = useTranslation("communication/postTitle");

    const navigate = useNavigate();

    const [getCommunityPostByPostId] = useLazyGetCommunityPostByPostIdQuery();

    const { data: targetCustomer, isLoading } = useGetCustomerByIdQuery(post?.customerId);

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
        return <></>;
    }

    return (
        <>
            <li className="posts__title list-group-item">
                <div className="posts__title content">
                    <div className="posts__title-username">
                        {post.postType === postType["user"]
                            ? <User
                                targetCustomerId={targetCustomer?.id}
                                setUserInformation={setUserInformation}
                                allowRemoveFriend={false}
                            />
                            : <div className={`community-post${communityPostStyle}`}
                                onMouseOver={communityPostOverHandler}
                                onMouseLeave={communityPostLeaveHandler}
                                onClick={goToCommunityAsync}
                                title={t("GoToCommunity")}>
                                <div className="community-post type">community</div>
                                <div className="community-post content">
                                    <FontAwesomeIcon
                                        icon={faComments}
                                    />
                                    <div>{post.owner}</div>
                                </div>
                            </div>
                        }
                    </div>
                    <div className="when">{dateFormatting(post?.when)}</div>
                </div>
                <div className="post-remove container">
                    {isMyPost &&
                        <FontAwesomeIcon
                            icon={faCircleXmark}
                            title={t("RemovePost")}
                            className="post-remove"
                            onClick={deletePostAsync}
                        />
                    }
                </div>
            </li>
            {userInformation !== null &&
                <div className="community-user-information">{userInformation}</div>
            }
        </>
    );
}

export default PostTitle;