import { faCheck, faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import '../../styles/communication/communication.scss';

const Communication = ({ currentMenuItem, setMenuItem, selectedCommunityName }) => {
    const { t } = useTranslation("communication/communication");

    const navigate = useNavigate();

    return (
        <ul className="communication__menu">
            <li className="menu-item" onClick={() => navigate("/feed")}>
                {currentMenuItem === 0 &&
                    <FontAwesomeIcon
                        icon={faCircleArrowRight}
                    />
                }
                <div className="title">{t("Feed")}</div>
            </li>
            <div>
                <li className="menu-item" onClick={() => navigate("/chats")}>
                    {currentMenuItem === 1 &&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                        />
                    }
                    <div className="title">{t("Chats")}</div>
                </li>
                {(currentMenuItem === 1 || currentMenuItem === 2) &&
                    <>
                        <li className="menu-item sub-menu">
                            {currentMenuItem === 1 &&
                                <FontAwesomeIcon
                                    icon={faCheck}
                                />
                            }
                            <div className="title">{t("Chats")}</div>
                        </li>
                        <li className="menu-item sub-menu" onClick={() => navigate("/chats/create")}>
                            {currentMenuItem === 2 &&
                                <FontAwesomeIcon
                                    icon={faCheck}
                                />
                            }
                            <div className="title">{t("CreateChat")}</div>
                        </li>
                    </>
                }
            </div>
            <div>
                <li className="menu-item" onClick={() => navigate("/communities")}>
                    {currentMenuItem === 3 &&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                        />
                    }
                    <div className="title">{t("Communities")}</div>
                </li>
                {(currentMenuItem >= 3 && currentMenuItem <= 5) &&
                    <>
                        <li className="menu-item sub-menu">
                            {currentMenuItem === 3 &&
                                <FontAwesomeIcon
                                    icon={faCheck}
                                />
                            }
                            <div className="title">{t("Communities")}</div>
                        </li>
                        <li className="menu-item_disabled sub-menu">
                            {currentMenuItem === 4 &&
                                <FontAwesomeIcon
                                    icon={faCheck}
                                />
                            }
                            <div className="title">{t("CreateCommunity")}</div>
                        </li>
                        {currentMenuItem === 5 &&
                            <li className="menu-item sub-menu">
                                <FontAwesomeIcon
                                    icon={faCheck}
                                />
                                <div className="title">{selectedCommunityName}</div>
                            </li>
                        }
                    </>
                }
            </div>
            <li className="menu-item_disabled">
                {currentMenuItem === 6 &&
                    <FontAwesomeIcon
                        icon={faCircleArrowRight}
                    />
                }
                <div className="title">{t("Events")}</div>
            </li>
            <li className="menu-item" onClick={() => navigate("/people")}>
                {currentMenuItem === 7 &&
                    <FontAwesomeIcon
                        icon={faCircleArrowRight}
                    />
                }
                <div className="title">{t("People")}</div>
            </li>
            <div>
                <li className="menu-item" onClick={() => navigate("/environment")}>
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
                        <li className="menu-item_disabled sub-menu">
                            {currentMenuItem === 11 &&
                                <FontAwesomeIcon
                                    icon={faCheck}
                                />
                            }
                            <div className="title">{t("Recomendations")}</div>
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

export default Communication;