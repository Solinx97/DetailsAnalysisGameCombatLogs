import { faArrowsLeftRightToLine, faArrowsToCircle, faCheck, faComments, faEnvelopesBulk, faUser, faUserGroup } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLazyGetUserByIdQuery } from '../../../store/api/Account.api';
import { useLazyUserPostSearchByUserIdQuery } from '../../../store/api/ChatApi';
import { useLazyGetPostByIdQuery } from '../../../store/api/communication/Post.api';
import CommunicationMenu from "../CommunicationMenu";
import Post from '../Post';
import Friends from '../myEnvironment/Friends';
import SelectedUserCommunities from './SelectedUserCommunities';
import SelectedUserProfile from './SelectedUserProfile';

import '../../../styles/communication/people/selectedUser.scss';

const SelectedUser = () => {
    const { t } = useTranslation("communication/people/user");

    const [getUserPosts] = useLazyUserPostSearchByUserIdQuery();
    const [getPostById] = useLazyGetPostByIdQuery();
    const [getUserById] = useLazyGetUserByIdQuery();

    const [personId, setPersonId] = useState(0);
    const [person, setPerson] = useState(null);
    const [currentMenuItem, setMenuItem] = useState(0);
    const [shortMenu, setShortMenu] = useState(false);
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
            <div className="communication__content user">
                <div className="user-information__username">
                    {person?.username}
                </div>
                <div className="user__container">
                    <ul className="user__menu">
                        <li className="sub-menu" onClick={() => setMenuItem(0)}>
                            <FontAwesomeIcon
                                className={`current${currentMenuItem === 0 ? "_active" : ""}`}
                                title={t("Profile")}
                                icon={faUser}
                            />
                            {!shortMenu &&
                                <>
                                    <div className="title">{t("Profile")}</div>
                                    {currentMenuItem === 0 &&
                                        <FontAwesomeIcon
                                            icon={faCheck}
                                        />
                                    }
                                </>
                            }
                        </li>
                        <li className="sub-menu" onClick={() => setMenuItem(1)}>
                            <FontAwesomeIcon
                                className={`current${currentMenuItem === 1 ? "_active" : ""}`}
                                title={t("Posts")}
                                icon={faEnvelopesBulk}
                            />
                            {!shortMenu &&
                                <>
                                    <div className="title">{t("Posts")}</div>
                                    {currentMenuItem === 1 &&
                                        <FontAwesomeIcon
                                            icon={faCheck}
                                        />
                                    }
                                </>
                            }
                        </li>
                        <li className="sub-menu" onClick={() => setMenuItem(2)}>
                            <FontAwesomeIcon
                                className={`current${currentMenuItem === 2 ? "_active" : ""}`}
                                title={t("Friends")}
                                icon={faUserGroup}
                            />
                            {!shortMenu &&
                                <>
                                    <div className="title">{t("Friends")}</div>
                                    {currentMenuItem === 2 &&
                                        <FontAwesomeIcon
                                            icon={faCheck}
                                        />
                                    }
                                </>
                            }
                        </li>
                        <li className="sub-menu" onClick={() => setMenuItem(3)}>
                            <FontAwesomeIcon
                                className={`current${currentMenuItem === 3 ? "_active" : ""}`}
                                title={t("Communities")}
                                icon={faComments}
                            />
                            {!shortMenu &&
                                <>
                                    <div className="title">{t("Communities")}</div>
                                    {currentMenuItem === 3 &&
                                        <FontAwesomeIcon
                                            icon={faCheck}
                                        />
                                    }
                                </>
                            }
                        </li>
                        {shortMenu
                            ? <li className="sub-menu control">
                                <FontAwesomeIcon
                                    icon={faArrowsLeftRightToLine}
                                    title={t("FullMenu")}
                                    onClick={() => setShortMenu((item) => !item)}
                                />
                            </li>
                            : <li className="sub-menu control">
                                <FontAwesomeIcon
                                    icon={faArrowsToCircle}
                                    title={t("ShortMenu")}
                                    onClick={() => setShortMenu((item) => !item)}
                                />
                            </li>
                        }
                    </ul>
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
                                            <Post
                                                customer={person}
                                                data={post}
                                                deletePostAsync={null}
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