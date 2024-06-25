import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useCreateUserPostAsyncMutation } from '../../store/api/communication/UserPost.api';
import Loading from '../Loading';
import CommunicationMenu from './CommunicationMenu';
import CreatePost from './CreatePost';
import FeedParticipants from './FeedParticipants';
import Recomendations from './Recomendations';

const Feed = () => {
    const { t } = useTranslation("communication/feed");
    const customer = useSelector((state) => state.customer.value);

    const [createNewUserPostAsync] = useCreateUserPostAsyncMutation();

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
            <Recomendations
                t={t}
            />
            <div className="communication__content">
                <CreatePost
                    customer={customer}
                    owner={customer?.username}
                    postTypeName="user"
                    createTypeOfPostFunc={createUserPostAsync}
                    t={t}
                />
                <FeedParticipants
                    customer={customer}
                    t={t}
                />
            </div>
        </>
    );
}

export default Feed;