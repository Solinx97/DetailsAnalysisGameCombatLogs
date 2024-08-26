import { faComments, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useRemoveCommunityPostMutation } from '../../../store/api/communication/CommunityPost.api';

const CommunityPostTitle = ({ post, dateFormatting, isMyPost }) => {
    const { t } = useTranslation("communication/postTitle");

    const navigate = useNavigate();

    const [removeCommunityPost] = useRemoveCommunityPostMutation();

    const removeCommunityPostAsync = async () => {
        await removeCommunityPost(post.id);
    }

    const goToCommunityAsync = async () => {
        navigate(`/community?id=${post.communityId}`);
    }

    return (
        <div className="posts__title">
            <div className="content">
                <div className="username">
                    <div className="community-post"
                        onClick={goToCommunityAsync}
                        title={t("GoToCommunity")}>
                        <div className="community-post type">{t("Community")}</div>
                        <div className="community-post content">
                            <FontAwesomeIcon
                                icon={faComments}
                            />
                            <div>{post?.communityName}</div>
                        </div>
                    </div>
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
                        onClick={removeCommunityPostAsync}
                    />
                </div>
            }
        </div>
    );
}

export default CommunityPostTitle;