import Loading from '@/shared/components/Loading';
import logger from '@/utils/Logger';
import { useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCommunityUsersByUserIdQuery } from '../../../community/api/CommunityUser.api';
import { useCreateInviteAsyncMutation } from '../../../community/api/InviteToCommunity.api';
import type { InviteToCommunityModel } from '../../../community/types/InviteToCommunityModel';
import type { AppUserModel } from '../../types/AppUserModel';
import TargetCommunity from '../TargetCommunity';

import './PeopleInvitesToCommunity.scss';

interface PeopleInvitesToCommunityProps {
    myself: AppUserModel | null;
    targetUser: AppUserModel;
    setOpenInviteToCommunity: (value: SetStateAction<boolean>) => void;
}

const PeopleInvitesToCommunity: React.FC<PeopleInvitesToCommunityProps> = ({ myself, targetUser, setOpenInviteToCommunity }) => {
    const { t } = useTranslation('communication/people/people');

    const { data: communityUsers, isLoading } = useGetCommunityUsersByUserIdQuery({ appUserId: myself?.id ?? "", page: 1, pageSize: 5 });

    const [communityIdToInvite, setCommunityIdToInvite] = useState<number[]>([]);

    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();

    const createInviteAsync = async () => {
        try {
            for (let i = 0; i < communityIdToInvite.length; i++) {
                const newInviteToCommunity: InviteToCommunityModel = {
                    id: 0,
                    communityId: communityIdToInvite[i],
                    toAppUserId: targetUser.id,
                    when: new Date(),
                    appUserId: myself?.id ?? "",
                }

                await createInviteAsyncMut(newInviteToCommunity).unwrap();
            }

            setOpenInviteToCommunity(false);
        } catch (error) {
            logger.error("Failed create invite to community", error);
        }
    }

    if (!communityUsers || isLoading) {
        return (<Loading />);
    }

    return (
        <div className="invites">
            <div className="title">{t("InviteToCommunity")}</div>
            <ul>
                {
                    communityUsers.users.map(item => (
                        <li key={item.id} className="community">
                            <TargetCommunity
                                communityId={item.communityId}
                                communityIdToInvite={communityIdToInvite}
                                setCommunityIdToInvite={setCommunityIdToInvite}
                            />
                        </li>
                    ))
                }
            </ul>
            <div className="actions">
                <div className="btn-shadow send" onClick={createInviteAsync}>{t("Send")}</div>
                <div className="btn-shadow" onClick={() => setOpenInviteToCommunity((item) => !item)}>{t("Cancel")}</div>
            </div>
        </div>
    );
}

export default PeopleInvitesToCommunity;