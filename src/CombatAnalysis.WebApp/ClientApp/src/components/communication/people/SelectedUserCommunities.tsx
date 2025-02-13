import { useSelector } from 'react-redux';
import { useCommunityUserSearchByUserIdQuery } from '../../../store/api/community/CommunityUser.api';
import { CommunityUser } from '../../../types/CommunityUser';
import { SelectedUserCommunitiesProps } from '../../../types/components/communication/people/SelectedUserCommunitiesProps';
import CommunityItem from "../community/CommunityItem";

const SelectedUserCommunities: React.FC<SelectedUserCommunitiesProps> = ({ user, t }) => {
    const me = useSelector((state: any) => state.user.value);

    const { data: communityUsers, isLoading } = useCommunityUserSearchByUserIdQuery(user?.id);

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="communities__list">
            {communityUsers?.length === 0
                ? <div>{t("Empty")}</div>
                : <ul>
                    {communityUsers?.map((communityUser: CommunityUser) => (
                        <li key={communityUser.id} className="community">
                            <CommunityItem
                                id={communityUser.communityId}
                                me={me}
                            />
                        </li>
                    ))}
                </ul>
            }
        </div>
    );
}

export default SelectedUserCommunities;