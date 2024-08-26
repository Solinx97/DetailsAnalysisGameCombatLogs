import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import Loading from '../Loading';
import CommunicationMenu from './CommunicationMenu';
import CreateUserPost from './post/CreateUserPost';
import FeedParticipants from './FeedParticipants';

const Feed = () => {
    const { t } = useTranslation("communication/feed");

    const user = useSelector((state) => state.user.value);

    if (!user) {
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
            <div className="communication-content">
                <CreateUserPost
                    user={user}
                    owner={user?.username}
                    t={t}
                />
                <FeedParticipants
                    user={user}
                    t={t}
                />
            </div>
            <CommunicationMenu
                currentMenuItem={0}
            />
        </>
    );
}

export default Feed;