import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSearchByUserIdAsyncQuery } from '../../../store/api/CommunityUser.api';
import { useCreateInviteAsyncMutation, useLazyInviteIsExistQuery } from '../../../store/api/InviteToCommunity.api';
import TargetCommunity from './TargetCommunity';

import '../../../styles/communication/people/inviteToCommunity.scss';

const InviteToCommunity = ({ customer, people, setOpenInviteToCommunity }) => {
    const { t } = useTranslation("communication/people/people");

    const { data: communityUsers, isLoading } = useSearchByUserIdAsyncQuery(customer?.id);

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
                result: 0,
                ownerId: customer?.id
            }

            await createInviteAsyncMut(newInviteToCommunity);
        }

        setOpenInviteToCommunity(false);
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="invite-to-community">
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
                <input type="button" value={t("Send")} className="btn btn-success" onClick={async () => await createInviteAsync()} />
                <input type="button" value={t("Cancel")} className="btn btn-secondary" onClick={() => setOpenInviteToCommunity((item) => !item)} />
            </div>
        </div>
    );
}

export default InviteToCommunity;