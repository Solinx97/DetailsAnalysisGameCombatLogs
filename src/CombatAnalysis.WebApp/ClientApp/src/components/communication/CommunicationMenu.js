import { faCheck, faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import '../../styles/communication/communication.scss';

const CommunicationMenu = ({ currentMenuItem, setMenuItem }) => {
    const { t } = useTranslation("communication/communication");

    const navigate = useNavigate();

    const navigateToFeed = () => navigate("/feed");
    const navigateToChats = () => navigate("/chats");
    const navigateToCreateChat = () => navigate("/chats/create");
    const navigateToCommunities = () => navigate("/communities");
    const navigateToCreateCommunity = () => navigate("/communities/create");
    const navigateToPeople = () => navigate("/people");
    const navigateToEnvironment = () => navigate("/environment");

    return (
        <ul className="communication__menu">
            <li className="menu-item" onClick={navigateToFeed}>
                {currentMenuItem === 0 &&
                    <FontAwesomeIcon
                        icon={faCircleArrowRight}
                    />
                }
                <div className="title">{t("Feed")}</div>
            </li>
            <li className="extended-menu-item">
                <div className="menu-item" onClick={navigateToChats}>
                    {(currentMenuItem === 1 || currentMenuItem === 2)&&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                        />
                    }
                    <div className="title">{t("Chats")}</div>
                </div>
                <div className="create" onClick={navigateToCreateChat}>{t("Create")}</div>
            </li>
            <li className="extended-menu-item">
                <div className="menu-item" onClick={navigateToCommunities}>
                    {(currentMenuItem === 3 || currentMenuItem === 4) &&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                        />
                    }
                    <div className="title">{t("Communities")}</div>
                </div>
                <div className="create" onClick={navigateToCreateCommunity}>{t("Create")}</div>
            </li>
            <li className="menu-item_disabled">
                {currentMenuItem === 6 &&
                    <FontAwesomeIcon
                        icon={faCircleArrowRight}
                    />
                }
                <div className="title">{t("Events")}</div>
            </li>
            <li className="menu-item" onClick={navigateToPeople}>
                {currentMenuItem === 7 &&
                    <FontAwesomeIcon
                        icon={faCircleArrowRight}
                    />
                }
                <div className="title">{t("People")}</div>
            </li>
            <div>
                <li className="menu-item" onClick={navigateToEnvironment}>
                    {currentMenuItem >= 8 &&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                        />
                    }
                    <div className="title">{t("MyEnvironment")}</div>
                </li>
                {currentMenuItem >= 8 &&
                    <>
                        <li className="menu-item sub-menu" onClick={() => setMenuItem(8)}>
                            {currentMenuItem === 8 &&
                                <FontAwesomeIcon
                                    icon={faCheck}
                                />
                            }
                            <div className="title">{t("MyPosts")}</div>
                        </li>
                        <li className="menu-item sub-menu" onClick={() => setMenuItem(9)}>
                            {currentMenuItem === 9 &&
                                <FontAwesomeIcon
                                    icon={faCheck}
                                />
                            }
                            <div className="title">{t("Friends")}</div>
                        </li>
                        <li className="menu-item sub-menu" onClick={() => setMenuItem(10)}>
                            {currentMenuItem === 10 &&
                                <FontAwesomeIcon
                                    icon={faCheck}
                                />
                            }
                            <div className="title">{t("Communities")}</div>
                        </li>
                        <li className="menu-item sub-menu" onClick={() => setMenuItem(12)}>
                            {currentMenuItem === 12 &&
                                <FontAwesomeIcon
                                    icon={faCheck}
                                />
                            }
                            <div className="title">{t("Profile")}</div>
                        </li>
                    </>
                }
            </div>
        </ul>
    );
}

export default CommunicationMenu;