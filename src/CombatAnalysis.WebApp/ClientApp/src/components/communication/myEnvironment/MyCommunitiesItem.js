import { NavLink } from 'react-router-dom';
import { useGetCommunityByIdQuery } from '../../../store/api/Community.api';

const MyCommunitiesItem = ({ userCommunity }) => {
    const { data: community, isLoading } = useGetCommunityByIdQuery(userCommunity?.communityId);

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="community">
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{community.name}</h5>
                    <p className="card-text">{community.description}</p>
                    <NavLink className="card-link" to={`/community?id=${community.id}`}>Open</NavLink>
                    <NavLink className="card-link">More details</NavLink>
                </div>
            </div>
        </div>
    );
}

export default MyCommunitiesItem;