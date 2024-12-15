import { faEnvelope, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCommunityByIdQuery } from '../../../store/api/core/Community.api';

const TargetCommunity = ({ communityId, communityIdToInvite, setCommunityIdToInvite }) => {
    const { t } = useTranslation("communication/people/people");

    const { data: community, isLoading } = useGetCommunityByIdQuery(communityId);

    const [isAddedToList, setIsAddedToList] = useState(false);

    const addCommunityToList = () => {
        const communities = communityIdToInvite;
        communities.push(communityId);

        setCommunityIdToInvite(communities);
        setIsAddedToList(true);
    }

    const removeCommunityFromList = () => {
        const communities = communityIdToInvite.filter((item) => item !== communityId);

        setCommunityIdToInvite(communities);
        setIsAddedToList(false);
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="community">
            <div>{community?.name}</div>
            {isAddedToList
                ? <FontAwesomeIcon
                    icon={faEnvelope}
                    title={t("CancelInvite")}
                    onClick={removeCommunityFromList}
                />
                : <FontAwesomeIcon
                    icon={faPlus}
                    title={t("SendInvite")}
                    onClick={addCommunityToList}
                />
            }
        </div>
    );
}

export default TargetCommunity;