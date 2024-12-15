import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useCommunityUserSearchByUserIdQuery } from '../../../store/api/community/CommunityUser.api';
import { useCreateInviteAsyncMutation, useLazyInviteIsExistQuery } from '../../../store/api/community/InviteToCommunity.api';
import Loading from '../../Loading';
import TargetCommunity from './TargetCommunity';

import '../../../styles/communication/people/peopleInvitesToCommunity.scss';

const PeopleInvitesToCommunity = ({ customer, people, setOpenInviteToCommunity }) => {
    const { t } = useTranslation("communication/people/people");

    const { data: communityUsers, isLoading } = useCommunityUserSearchByUserIdQuery(customer?.id);

    const [communityIdToInvite, setCommunityIdToInvite] = useState([]);

    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();
    const [isInviteExistAsync] = useLazyInviteIsExistQuery();

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
        for (let i = 0; i < communityIdToInvite.length; i++) {
            const isExist = await checkIfRequestExistAsync(people.id, communityIdToInvite[i]);
            if (isExist) {
                continue;
            }

            const newInviteToCommunity = {
                communityId: communityIdToInvite[i],
                toCustomerId: people?.id,
                when: new Date(),
                customerId: customer?.id
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
                    communityUsers?.map((item) => (
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
                <div className="btn-shadow send" onClick={async () => await createInviteAsync()}>{t("Send")}</div>
                <div className="btn-shadow" onClick={() => setOpenInviteToCommunity((item) => !item)}>{t("Cancel")}</div>
            </div>
        </div>
    );
}

export default PeopleInvitesToCommunity;