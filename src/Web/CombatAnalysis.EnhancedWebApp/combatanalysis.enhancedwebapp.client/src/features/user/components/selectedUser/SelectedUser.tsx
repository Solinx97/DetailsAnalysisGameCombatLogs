import { APP_CONFIG } from '@/config/appConfig';
import type { RootState } from '@/app/Store';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import { faComments, faEnvelopesBulk, faUser, faUserGroup } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import useFetchUserPosts from '@/features/feed/hooks/useFetchUserPosts';
import { useEffect, useRef, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router-dom';
import UserPost from '../../../feed/components/post/UserPost';
import Friends from '../userEnvironment/Friends';
import SelectedUserCommunities from './SelectedUserCommunities';
import SelectedUserProfile from './SelectedUserProfile';
import { useGetUserByIdQuery } from '../../api/Account.api';

import './SelectedUser.scss';

const SelectedUser: React.FC = () => {
    const { t } = useTranslation('communication/people/user');

    const myself = useSelector((state: RootState) => state.user.value);

    const location = useLocation();

    const [personId, setPersonId] = useState<string>("0");
    const [currentMenuItem, setMenuItem] = useState(0);

    const [page, setPage] = useState(1);;
    const [hasMore, setHasMore] = useState(false);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.userPostSize ?? 5);

    const { posts, isLoading, isFetching } = useFetchUserPosts(page, pageSizeRef.current, personId);
    const { data: person, isLoading: personIsLoading } = useGetUserByIdQuery(personId);

    useEffect(() => {
        const queryParams = new URLSearchParams(location.search);
        const id: string = queryParams.get("id") || "0";

        setPersonId(id);
    }, []);

    useEffect(() => {
        if (!posts) {
            return;
        }

        setHasMore(((page - 1) * pageSizeRef.current) < posts.length);
    }, [page, posts]);

    if (!myself || !posts || isLoading || !person || personIsLoading) {
        return (<></>);
    }

    return (
        <div className="communication">
            <CommunicationMenu
                currentMenuItem={7}
                hasSubMenu={false}
            />
            <div className="communication-content user">
                <div className="user-information__username">
                    {person.username}
                </div>
                <div className="user__container">
                    <div className="menu-container">
                        <ul className="user__menu">
                            <li className="sub-menu" onClick={() => setMenuItem(0)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 0 ? "_active" : ""}`}
                                    title={t("Profile") || ""}
                                    icon={faUser}
                                />
                            </li>
                            <li className="sub-menu" onClick={() => setMenuItem(1)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 1 ? "_active" : ""}`}
                                    title={t("Posts") || ""}
                                    icon={faEnvelopesBulk}
                                />
                            </li>
                            <li className="sub-menu" onClick={() => setMenuItem(2)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 2 ? "_active" : ""}`}
                                    title={t("Friends") || ""}
                                    icon={faUserGroup}
                                />
                            </li>
                            <li className="sub-menu" onClick={() => setMenuItem(3)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 3 ? "_active" : ""}`}
                                    title={t("Communities") || ""}
                                    icon={faComments}
                                />
                            </li>
                        </ul>
                    </div>
                    <div className="user__content">
                        {currentMenuItem === 0 &&
                            <SelectedUserProfile
                                person={person}
                            />
                        }
                        {currentMenuItem === 1 &&
                            <ul className="posts">
                                {posts.length === 0
                                    ? <div>{t("Empty")}</div>
                                    : posts.map(post => (
                                        <li key={post.id}>
                                            <UserPost
                                                myself={myself}
                                                post={post}
                                            />
                                        </li>
                                    ))
                                }
                                {posts.length > 0 &&
                                    < li className="posts__item">
                                        <InfiniteScrollTrigger
                                            onLoadMore={() => setPage(p => p + 1)}
                                            hasMore={hasMore}
                                            isLoading={isFetching}
                                        />
                                    </li>
                                }
                            </ul>
                        }
                        {currentMenuItem === 2 &&
                            <Friends />
                        }
                        {currentMenuItem === 3 &&
                            <SelectedUserCommunities
                                user={person}
                                t={t}
                            />
                        }
                    </div>
                </div>
            </div>
        </div >
    );
}

export default SelectedUser;