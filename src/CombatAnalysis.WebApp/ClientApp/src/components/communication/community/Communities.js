import { faArrowsRotate, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { NavLink } from 'react-router-dom';
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import CommunityService from '../../../services/CommunityService';
import Alert from '../../Alert';
import MyCommunities from '../myEnvironment/MyCommunities';

import '../../../styles/communication/communities.scss';

const Communities = ({ openCommunity }) => {
    const communityService = new CommunityService();

    const [showCommunities, setShowCommunities] = useState(true);
    const [communityList, setCommunityList] = useState(<></>);
    const [userCommunitiesId, setUserCommunitiesId] = useState([]);
    const [showUpdatingAlert, setShowUpdatingAlert] = useState(false);

    const checkAuthentificationAsync = useAuthentificationAsync();

    useEffect(() => {
        let checkAuthentification = async () => {
            await checkAuthentificationAsync();
        }

        let getCommunities = async () => {
            await getCommunitiesAsync();
        }

        checkAuthentification();
        getCommunities();
    }, [])

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
        communityList.key === undefined || communityList.key === null 
            ? setShowUpdatingAlert(false)
            : setShowUpdatingAlert(true);
    }, [communityList])

    const getCommunitiesAsync = async () => {
        const newCommunities = [];

        setShowUpdatingAlert(true);
        const communities = await communityService.getAllAsync();
        for (let i = 0; i < communities.length; i++) {
            if (userCommunitiesId.indexOf(communities[i].id) === -1) {
                newCommunities.push(communities[i]);
            }
        }

        createCommunityList(newCommunities);
    }

    const createCommunityList = (communities) => {
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
                        <NavLink className="card-link" onClick={() => openCommunity(community)}>Open</NavLink>
                        <NavLink className="card-link">More details</NavLink>
                    </div>
                </div>
            </li>
        );
    }

    const render = () => {
        return (
            <div className="communities">
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

    return render();
}

export default Communities;