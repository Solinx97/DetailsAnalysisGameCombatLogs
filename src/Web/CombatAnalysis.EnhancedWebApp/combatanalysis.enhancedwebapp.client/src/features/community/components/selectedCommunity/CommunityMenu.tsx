import AddPeople from '@/shared/components/AddPeople';
import logger from '@/utils/Logger';
import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type SetStateAction } from "react";
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import GroupChatMembers from '../../../chat/components/groupChat/GroupChatMembers';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useRemoveCommunityAsyncMutation, useUpdateCommunityAsyncMutation } from '../../api/Community.api';
import { useLazyCommunityUserFindByUserIdQuery, useRemoveCommunityUserMutation } from '../../api/CommunityUser.api';
import { useCreateInviteAsyncMutation, useLazyInviteIsExistQuery } from '../../api/InviteToCommunity.api';
import type { CommunityModel } from '../../types/CommunityModel';
import type { CommunityUserModel } from '../../types/CommunityUserModel';
import type { InviteToCommunityModel } from '../../types/InviteToCommunityModel';
import CommonItem from '../create/CommonItem';
import CommunityRulesItem from '../create/CommunityRulesItem';
import ItemConnector from '../create/ItemConnector';

import './CommunityMenu.scss';

const successNotificationTimeout = 2000;
const failedNotificationTimeout = 2000;

interface CommunityMenuProps {
    setShowMenu: (value: SetStateAction<boolean>) => void;
    user: AppUserModel;
    community: CommunityModel;
    setCommunity: (value: SetStateAction<CommunityModel | null>) => void;
}

const CommunityMenu: React.FC<CommunityMenuProps> = ({ setShowMenu, user, community, setCommunity }) => {
    const { t } = useTranslation('communication/community/communityMenu');

    const navigate = useNavigate();

    const [itemIndex, seItemIndex] = useState(0);
    const [communityName, setCommunityName] = useState(community?.name);
    const [communityDescription, setCommunityDescription] = useState(community?.description);
    const [showLeaveFromCommunity, setShowLeaveFromCommunity] = useState(false);
    const [peopleIdToJoin, setPeopleIdToJoin] = useState<AppUserModel[]>([]);
    const [showInvitesSuccess, setShowInvitesSuccess] = useState(false);
    const [showInvitesFailed, setShowInvitesFailed] = useState(false);

    const [removeCommunityAsync] = useRemoveCommunityAsyncMutation();
    const [findByUserIdAsync] = useLazyCommunityUserFindByUserIdQuery();
    const [removeCommunityUserAsync] = useRemoveCommunityUserMutation();
    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();
    const [isInviteExistAsync] = useLazyInviteIsExistQuery();
    const [updateCommunityAsyncMut] = useUpdateCommunityAsyncMutation();

    const leaveFromCommunityAsync = async () => {
        try {
            const myCommunities = await findByUserIdAsync(user?.id).unwrap();

            const meInCommunity = myCommunities.filter(x => x.communityId === community?.id)[0];
            if (!meInCommunity) {
                throw new Error();
            }

            await removeCommunityUserAsync(meInCommunity.id).unwrap();
            navigate('/communities');
        } catch (e) {
            logger.error("Failed to leave from community", e);
        }
    }

    const ownerLeaveFromCommunityAsync = async () => {
        try {
            await removeCommunityAsync(community.id).unwrap();
            navigate('/communities');
        } catch (e) {
            logger.error("Failed to leave from community", e);
        }
    }

    const updateCommunityAsync = async () => {
        try {
            const communityForUpdate = Object.assign({}, community);
            communityForUpdate.name = communityName;
            communityForUpdate.description = communityDescription;

            await updateCommunityAsyncMut(communityForUpdate).unwrap();
            setCommunity(communityForUpdate);
        } catch (e) {
            logger.error("Failed to update commuity", e);
        }
    }

    const changeMenuItem = (index: number) => {
        seItemIndex(index);
    }

    const checkIfRequestExistAsync = async (appUserId: string, communityId: number) => {
        try {
            const isExist = await isInviteExistAsync({ appUserId: appUserId, communityId: communityId }).unwrap();
            return isExist;
        } catch (e) {
            logger.error("Failed to check if request to join to community exist", e);
        }
    }

    const createInviteAsync = async () => {
        for (let i = 0; i < peopleIdToJoin.length; i++) {
            const isExist = await checkIfRequestExistAsync(peopleIdToJoin[i].id, community.id);
            if (isExist) {
                continue;
            }

            const newInviteToCommunity: InviteToCommunityModel = {
                id: 0,
                communityId: community.id,
                toAppUserId: peopleIdToJoin[i].id,
                when: new Date(),
                appUserId: user?.id
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

    const removeUsersAsync = async (peopleToRemove: CommunityUserModel[]) => {
        try {
            for (let i = 0; i < peopleToRemove.length; i++) {
                await removeCommunityUserAsync(peopleToRemove[i].id).unwrap();
            }
        } catch (e) {
            logger.error("Failed to remove user from commuity", e);
        }
    }

    const canRemovePeople = () => {
        return false;
    }

    return (
        <div className="communication-content community-menu box-shadow">
            {showLeaveFromCommunity &&
                <div className="leave-from-community">
                    <div className="leave-from-community__title">{t("LeaveAlert")}</div>
                    <div>
                        <div>{t("LeaveConfirm")}?</div>
                    </div>
                    {user.id === community.appUserId
                        ? <>
                            <div className="alert alert-danger" role="alert">
                                {t("LeaveOwnerConfirm")}
                            </div>
                            <div className="actions">
                                <button className="btn btn-outline-danger" onClick={ownerLeaveFromCommunityAsync}>{t("Leave")}</button>
                                <button className="btn btn-outline-success" onClick={() => setShowLeaveFromCommunity((item) => !item)}>{t("Cancel")}</button>
                            </div>
                        </>
                        : <div className="actions">
                            <button className="btn btn-outline-danger" onClick={leaveFromCommunityAsync}>{t("Leave")}</button>
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
                    {community?.appUserId === user?.id &&
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
                                <div className="btn-shadow" onClick={updateCommunityAsync}>{t("Update")}</div>
                            </div>
                        </>
                    }
                    {itemIndex === 1 &&
                        <div className="members">
                            <GroupChatMembers
                                communicationId={community.id}
                                isPopup={false}
                                removeUsersAsync={removeUsersAsync}
                                canRemovePeople={canRemovePeople}
                                chatHub={null}
                            />
                        </div>
                    }
                    {itemIndex === 2 &&
                        <>
                            <>
                                <AddPeople
                                    usersId={[user?.id]}
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
                                <div className="btn-shadow" onClick={createInviteAsync}>{t("Apply")}</div>
                            </div>
                        </>
                    }
                    {itemIndex === 4 &&
                        <>
                            <CommunityRulesItem
                                t={t}
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
                        title={t("Close") || ""}
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