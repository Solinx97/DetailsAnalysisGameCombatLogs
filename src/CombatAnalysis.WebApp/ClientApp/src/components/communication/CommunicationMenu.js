import { faCheck, faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { Fragment } from "react";

import '../../styles/communication/communication.scss';

const CommunicationMenu = ({ currentMenuItem, setMenuItem }) => {
    const { t } = useTranslation("communication/communication");

    const navigate = useNavigate();

    const menuItems = [
        { id: 0, label: "Feed", navigateTo: "/feed" },
        { id: 1, label: "Chats", navigateTo: "/chats", createLabel: "Create", createNavigateTo: "/chats/create" },
        { id: 3, label: "Communities", navigateTo: "/communities", createLabel: "Create", createNavigateTo: "/communities/create" },
        { id: 6, label: "Events", disabled: true },
        { id: 7, label: "People", navigateTo: "/people" },
        {
            id: 8, label: "MyEnvironment", navigateTo: "/environment", subMenu: [
                { id: 8, label: "MyPosts" },
                { id: 9, label: "Friends" },
                { id: 10, label: "Communities" },
                { id: 12, label: "Profile" }
            ]
        }
    ];
    const renderMenuItem = (item) => (
        <li className={`${item.createLabel ? "extended-" : ""}menu-item${item.disabled ? '_disabled' : ''}`}
            onClick={!item.disabled && !item.createLabel ? () => navigate(item.navigateTo) : undefined}>
            {item.createLabel &&
                <div className="menu-item" onClick={() => navigate(item.navigateTo)}>
                    {currentMenuItem === item.id &&
                        <FontAwesomeIcon
                            icon={faCircleArrowRight}
                        />
                    }
                    <div className="title">{t(item.label)}</div>
                </div>
            }
            {(currentMenuItem === item.id && !item.createLabel) &&
                <FontAwesomeIcon
                    icon={faCircleArrowRight}
                />
            }
            {item.createLabel
                ? <div className="create" onClick={() => navigate(item.createNavigateTo)}>{t(item.createLabel)}</div>
                : <div className="title">{t(item.label)}</div>
            }
        </li>
    )

    const renderSubMenu = (subMenu) => (
        subMenu.map(subItem => (
            <li key={subItem.id} className="menu-item sub-menu" onClick={() => setMenuItem(subItem.id)}>
                {currentMenuItem === subItem.id &&
                    <FontAwesomeIcon
                        icon={faCheck}
                    />
                }
                <div className="title">{t(subItem.label)}</div>
            </li>
        ))
    )

    return (
        <ul className="communication__menu">
            {menuItems.map(item => (
                <Fragment key={item.id}>
                    {renderMenuItem(item)}
                    {item.subMenu && currentMenuItem >= 8 && renderSubMenu(item.subMenu)}
                </Fragment>
            ))}
        </ul>
    );
}

export default CommunicationMenu;