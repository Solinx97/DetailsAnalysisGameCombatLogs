import AddPeople from '@/shared/components/AddPeople';
import logger from '@/utils/Logger';
import type { AppUserModel } from '@/features/user/types/AppUserModel';
import { useState, type SetStateAction } from "react";
import { useTranslation } from 'react-i18next';
import { useUpdateCommunityAsyncMutation } from '../../../api/Community.api';
import { useCreateInviteAsyncMutation } from '../../../api/InviteToCommunity.api';
import type { CommunityModel } from '../../../types/CommunityModel';
import type { InviteToCommunityModel } from '../../../types/InviteToCommunityModel';
import CommonItem from '../../create/CommonItem';
import CommunityRulesItem from '../../create/CommunityRulesItem';
import ItemConnector from '../../create/ItemConnector';
import CommunityMembers from '../CommunityMembers';

const successNotificationTimeout = 2000;
const failedNotificationTimeout = 2000;

interface CommunityMenuRestrictionContentProps {
    itemIndex: number;
    user: AppUserModel;
    community: CommunityModel;
    setCommunity: (value: SetStateAction<CommunityModel | null>) => void;
}

const CommunityMenuRestrictionContent: React.FC<CommunityMenuRestrictionContentProps> = ({ itemIndex, user, community, setCommunity }) => {
    const { t } = useTranslation('communication/community/communityMenu');

    const [communityName, setCommunityName] = useState(community?.name);
    const [communityDescription, setCommunityDescription] = useState(community?.description);
    const [peopleIdToJoin, setPeopleIdToJoin] = useState<AppUserModel[]>([]);
    const [showInvitesSuccess, setShowInvitesSuccess] = useState(false);
    const [showInvitesFailed, setShowInvitesFailed] = useState(false);

    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();
    const [updateCommunityAsyncMut] = useUpdateCommunityAsyncMutation();

    const updateCommunityAsync = async () => {
        try {
            const communityForUpdate = Object.assign({}, community);
            communityForUpdate.name = communityName;
            communityForUpdate.description = communityDescription;

            await updateCommunityAsyncMut({ id: communityForUpdate.id, community: communityForUpdate }).unwrap();
            setCommunity(communityForUpdate);
        } catch (e) {
            logger.error("Failed to update commuity", e);
        }
    }

    const createInviteAsync = async () => {
        try {
            for (let i = 0; i < peopleIdToJoin.length; i++) {
                const newInviteToCommunity: InviteToCommunityModel = {
                    id: 0,
                    communityId: community.id,
                    toAppUserId: peopleIdToJoin[i].id,
                    when: new Date(),
                    appUserId: user?.id
                }

                await createInviteAsyncMut(newInviteToCommunity).unwrap();
                setShowInvitesFailed(true);

                setTimeout(() => {
                    setShowInvitesFailed(false);
                }, failedNotificationTimeout);
            }

            setShowInvitesSuccess(true);

            setTimeout(() => {
                setShowInvitesSuccess(false);
            }, successNotificationTimeout);
        } catch (error) {
            logger.error("Failed to create invite to commuity", error);
        }
    }

    return (
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
                    <CommunityMembers
                        community={community}
                        myself={user}
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
    );
}

export default CommunityMenuRestrictionContent;