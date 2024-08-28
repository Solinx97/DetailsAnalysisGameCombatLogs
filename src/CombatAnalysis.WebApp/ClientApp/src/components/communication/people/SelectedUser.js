import { faComments, faEnvelopesBulk, faUser, faUserGroup } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLazyGetUserByIdQuery } from '../../../store/api/Account.api';
import { useLazyGetUserPostsByUserIdQuery } from '../../../store/api/CommunityApi';
import { useLazyGetUserPostByIdQuery } from '../../../store/api/communication/UserPost.api';
import CommunicationMenu from "../CommunicationMenu";
import UserPost from '../post/UserPost';
import Friends from '../myEnvironment/Friends';
import SelectedUserCommunities from './SelectedUserCommunities';
import SelectedUserProfile from './SelectedUserProfile';

import '../../../styles/communication/people/selectedUser.scss';

const SelectedUser = () => {
    const { t } = useTranslation("communication/people/user");

    const [getUserPosts] = useLazyGetUserPostsByUserIdQuery();
    const [getPostById] = useLazyGetUserPostByIdQuery();
    const [getUserById] = useLazyGetUserByIdQuery();

    const [personId, setPersonId] = useState(0);
    const [person, setPerson] = useState(null);
    const [currentMenuItem, setMenuItem] = useState(0);
    const [allPosts, setAllPosts] = useState([]);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setPersonId(queryParams.get("id"));
    }, [])

    useEffect(() => {
        if (personId === 0) {
            return;
        }

        const getUser = async () => {
            await getUserByIdAsync();
        }

        getUser();
    }, [personId])

    const getUserByIdAsync = async () => {
        const user = await getUserById(personId);

        if (user.data !== undefined) {
            setPerson(user.data);

            getAllPostsAsync(user.data.id);
        }
    }

    const getAllPostsAsync = async (userId) => {
        const userPosts = await getUserPosts(userId);

        if (userPosts.data !== undefined) {
            const userPersonalPosts = await getUserPostsAsync(userPosts.data);
            setAllPosts(userPersonalPosts);
        }
    }

    const getUserPostsAsync = async (userPosts) => {
        const userPersonalPosts = [];

        for (let i = 0; i < userPosts.length; i++) {
            const post = await getPostById(userPosts[i].postId);

            if (post.data !== undefined) {
                userPersonalPosts.push(post.data);
            }
        }

        return userPersonalPosts;
    }

    return (
        <div className="communication">
            <CommunicationMenu
                currentMenuItem={7}
            />
            <div className="communication-content user">
                <div className="user-information__username">
                    {person?.username}
                </div>
                <div className="user__container">
                    <div className="menu-container">
                        <ul className="user__menu">
                            <li className="sub-menu" onClick={() => setMenuItem(0)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 0 ? "_active" : ""}`}
                                    title={t("Profile")}
                                    icon={faUser}
                                />
                            </li>
                            <li className="sub-menu" onClick={() => setMenuItem(1)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 1 ? "_active" : ""}`}
                                    title={t("Posts")}
                                    icon={faEnvelopesBulk}
                                />
                            </li>
                            <li className="sub-menu" onClick={() => setMenuItem(2)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 2 ? "_active" : ""}`}
                                    title={t("Friends")}
                                    icon={faUserGroup}
                                />
                            </li>
                            <li className="sub-menu" onClick={() => setMenuItem(3)}>
                                <FontAwesomeIcon
                                    className={`current${currentMenuItem === 3 ? "_active" : ""}`}
                                    title={t("Communities")}
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
                                {allPosts.length === 0
                                    ? <div>{t("Empty")}</div>
                                    : allPosts?.map(post => (
                                        <li key={post.id}>
                                            <UserPost
                                                user={person}
                                                post={post}
                                            />
                                        </li>
                                    ))
                                }
                            </ul>
                        }
                        {currentMenuItem === 2 &&
                            <Friends
                                customer={person}
                                requestsToConnect={null}
                                allowRemoveFriend={false}
                            />
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
        </div>
    );
}

export default SelectedUser;