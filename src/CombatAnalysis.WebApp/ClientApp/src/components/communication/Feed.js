import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useCreateUserPostAsyncMutation } from '../../store/api/communication/UserPost.api';
import Loading from '../Loading';
import CommunicationMenu from './CommunicationMenu';
import CreatePost from './CreatePost';
import FeedParticipants from './FeedParticipants';

const Feed = () => {
    const { t } = useTranslation("communication/feed");
    const customer = useSelector((state) => state.customer.value);

    const [createNewUserPostAsync] = useCreateUserPostAsyncMutation();

    const [showNewPostsInform, setShowNewPostsInform] = useState(false);
    const [showNewPosts, setShowNewPosts] = useState(false);

    const createUserPostAsync = async (postId) => {
        try {
            const newUserPost = {
                userId: customer?.id,
                postId: postId
            }

            const response = await createNewUserPostAsync(newUserPost);
            if (response.error) {
                console.error("Error creating user post:", response.error);

                return false;
            }

            if (response.data) {
                return true;
            } else {
                console.error("Unexpected response structure:", response);

                return false;

            }
        } catch (e) {
            console.error("Failed to create user post:", e);

            return false;
        }
    }

    const showNewPostsHandle = () => {
        setShowNewPosts(true);
        setShowNewPostsInform(false);
    }

    if (customer === null) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={0}
                />
                <Loading />
            </>
        );
    }

    return (
        <>
            <CommunicationMenu
                currentMenuItem={0}
            />
            <div className="communication__content">
                <div className="new-posts"
                    style={{ display: showNewPostsInform ? "flex" : "none" }}
                    onClick={showNewPostsHandle}>
                    <div className="new-posts__content">{t("NewPosts")}</div>
                </div>
                <CreatePost
                    customer={customer}
                    owner={customer?.username}
                    postTypeName="user"
                    createTypeOfPostFunc={createUserPostAsync}
                />
                <FeedParticipants
                    customer={customer}
                    setShowNewPostsInform={setShowNewPostsInform}
                    showNewPosts={showNewPosts}
                />
            </div>
        </>
    );
}

export default Feed;