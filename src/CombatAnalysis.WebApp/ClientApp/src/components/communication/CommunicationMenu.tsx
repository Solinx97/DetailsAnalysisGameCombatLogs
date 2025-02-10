import { faAddressBook, faBars, faCalendar, faCheck, faComment, faComments, faLaptop, faPeopleLine, faPerson, faRss, faSquareMinus, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Fragment } from "react";
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { updateMenu } from '../../store/slicers/CommunityMenuSlice';
import { CommunicationMenuProps } from '../../types/components/communication/CommunicationMenuProps';
import { MenuItem } from '../../types/components/communication/MenuItem';

import '../../styles/communication/communication.scss';

const CommunicationMenu: React.FC<CommunicationMenuProps> = ({ currentMenuItem, hasSubMenu }) => {
    const { t } = useTranslation("communication/communication");

    const menu = useSelector((state: any) => state.communityMenu.value);

    const dispatch = useDispatch();
    const navigate = useNavigate();

    const maxWidth = 425;
    const screenSize = {
        width: window.innerWidth,
        height: window.innerHeight
    };

    const menuItems: MenuItem[] = [
        { id: 0, label: "Feed", disabled: false, navigateTo: "/feed", icon: faRss, subMenu: null },
        { id: 1, label: "Chats", disabled: false, navigateTo: "/chats", icon: faComment, subMenu: null },
        { id: 2, label: "Communities", disabled: false, navigateTo: "/communities", icon: faPeopleLine, subMenu: null },
        { id: 3, label: "Events", disabled: true, navigateTo: "", icon: faCalendar, subMenu: null },
        { id: 4, label: "People", disabled: false, navigateTo: "/people", icon: faPerson, subMenu: null },
        { id: 5, label: "MyEnvironment", disabled: false, navigateTo: "/environment/feed", icon: faLaptop, subMenu:
            [
                { id: 5, label: "MyPosts", disabled: false, navigateTo: "/environment/feed", icon: faRss, subMenu: null },
                { id: 6, label: "Friends", disabled: false, navigateTo: "/environment/friends", icon: faAddressBook, subMenu: null },
                { id: 7, label: "Communities", disabled: false, navigateTo: "/environment/communities", icon: faComments, subMenu: null },
                { id: 8, label: "Profile", disabled: false, navigateTo: "/environment/profile", icon: faUser, subMenu: null }
            ]
        }
    ];

    const renderShortMenuItem = (menuItem: MenuItem) => (
        <li className={`short-menu-item${menuItem.disabled ? ' menu-item_disabled' : currentMenuItem === menuItem.id ? ' short-menu-item_selected' : ''}`}
                onClick={!menuItem.disabled ? () => navigate(menuItem.navigateTo) : undefined}>
            <FontAwesomeIcon
                icon={menuItem.icon}
                title={t(menuItem.label) || ""}
            />
        </li>
    )

    const renderMenuItem = (menuItem: MenuItem) => (
        <li className={`menu-item${menuItem.disabled ? ' menu-item_disabled'
                    : currentMenuItem === menuItem.id || (menuItem.subMenu !== null && menuItem.subMenu.findIndex(x => x.id === currentMenuItem) > 0)
                    ? ' menu-item_selected' : ''}`}
            onClick={!menuItem.disabled ? () => navigate(menuItem.navigateTo) : undefined}>
            <div className="title">{t(menuItem.label)}</div>
        </li>
    )

    const renderSubMenuByType = (subMenuItems: MenuItem[]) => {
        if (menu === 0) {
            return renderSubMenu(subMenuItems);
        }
        else {
            return renderShortSubMenu(subMenuItems);
        }
    }

    const renderShortSubMenu = (subMenuItems: MenuItem[]) => (
        subMenuItems.map((subItem: any) => (
            <li key={subItem.id} className={`short-menu-item ${currentMenuItem === subItem.id ? "short-menu-item_selected" : ""}`}
                    onClick={!subItem.disabled ? () => navigate(subItem.navigateTo) : undefined}>
                <FontAwesomeIcon
                    icon={subItem.icon}
                    title={t(subItem.label) || ""}
                />
            </li>
        ))
    )

    const renderSubMenu = (subMenuItems: MenuItem[]) => (
        subMenuItems.map((subItem: any) => (
            <li key={subItem.id} className="menu-item"
                    onClick={!subItem.disabled ? () => navigate(subItem.navigateTo) : undefined}>
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
                    {hasSubMenu &&
                        <ul className="sub-menu">
                            {menuItems.map(item => (
                                <Fragment key={item.id}>
                                    {item.subMenu && renderShortSubMenu(item.subMenu)}
                                </Fragment>
                            ))}
                        </ul>
                    }
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
        <div className={`communication-${menu === -1 ? "hide-" : menu === 1 ? "short-" : ""}menu`}>
            <div className="communication-menu__title">
                <FontAwesomeIcon
                    icon={faBars}
                    title={t("Menu") || ""}
                    onClick={menu === -1 ? () => dispatch(updateMenu(0)) : () => dispatch(updateMenu(menu === 1 ? 0 : 1))}
                />
            </div>
            {menu > -1 &&
                <>
                <div className={`communication-${menu === 1 ? "short-" : ""}menu__container`}>
                        {hasSubMenu &&
                            <ul className="sub-menu">
                                {menuItems.map(item => (
                                    <Fragment key={item.id}>
                                        {item.subMenu && renderSubMenuByType(item.subMenu)}
                                    </Fragment>
                                ))}
                            </ul>
                        }
                        <ul className="main-menu">
                            {menuItems.map(item => (
                                <Fragment key={item.id}>
                                    {menu === 0 ? renderMenuItem(item) : renderShortMenuItem(item)}
                                </Fragment>
                            ))}
                        </ul>
                    </div>
                    <div className="communication-menu__hide">
                        <FontAwesomeIcon
                            icon={faSquareMinus}
                            title={t("Menu") || ""}
                            onClick={() => dispatch(updateMenu(-1))}
                        />
                    </div>
                </>
            }
        </div>
    );
}

export default CommunicationMenu;