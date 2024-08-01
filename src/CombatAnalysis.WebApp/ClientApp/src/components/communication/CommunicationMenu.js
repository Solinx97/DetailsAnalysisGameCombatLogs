import { faAddressBook, faBars, faCalendar, faCheck, faComment, faComments, faLaptop, faPeopleLine, faPerson, faRss, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Fragment } from "react";
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { updateMenu } from '../../store/slicers/CommunityMenuSlice';

import '../../styles/communication/communication.scss';

const CommunicationMenu = ({ currentMenuItem, setMenuItem }) => {
    const { t } = useTranslation("communication/communication");

    const menu = useSelector((state) => state.communityMenu.value);

    const dispatch = useDispatch();
    const navigate = useNavigate();

    const maxWidth = 425;
    const screenSize = {
        width: window.innerWidth,
        height: window.innerHeight
    };

    const menuItems = [
        { id: 0, label: "Feed", navigateTo: "/feed", icon: faRss },
        { id: 1, label: "Chats", navigateTo: "/chats", icon: faComment },
        { id: 3, label: "Communities", navigateTo: "/communities", icon: faPeopleLine },
        { id: 6, label: "Events", disabled: true, icon: faCalendar },
        { id: 7, label: "People", navigateTo: "/people", icon: faPerson },
        {
            id: 8, label: "MyEnvironment", navigateTo: "/environment", icon: faLaptop, subMenu: [
                { id: 8, label: "MyPosts", icon: faRss },
                { id: 9, label: "Friends", icon: faAddressBook },
                { id: 10, label: "Communities", icon: faComments },
                { id: 12, label: "Profile", icon: faUser }
            ]
        }
    ];

    const renderShortMenuItem = (item) => (
        <li className={`short-menu-item${item.disabled ? ' menu-item_disabled' : (currentMenuItem >= item.id && item.subMenu !== undefined || currentMenuItem === item.id) ? ' short-menu-item_selected' : ''}`}
            onClick={!item.disabled ? () => navigate(item.navigateTo) : undefined}>
            <FontAwesomeIcon
                icon={item.icon}
                title={t(item.label)}
            />
        </li>
    )

    const renderMenuItem = (item) => (
        <li className={`menu-item${item.disabled ? ' menu-item_disabled' : ((currentMenuItem >= item.id && item.subMenu !== undefined || currentMenuItem === item.id) && !item.createLabel) ? ' menu-item_selected' : ''}`}
            onClick={!item.disabled ? () => navigate(item.navigateTo) : undefined}>
            <div className="title">{t(item.label)}</div>
        </li>
    )

    const renderShortSubMenu = (subMenu) => (
        subMenu.map(subItem => (
            <li key={subItem.id} className={`short-menu-item ${currentMenuItem === subItem.id ? "short-menu-item_selected" : ""}`} onClick={() => setMenuItem(subItem.id)}>
                <FontAwesomeIcon
                    icon={subItem.icon}
                    title={t(subItem.label)}
                />
            </li>
        ))
    )

    const renderSubMenu = (subMenu) => (
        subMenu.map(subItem => (
            <li key={subItem.id} className="menu-item" onClick={() => setMenuItem(subItem.id)}>
                {currentMenuItem === subItem.id &&
                    <FontAwesomeIcon
                        icon={faCheck}
                    />
                }
                <div className="title">{t(subItem.label)}</div>
            </li>
        ))
    )

    if (screenSize.width <= maxWidth) {
        return (
            <div className="communication-short-menu">
                <div className="communication-short-menu__container">
                    <ul className="sub-menu">
                        {menuItems.map(item => (
                            <Fragment key={item.id}>
                                {item.subMenu && currentMenuItem >= 8 && renderShortSubMenu(item.subMenu)}
                            </Fragment>
                        ))}
                    </ul>
                    <ul className="main-menu">
                        {menuItems.map(item => (
                            <Fragment key={item.id}>
                                {renderShortMenuItem(item)}
                            </Fragment>
                        ))}
                    </ul>
                </div>
            </div>
        );
    }

    if (menu) {
        return (
            <div className="communication-short-menu">
                <div className="communication-short-menu__title">
                    <div>
                        <FontAwesomeIcon
                            icon={faBars}
                            title={t("Menu")}
                            onClick={() => dispatch(updateMenu(!menu))}
                        />
                    </div>
                </div>
                <div className="communication-short-menu__container">
                    <ul className="sub-menu">
                        {menuItems.map(item => (
                            <Fragment key={item.id}>
                                {item.subMenu && currentMenuItem >= 8 && renderShortSubMenu(item.subMenu)}
                            </Fragment>
                        ))}
                    </ul>
                    <ul className="main-menu">
                        {menuItems.map(item => (
                            <Fragment key={item.id}>
                                {renderShortMenuItem(item)}
                            </Fragment>
                        ))}
                    </ul>
                </div>
            </div>
        );
    }

    return (
        <div className="communication-menu">
            <div className="communication-menu__title">
                <div>
                    <FontAwesomeIcon
                        icon={faBars}
                        title={t("Menu")}
                        onClick={() => dispatch(updateMenu(!menu))}
                    />
                </div>
            </div>
            <div className="communication-menu__container">
                <ul className="sub-menu">
                    {menuItems.map(item => (
                        <Fragment key={item.id}>
                            {item.subMenu && currentMenuItem >= 8 && renderSubMenu(item.subMenu)}
                        </Fragment>
                    ))}
                </ul>
                <ul className="main-menu">
                    {menuItems.map(item => (
                        <Fragment key={item.id}>
                            {renderMenuItem(item)}
                        </Fragment>
                    ))}
                </ul>
            </div>
        </div>
    );
}

export default CommunicationMenu;