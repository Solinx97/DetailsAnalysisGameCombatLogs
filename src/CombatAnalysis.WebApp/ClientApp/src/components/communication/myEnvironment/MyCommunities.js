import { faArrowsRotate, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import { useSearchByUserIdAsyncQuery } from '../../../store/api/CommunityUser.api';
import CreateCommunity from './CreateCommunity';
import InvitesToCommunity from './InvitesToCommunity';

import '../../../styles/communication/communities.scss';
import MyCommunitiesItem from './MyCommunitiesItem';

const MyCommunities = ({ openCommunity }) => {
    const [, customer] = useAuthentificationAsync();
    const { data: userCommunities, isLoading } = useSearchByUserIdAsyncQuery(customer?.id);

    const [showCreateCommunity, setShowCreateCommunity] = useState(false);
    const [showMyCommunities, setShowMyCommunities] = useState(true);

    if (isLoading) {
        return <>Loading...</>;
    }

    return (
        <div>
            <InvitesToCommunity customer={customer} />
            <div className="communities__list">
                <div className="title">
                    <button type="button" className="btn btn-success" onClick={() => setShowCreateCommunity((item) => !item)}>Create</button>
                    <div className="content">
                        <FontAwesomeIcon icon={faArrowsRotate} title="Refresh" />
                        <div>My communities</div>
                        {showMyCommunities
                            ? <FontAwesomeIcon icon={faEye} title="Hide" onClick={() => setShowMyCommunities((item) => !item)} />
                            : <FontAwesomeIcon icon={faEyeSlash} title="Show" onClick={() => setShowMyCommunities((item) => !item)} />
                        }
                    </div>
                </div>
                <ul>
                    {
                        userCommunities.map((item) => (
                            <li key={item.id}>
                                <MyCommunitiesItem userCommunity={item} openCommunity={openCommunity} />
                            </li>
                        ))
                    }
                </ul>
            </div>
            {showCreateCommunity &&
                <CreateCommunity customer={customer} setShowCreateCommunity={setShowCreateCommunity} />
            }
        </div>
    );
}

export default MyCommunities;