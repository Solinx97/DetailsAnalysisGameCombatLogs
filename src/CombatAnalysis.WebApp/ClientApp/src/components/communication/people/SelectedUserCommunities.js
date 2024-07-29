import { useSelector } from 'react-redux';
import { useSearchByUserIdAsyncQuery } from '../../../store/api/communication/community/CommunityUser.api';
import CommunityItem from "../community/CommunityItem";

const SelectedUserCommunities = ({ user, t}) => {
    const me = useSelector((state) => state.user.value);

    const { data: userCommunities, isLoading } = useSearchByUserIdAsyncQuery(user?.id);

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="communities__list">
            {userCommunities?.length === 0
                ? <div>{t("Empty")}</div>
                : <ul>
                    {userCommunities?.map((item) => (
                        <li key={item.id} className="community">
                            <CommunityItem
                                id={item.communityId}
                                me={me}
                            />
                        </li>
                    ))
                    }
                </ul>
            }

        </div>
    );
}

export default SelectedUserCommunities;