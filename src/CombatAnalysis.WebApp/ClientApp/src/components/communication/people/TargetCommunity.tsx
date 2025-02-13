import { faEnvelope, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCommunityByIdQuery } from '../../../store/api/core/Community.api';
import { TargetCommunityProps } from '../../../types/components/communication/people/TargetCommunityProps';

const TargetCommunity: React.FC<TargetCommunityProps> = ({ communityId, communityIdToInvite, setCommunityIdToInvite }) => {
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
            <FontAwesomeIcon
                icon={isAddedToList ? faEnvelope : faPlus}
                title={(isAddedToList ? t("CancelInvite") : t("SendInvite")) || ""}
                onClick={isAddedToList ? removeCommunityFromList : addCommunityToList}
            />
        </div>
    );
}

export default TargetCommunity;