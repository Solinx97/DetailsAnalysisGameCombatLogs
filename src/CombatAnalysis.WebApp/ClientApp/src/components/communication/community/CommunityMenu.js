import { faCircleCheck } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import { useRemoveCommunityAsyncMutation } from '../../../store/api/communication/community/Community.api';
import { useLazySearchByUserIdAsyncQuery, useRemoveCommunityUserAsyncMutation } from '../../../store/api/communication/community/CommunityUser.api';
import { useCreateInviteAsyncMutation, useLazyInviteIsExistQuery } from '../../../store/api/communication/community/InviteToCommunity.api';
import AddPeople from '../../AddPeople';
import Members from '../Members';
import CommonItem from "../create/CommonItem";
import CommunityRulesItem from "../create/CommunityRulesItem";
import ItemConnector from '../create/ItemConnector';

import '../../../styles/communication/community/communityMenu.scss';

const successNotificationTimeout = 2000;
const failedNotificationTimeout = 2000;

const CommunityMenu = ({ setShowMenu, customer, community }) => {
    const { t } = useTranslation("communication/community/communityMenu");

    const navigate = useNavigate();

    const [itemIndex, seItemIndex] = useState(0);
    const [chatName, setChatName] = useState("");
    const [chatShortName, setChatShortName] = useState("");
    const [showLeaveFromCommunity, setShowLeaveFromCommunity] = useState(false);
    const [peopleIdToJoin, setPeopleIdToJoin] = useState([]);
    const [showInvitesSuccess, setShowInvitesSuccess] = useState(false);
    const [showInvitesFailed, setShowInvitesFailed] = useState(false);

    const [removeCommunityAsync] = useRemoveCommunityAsyncMutation();
    const [searchByUserIdAsync] = useLazySearchByUserIdAsyncQuery();
    const [removeCommunityUserAsync] = useRemoveCommunityUserAsyncMutation();
    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();
    const [isInviteExistAsync] = useLazyInviteIsExistQuery();
    const { data: communityUsers, isLoading } = useSearchByCommunityIdAsyncQuery(community?.id);

    const leaveFromCommunityAsync = async () => {
        const myCommunityUserId = await searchByUserIdAsync(customer.id);
        const deletedItemCount = await removeCommunityUserAsync(myCommunityUserId);
        if (deletedItemCount.data !== undefined) {
            navigate('/communities');
        }
    }

    const ownerLeaveFromCommunityAsync = async () => {
        const deletedItemCount = await removeCommunityAsync(community.id);
        if (deletedItemCount.data !== undefined) {
            navigate('/communities');
        }
    }

    const changeMenuItem = (index) => {
        seItemIndex(index);
    }

    const checkIfRequestExistAsync = async (peopleId, communityId) => {
        const arg = {
            peopleId: peopleId,
            communityId: communityId
        };

        const isExist = await isInviteExistAsync(arg);
        if (isExist.error !== undefined) {
            return true;
        }

        return isExist.data;
    }

    const createInviteAsync = async () => {
        for (let i = 0; i < peopleIdToJoin.length; i++) {
            const isExist = await checkIfRequestExistAsync(peopleIdToJoin[i], community.id);
            if (isExist) {
                continue;
            }

            const newInviteToCommunity = {
                communityId: community.id,
                toCustomerId: peopleIdToJoin[i].id,
                when: new Date(),
                customerId: customer?.id
            }

            const createdInvite = await createInviteAsyncMut(newInviteToCommunity);
            if (createdInvite.error !== undefined) {
                setShowInvitesFailed(true);

                setTimeout(() => {
                    setShowInvitesFailed(false);
                }, failedNotificationTimeout);

                return;
            }
        }

        setShowInvitesSuccess(true);

        setTimeout(() => {
            setShowInvitesSuccess(false);
        }, successNotificationTimeout);
    }

    const removeUsersAsync = async (peopleToRemove) => {
        for (let i = 0; i < peopleToRemove.length; i++) {
            await removeCommunityUserAsync(peopleToRemove[i].id);
        }
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="communication__content community-menu">
            {showLeaveFromCommunity &&
                <div className="leave-from-community">
                    <div>{t("LeaveAlert")}</div>
                    <div>
                        <div>{t("LeaveConfirm")} <strong>'{community.name}'</strong>?</div>
                    </div>
                    {customer.id === community.customerId
                        ? <>
                            <div className="alert alert-danger" role="alert">
                                {t("LeaveOwnerConfirm")}
                            </div>
                            <div>
                                <button className="btn btn-outline-danger" onClick={async () => await ownerLeaveFromCommunityAsync()}>{t("Leave")}</button>
                                <button className="btn btn-outline-success" onClick={() => setShowLeaveFromCommunity((item) => !item)}>{t("Cancel")}</button>
                            </div>
                        </>
                        : <div>
                            <button className="btn btn-outline-danger" onClick={async () => await leaveFromCommunityAsync()}>{t("Leave")}</button>
                            <button className="btn btn-outline-success" onClick={() => setShowLeaveFromCommunity((item) => !item)}>{t("Cancel")}</button>
                        </div>
                    }
                </div>
            }
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
                    <li className="menu-item-leave">
                        <button className="btn btn-outline-danger" onClick={() => setShowLeaveFromCommunity((item) => !item)}>{t("Leave")}</button>
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
                            <Members
                                me={customer}
                                users={communityUsers}
                                communityItem={community}
                                removeUsersAsync={removeUsersAsync}
                            />
                        </div>
                    }
                    {itemIndex === 2 &&
                        <>
                            <div className="create-group-chat__item">
                                <AddPeople
                                    customer={customer}
                                    communityUsersId={[customer?.id]}
                                    peopleToJoin={peopleIdToJoin}
                                    setPeopleToJoin={setPeopleIdToJoin}
                                />
                                <ItemConnector
                                    connectorType={0}
                                />
                            </div>
                            <div className={`alert alert-success ${showInvitesSuccess ? "active" : ""}`} role="alert">
                                {t("InviteSuccess")}
                            </div>
                            <div className={`alert alert-warning ${showInvitesFailed ? "active" : ""}`} role="alert">
                                {t("InviteFailed")}
                            </div>
                            <div className="actions">
                                <input type="button" value={t("Apply")} className="btn btn-success" onClick={async () => await createInviteAsync()} />
                            </div>
                        </>
                    }
                    {itemIndex === 4 &&
                        <>
                            <CommunityRulesItem
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