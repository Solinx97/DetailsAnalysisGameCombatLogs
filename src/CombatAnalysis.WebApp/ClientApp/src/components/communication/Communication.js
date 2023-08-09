import { faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import '../../styles/communication/communication.scss';

const Communication = ({ currentMenuItem }) => {
    const { t } = useTranslation("communication/communication");

    const navigate = useNavigate();

    const render = () => {
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
                <li className="menu-item" onClick={() => navigate("/chats")}>
                    {currentMenuItem === 1 &&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                        />
                    }
                    <div className="title">{t("Chats")}</div>
                </li>
                <div>
                    <li className="menu-item" onClick={() => navigate("/communities")}>
                        {currentMenuItem === 2 &&
                            <FontAwesomeIcon
                                icon={faCircleArrowRight}
                            />
                        }
                        <div className="title">{t("Communities")}</div>
                    </li>
                    {currentMenuItem === 6 &&
                        <li className="menu-item selected-community">
                            <FontAwesomeIcon
                                icon={faCircleArrowRight}
                            />
                            <div className="title">Temp</div>
                        </li>
                    }
                </div>
                <li className="menu-item_disabled">
                    {currentMenuItem === 3 &&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                        />
                    }
                    <div className="title">{t("Events")}</div>
                </li>
                <li className="menu-item" onClick={() => navigate("/people")}>
                    {currentMenuItem === 4 &&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                        />
                    }
                    <div className="title">{t("People")}</div>
                </li>
                <li className="menu-item" onClick={() => navigate("/environment")}>
                    {currentMenuItem === 5 &&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                        />
                    }
                    <div className="title">{t("MyEnvironment")}</div>
                </li>
            </ul>
        );
    }

    return render();
}

export default memo(Communication);