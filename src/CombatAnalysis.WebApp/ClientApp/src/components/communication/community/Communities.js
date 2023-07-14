import { faArrowsRotate, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { NavLink } from 'react-router-dom';
import MyCommunities from '../myEnvironment/MyCommunities';
import Alert from '../../Alert';

import '../../../styles/communication/communities.scss';

const Communities = ({ openCommunity }) => {
    const [showCommunities, setShowCommunities] = useState(true);
    const [communities, setCommunities] = useState(<></>);
    const [userCommunitiesId, setUserCommunitiesId] = useState([]);
    const [showUpdatingAlert, setShowUpdatingAlert] = useState(false);

    useEffect(() => {
        if (userCommunitiesId.length === 0) {
            return;
        }

        let getCommunities = async () => {
            await getCommunitiesAsync();
        }

        getCommunities();
    }, [userCommunitiesId])

    useEffect(() => {
        communities.key === undefined || communities.key === null 
            ? setShowUpdatingAlert(false)
            : setShowUpdatingAlert(true);
    }, [communities])

    const getCommunitiesAsync = async () => {
        const newCommunities = [];

        setShowUpdatingAlert(true);
        const response = await fetch('api/v1/Community');
        if (response.status !== 200) {
            return;
        }

        const communities = await response.json();
        for (var i = 0; i < communities.length; i++) {
            if (userCommunitiesId.indexOf(communities[i].id) === -1) {
                newCommunities.push(communities[i]);
            }
        }

        fillCommunities(newCommunities);
    }

    const fillCommunities = (communities) => {
        const list = communities.length !== 0
            ? communities.map((element) => createCommunityCard(element))
            : (<div className="community__empty">Didn't found any communities</div>);

        setCommunities(list);
    }

    const createCommunityCard = (community) => {
        return (<li key={community.id} className="community">
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{community.name}</h5>
                    <p className="card-text">{community.description}</p>
                    <NavLink className="card-link" onClick={() => openCommunity(community)}>Open</NavLink>
                    <NavLink className="card-link">More details</NavLink>
                </div>
            </div>
        </li>);
    }

    const handleCommunityVisibility = () => {
        setShowCommunities(!showCommunities);
    }

    const render = () => {
        return (<div className="communities">
            <Alert
                isVisible={showUpdatingAlert}
                content="Updating..."
            />
            <MyCommunities
                openCommunity={openCommunity}
                setUserCommunitiesId={setUserCommunitiesId}
            />
            <div className="communities__list">
                <div className="title">
                    <div className="content">
                        <FontAwesomeIcon
                            icon={faArrowsRotate}
                            title="Refresh"
                            onClick={async () => await getCommunitiesAsync()}
                        />
                        <div>Communities</div>
                        {showCommunities
                            ? <FontAwesomeIcon
                                icon={faEye}
                                title="Hide"
                                onClick={handleCommunityVisibility}
                            />
                            : <FontAwesomeIcon
                                icon={faEyeSlash}
                                title="Show"
                                onClick={handleCommunityVisibility}
                            />
                        }
                    </div>
                </div>
                {showCommunities
                    ? <ul>
                        {communities}
                    </ul>
                    : null
                }
            </div>
        </div>);
    }

    return render();
}

export default Communities;