import { faArrowsRotate, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { NavLink, useNavigate } from 'react-router-dom';
import { useGetCommunitiesQuery } from '../../../store/api/ChatApi';
import MyCommunities from '../myEnvironment/MyCommunities';

import '../../../styles/communication/communities.scss';

const Communities = () => {
    const [showCommunities, setShowCommunities] = useState(true);
    const [communityList, setCommunityList] = useState(<></>);

    const { data: communities, isLoading } = useGetCommunitiesQuery();

    const navigate = useNavigate();

    useEffect(() => {
        !isLoading && createCommunityList();
    }, [isLoading])

    const createCommunityList = () => {
        const list = communities.length !== 0
            ? communities.map((element) => createCommunityItem(element))
            : (<div className="community__empty">Didn't found any communities</div>);

        setCommunityList(list);
    }

    const createCommunityItem = (community) => {
        return (
            <li key={community.id} className="community">
                <div className="card">
                    <div className="card-body">
                        <h5 className="card-title">{community.name}</h5>
                        <p className="card-text">{community.description}</p>
                        <NavLink className="card-link" onClick={() => navigate(`/community?id=${community.id}`)}>Open</NavLink>
                        <NavLink className="card-link">More details</NavLink>
                    </div>
                </div>
            </li>
        );
    }

    if (isLoading) {
        return <>Loading...</>;
    }

    return (
        <div className="communities">
            <MyCommunities/>
            <div className="communities__list">
                <div className="title">
                    <div className="content">
                        <FontAwesomeIcon
                            icon={faArrowsRotate}
                            title="Refresh"
                            onClick={createCommunityList}
                        />
                        <div>Communities</div>
                        {showCommunities
                            ? <FontAwesomeIcon
                                icon={faEye}
                                title="Hide"
                                onClick={() => setShowCommunities((item) => !item)}
                            />
                            : <FontAwesomeIcon
                                icon={faEyeSlash}
                                title="Show"
                                onClick={() => setShowCommunities((item) => !item)}
                            />
                        }
                    </div>
                </div>
                {showCommunities
                    ? <ul>
                        {communityList}
                    </ul>
                    : null
                }
            </div>
        </div>
    );
}

export default Communities;