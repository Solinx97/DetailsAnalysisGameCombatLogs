import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useCommunityUserSearchByCommunityIdQuery, useLazyCommunityUserSearchByUserIdQuery, useRemoveCommunityUserMutation } from '../../../store/api/community/CommunityUser.api';
import { useCreateInviteAsyncMutation, useLazyInviteIsExistQuery } from '../../../store/api/community/InviteToCommunity.api';
import { useRemoveCommunityAsyncMutation, useUpdateCommunityAsyncMutation } from '../../../store/api/core/Community.api';
import AddPeople from '../../AddPeople';
import Loading from '../../Loading';
import Members from '../Members';
import CommonItem from "../create/CommonItem";
import CommunityRulesItem from "../create/CommunityRulesItem";
import ItemConnector from '../create/ItemConnector';

import '../../../styles/communication/community/communityMenu.scss';

const successNotificationTimeout = 2000;
const failedNotificationTimeout = 2000;

const CommunityMenu = ({ setShowMenu, customer, community, setCommunity }) => {
    const { t } = useTranslation("communication/community/communityMenu");

    const navigate = useNavigate();

    const [itemIndex, seItemIndex] = useState(0);
    const [communityName, setCommunityName] = useState(community?.name);
    const [communityDescription, setCommunityDescription] = useState(community?.description);
    const [showLeaveFromCommunity, setShowLeaveFromCommunity] = useState(false);
    const [peopleIdToJoin, setPeopleIdToJoin] = useState([]);
    const [showInvitesSuccess, setShowInvitesSuccess] = useState(false);
    const [showInvitesFailed, setShowInvitesFailed] = useState(false);

    const [removeCommunityAsync] = useRemoveCommunityAsyncMutation();
    const [searchByUserIdAsync] = useLazyCommunityUserSearchByUserIdQuery();
    const [removeCommunityUserAsync] = useRemoveCommunityUserMutation();
    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();
    const [isInviteExistAsync] = useLazyInviteIsExistQuery();
    const [updateCommunityAsyncMut] = useUpdateCommunityAsyncMutation();
    const { data: communityUsers, isLoading } = useCommunityUserSearchByCommunityIdQuery(community?.id);

    const leaveFromCommunityAsync = async () => {
        const myCommunityUserId = await searchByUserIdAsync(customer?.id);
        if (myCommunityUserId.error !== undefined) {
            return;
        }

        const meInCommunity = myCommunityUserId.data.filter(x => x.communityId === community?.id)[0];

        const deletedItemCount = await removeCommunityUserAsync(meInCommunity.id);
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

    const updateCommunityAsync = async () => {
        const communityForUpdate = Object.assign({}, community);
        communityForUpdate.name = communityName;
        communityForUpdate.description = communityDescription;

        const updated = await updateCommunityAsyncMut(communityForUpdate);
        if (updated.data !== undefined) {
            setCommunity(communityForUpdate);
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
        return (<Loading />);
    }

    return (
        <div className="communication-content community-menu box-shadow">
            {showLeaveFromCommunity &&
                <div className="leave-from-community">
                    <div className="leave-from-community__title">{t("LeaveAlert")}</div>
                    <div>
                        <div>{t("LeaveConfirm")}?</div>
                    </div>
                    {customer.id === community.customerId
                        ? <>
                            <div className="alert alert-danger" role="alert">
                                {t("LeaveOwnerConfirm")}
                            </div>
                            <div className="actions">
                                <button className="btn btn-outline-danger" onClick={async () => await ownerLeaveFromCommunityAsync()}>{t("Leave")}</button>
                                <button className="btn btn-outline-success" onClick={() => setShowLeaveFromCommunity((item) => !item)}>{t("Cancel")}</button>
                            </div>
                        </>
                        : <div className="actions">
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
                    {community?.customerId === customer?.id &&
                        <li className="menu-item" onClick={() => changeMenuItem(4)}>
                            {itemIndex === 4 &&
                                <FontAwesomeIcon
                                    className="menu-item__passed"
                                    icon={faCircleCheck}
                                />
                            }
                            <div>{t("Rules")}</div>
                        </li>
                    }
                    <li className="menu-item__leave">
                        <div className="btn-shadow" onClick={() => setShowLeaveFromCommunity((item) => !item)}>{t("Leave")}</div>
                    </li>
                </ul>
                <div className="community-menu__items">
                    {itemIndex === 0 &&
                        <>
                            <CommonItem
                                name={communityName}
                                setName={setCommunityName}
                                description={communityDescription}
                                setDescription={setCommunityDescription}
                                useDescription={true}
                                connector={
                                    <ItemConnector
                                        connectorType={0}
                                    />
                                }
                            />
                            <div className="actions">
                                <div className="btn-shadow" onClick={async () => await updateCommunityAsync()}>{t("Update")}</div>
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
                            <>
                                <AddPeople
                                    customer={customer}
                                    communityUsersId={[customer?.id]}
                                    peopleToJoin={peopleIdToJoin}
                                    setPeopleToJoin={setPeopleIdToJoin}
                                />
                                <ItemConnector
                                    connectorType={0}
                                />
                            </>
                            {showInvitesSuccess &&
                                <div className="alert alert-success" role="alert">
                                    {t("InviteSuccess")}
                                </div>
                            }
                            {showInvitesFailed &&
                                <div className="alert alert-warning " role="alert">
                                    {t("InviteFailed")}
                                </div>
                            }
                            <div className="actions">
                                <div className="btn-shadow" onClick={async () => await createInviteAsync()}>{t("Apply")}</div>
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
                                <div className="btn-shadow">{t("Update")}</div>
                            </div>
                        </>
                    }
                </div>
                <div className="close">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title={t("Close")}
                        onClick={() => setShowMenu(false)}
                    />
                </div>
            </div>
            <div className="finish-create">
                <div className="btn-shadow" onClick={() => setShowMenu(false)}>{t("Cancel")}</div>
            </div>
        </div>
    );
}

export default CommunityMenu;