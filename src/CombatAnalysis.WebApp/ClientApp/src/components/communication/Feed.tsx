import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import Loading from '../Loading';
import CommunicationMenu from './CommunicationMenu';
import CreateUserPost from './post/CreateUserPost';
import FeedParticipants from './FeedParticipants';

const Feed: React.FC = () => {
    const { t } = useTranslation("communication/feed");

    const me = useSelector((state: any) => state.user.value);

    if (!me) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={0}
                    hasSubMenu={false}
                />
                <Loading />
            </>
        );
    }

    return (
        <>
            <div className="communication-content">
                <CreateUserPost
                    user={me}
                    owner={me?.username}
                    t={t}
                />
                <FeedParticipants
                    meId={me?.id}
                    t={t}
                />
            </div>
            <CommunicationMenu
                currentMenuItem={0}
                hasSubMenu={false}
            />
        </>
    );
}

export default Feed;