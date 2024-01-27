import { useSelector } from 'react-redux';
import { useCreateUserPostAsyncMutation } from '../../store/api/communication/UserPost.api';
import CommunicationMenu from './CommunicationMenu';
import CreatePost from './CreatePost';
import FeedParticipants from './FeedParticipants';

const Feed = () => {
    const customer = useSelector((state) => state.customer.value);

    const [createNewUserPostAsync] = useCreateUserPostAsyncMutation();

    const createUserPostAsync = async (postId) => {
        const newUserPost = {
            userId: customer?.id,
            postId: postId
        }

        const createdUserPost = await createNewUserPostAsync(newUserPost);
        return createdUserPost.data === undefined ? false : true;
    }

    if (customer === null) {
        return (
            <CommunicationMenu
                currentMenuItem={0}
            />
        );
    }

    return (
        <>
            <CommunicationMenu
                currentMenuItem={0}
            />
            <div className="communication__content">
                <CreatePost
                    customer={customer}
                    owner={customer?.username}
                    postTypeName="user"
                    createTypeOfPostFunc={createUserPostAsync}
                />
                <FeedParticipants
                    customer={customer}
                />
            </div>
        </>
    );
}

export default Feed;