import { faCircleCheck } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import AddPeople from '../../AddPeople';
import CommonItem from "../chats/createGroupChat/CommonItem";
import ItemConnector from '../chats/createGroupChat/ItemConnector';
import RulesItem from "../chats/createGroupChat/RulesItem";
import CommunityMembers from './CommunityMembers';

import '../../../styles/communication/community/communityMenu.scss';

const CommunityMenu = ({ setShowMenu, customer, community }) => {
    const { t } = useTranslation("communication/chats/createGroupChat");

    const [itemIndex, seItemIndex] = useState(0);

    const [chatName, setChatName] = useState("");
    const [chatShortName, setChatShortName] = useState("");

    const [peopleIdToJoin, setPeopleIdToJoin] = useState([]);

    const changeMenuItem = (index) => {
        seItemIndex(index);
    }

    const openAddPeople = () => {
        seItemIndex(2);
    }

    return (
        <div className="communication__content community-menu">
            <div className="community-menu__content">
                <ul className="community-menu__menu">
                    <li className="menu-item" onClick={() => changeMenuItem(0)}>
                        {itemIndex === 0 &&
                            <FontAwesomeIcon
                                className="menu-item__passed"
                                icon={faCircleCheck}
                            />
                        }
                        <div>{t("Main")}</div>
                    </li>
                    <li className="menu-item" onClick={() => changeMenuItem(1)}>
                        {itemIndex === 1 &&
                            <FontAwesomeIcon
                                className="menu-item__passed"
                                icon={faCircleCheck}
                            />
                        }
                        <div>{t("Members")}</div>
                    </li>
                    <li className="menu-item" onClick={() => changeMenuItem(2)}>
                        {itemIndex === 2 &&
                            <FontAwesomeIcon
                                className="menu-item__passed"
                                icon={faCircleCheck}
                            />
                        }
                        <div>{t("InvitePeople")}</div>
                    </li>
                    <li className="menu-item" style={{ opacity: 0.5 }}>
                        {itemIndex === 3 &&
                            <FontAwesomeIcon
                                className="menu-item__passed"
                                icon={faCircleCheck}
                            />
                        }
                        <div>{t("Permisions")}</div>
                    </li>
                    <li className="menu-item" onClick={() => changeMenuItem(4)}>
                        {itemIndex === 4 &&
                            <FontAwesomeIcon
                                className="menu-item__passed"
                                icon={faCircleCheck}
                            />
                        }
                        <div>{t("Rules")}</div>
                    </li>
                </ul>
                <div className="community-menu__items">
                    {itemIndex === 0 &&
                        <>
                            <CommonItem
                                chatName={chatName}
                                setChatName={setChatName}
                                chatShortName={chatShortName}
                                setChatShortName={setChatShortName}
                                connector={
                                    <ItemConnector
                                        connectorType={0}
                                    />
                                }
                        />
                            <div className="actions">
                                <input type="submit" value={t("Update")} className="btn btn-success" />
                            </div>
                        </>
                    }
                    {itemIndex === 1 &&
                        <div className="members">
                            <CommunityMembers
                                community={community}
                                customer={customer}
                                handleShowAddPeople={openAddPeople}
                                showAddPeople={false}
                            />
                        </div>
                    }
                    {itemIndex === 2 &&
                        <>
                            <div className="create-group-chat__item">
                                <AddPeople
                                    customer={customer}
                                    communityUsersId={[customer.id]}
                                    peopleToJoin={peopleIdToJoin}
                                    setPeopleToJoin={setPeopleIdToJoin}
                                />
                                <ItemConnector
                                    connectorType={0}
                                />
                            </div>
                            <div className="actions">
                                <input type="submit" value={t("Apply")} className="btn btn-success" />
                            </div>
                        </>
                    }
                    {itemIndex === 4 &&
                        <>
                            <RulesItem
                                connector={
                                    <ItemConnector
                                        connectorType={0}
                                    />
                                }
                            />
                            <div className="actions">
                                <input type="submit" value={t("Update")} className="btn btn-success" />
                            </div>
                        </>
                    }
                </div>
            </div>
            <div className="finish-create">
                <input type="submit" value={t("Cancel")} className="btn btn-secondary" onClick={() => setShowMenu((item) => !item)} />
            </div>
        </div>
    );
}

export default CommunityMenu;