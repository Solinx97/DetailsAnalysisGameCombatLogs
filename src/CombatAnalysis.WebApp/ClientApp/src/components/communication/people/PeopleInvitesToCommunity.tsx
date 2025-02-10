import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useCommunityUserSearchByUserIdQuery } from '../../../store/api/community/CommunityUser.api';
import { useCreateInviteAsyncMutation, useLazyInviteIsExistQuery } from '../../../store/api/community/InviteToCommunity.api';
import { PeopleInvitesToCommunityProps } from '../../../types/components/communication/people/PeopleInvitesToCommunityProps';
import Loading from '../../Loading';
import TargetCommunity from './TargetCommunity';

import '../../../styles/communication/people/peopleInvitesToCommunity.scss';

const PeopleInvitesToCommunity: React.FC<PeopleInvitesToCommunityProps> = ({ me, targetUser, setOpenInviteToCommunity }) => {
    const { t } = useTranslation("communication/people/people");

    const { data: communityUsers, isLoading } = useCommunityUserSearchByUserIdQuery(me?.id);

    const [communityIdToInvite, setCommunityIdToInvite] = useState([]);

    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();
    const [isInviteExistAsync] = useLazyInviteIsExistQuery();

    const checkIfRequestExistAsync = async (peopleId: string, communityId: number) => {
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
        for (let i = 0; i < communityIdToInvite.length; i++) {
            const isExist = await checkIfRequestExistAsync(targetUser.id, communityIdToInvite[i]);
            if (isExist) {
                continue;
            }

            const newInviteToCommunity = {
                communityId: communityIdToInvite[i],
                toCustomerId: targetUser?.id,
                when: new Date(),
                appUserId: me?.id
            }

            await createInviteAsyncMut(newInviteToCommunity);
        }

        setOpenInviteToCommunity(false);
    }

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <div className="invites">
            <div className="title">{t("InviteToCommunity")}</div>
            <ul>
                {
                    communityUsers?.map((item :any) => (
                        <li key={item.id}>
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