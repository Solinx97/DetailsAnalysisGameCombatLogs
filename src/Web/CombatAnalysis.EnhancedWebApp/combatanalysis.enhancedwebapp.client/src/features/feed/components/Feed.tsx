import type { RootState } from '@/app/Store';
import Loading from '@/shared/components/Loading';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import CommunicationMenu from './CommunicationMenu';
import FeedParticipants from './FeedParticipants';
import CreateUserPost from './post/CreateUserPost';
import { useState } from 'react';

import './Feed.scss';

const Feed: React.FC = () => {
    const { t } = useTranslation('communication/feed');

    const [lastCheck, setLastCheck] = useState((new Date()).toISOString());
    const [feedVersion, setFeedVersion] = useState(1);

    const myself = useSelector((state: RootState) => state.user.value);

    return (
        <>
            <div className="communication-content">
                {!myself
                    ? <Loading />
                    : <>
                        <CreateUserPost
                            user={myself}
                            owner={myself.username}
                            feedVersion={feedVersion}
                            t={t}
                        />
                        <FeedParticipants
                            myself={myself}
                            lastCheck={lastCheck}
                            setLastCheck={setLastCheck}
                            feedVersion={feedVersion}
                            setFeedVersion={setFeedVersion}
                            t={t}
                        />
                    </>
                }
            </div>
            <CommunicationMenu
                currentMenuItem={0}
                hasSubMenu={false}
            />
        </>
    );
}

export default Feed;